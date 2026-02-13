# Pagination

Many LACRM API endpoints return large result sets that require pagination. This guide explains how to effectively work with paginated data in the Enduro.Lacrm library.

## Understanding Pagination

Pagination splits large result sets into smaller "pages" to improve performance and reduce memory usage. The LACRM API uses page-based pagination with configurable page sizes.

### Key Concepts

- **Page**: A subset of results (e.g., results 1-100)
- **Page Number**: Which page to retrieve (1-based indexing)
- **Page Size**: Number of results per page (MaxNumberOfResults)
- **Total Results**: Total count across all pages

## Paginated Endpoints

The following endpoints support pagination:

- `GetTasks` - Retrieve multiple tasks
- `GetTasksAttachedToContact` - Tasks for a contact
- `GetEvents` - Retrieve multiple events
- `GetEventsAttachedToContact` - Events for a contact
- `GetNotes` - Retrieve multiple notes
- `GetNotesAttachedToContact` - Notes for a contact
- `GetPipelineReport` - Pipeline items
- `SearchContacts` - Contact search results

## Basic Pagination

### Single Page Request

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new GetTasksParams
{
    MaxNumberOfResults = 50,  // Page size
    Page = 1,                 // First page
    IncludeCompleted = false
};

var response = await lacrm.GetTasks(parameters);

Console.WriteLine($"Total results: {response.TotalResults}");
Console.WriteLine($"Current page: {response.Page}");
Console.WriteLine($"Results on this page: {response.Result.Count()}");
```

### Default Page Sizes

Different endpoints have different defaults:

```csharp
// Tasks attached to contact - default 500
var tasks = await lacrm.GetTasksAttachedToContact("contact-id");

// Events attached to contact - default 500
var events = await lacrm.GetEventsAttachedToContact("contact-id");

// Notes attached to contact - default 500
var notes = await lacrm.GetNotesAttachedToContact("contact-id");

// With explicit page size
var tasksPaginated = await lacrm.GetTasksAttachedToContact(
    contactId: "contact-id",
    maxNumberOfResults: 25,
    page: 1
);
```

## Iterating Through Pages

### Manual Pagination

```csharp
var page = 1;
var pageSize = 100;
var allTasks = new List<Enduro.Lacrm.Models.Task>();

while (true)
{
    var parameters = new GetTasksParams
    {
        MaxNumberOfResults = pageSize,
        Page = page,
        IncludeCompleted = false
    };

    var response = await lacrm.GetTasks(parameters);
    allTasks.AddRange(response.Result);

    Console.WriteLine($"Retrieved page {page}: {response.Result.Count()} tasks");

    // Check if we've reached the last page
    if (response.Result.Count() < pageSize)
        break;

    page++;
}

Console.WriteLine($"Total tasks retrieved: {allTasks.Count}");
```

### Generic Pagination Helper

```csharp
public async Task<List<T>> GetAllPages<T>(
    Func<int, int, Task<(IEnumerable<T> Items, int Total)>> fetchPage,
    int pageSize = 100)
{
    var allItems = new List<T>();
    var page = 1;

    while (true)
    {
        var (items, total) = await fetchPage(page, pageSize);
        var itemsList = items.ToList();
        
        allItems.AddRange(itemsList);

        if (itemsList.Count < pageSize)
            break;

        page++;
    }

    return allItems;
}

// Usage
var allTasks = await GetAllPages<Enduro.Lacrm.Models.Task>(
    async (page, pageSize) =>
    {
        var parameters = new GetTasksParams
        {
            MaxNumberOfResults = pageSize,
            Page = page
        };
        var response = await lacrm.GetTasks(parameters);
        return (response.Result, response.TotalResults ?? 0);
    }
);
```

### IAsyncEnumerable Pattern

For .NET Core 3.0+, use async streams:

```csharp
public async IAsyncEnumerable<Enduro.Lacrm.Models.Task> GetAllTasksAsync(
    GetTasksParams baseParams,
    int pageSize = 100)
{
    var page = 1;

    while (true)
    {
        baseParams.MaxNumberOfResults = pageSize;
        baseParams.Page = page;

        var response = await _lacrm.GetTasks(baseParams);

        foreach (var task in response.Result)
        {
            yield return task;
        }

        if (response.Result.Count() < pageSize)
            break;

        page++;
    }
}

// Usage
await foreach (var task in GetAllTasksAsync(new GetTasksParams()))
{
    Console.WriteLine($"Task: {task.Name}");
}
```

## Pagination Strategies

### Load All at Once

Best for: Small to medium datasets (< 1000 items)

```csharp
public async Task<List<Note>> GetAllNotes(string contactId)
{
    var allNotes = new List<Note>();
    var page = 1;

    while (true)
    {
        var response = await _lacrm.GetNotesAttachedToContact(
            contactId,
            maxNumberOfResults: 100,
            page: page
        );

        allNotes.AddRange(response.Result);

        if (response.Result.Count() < 100)
            break;

        page++;
    }

    return allNotes;
}
```

### Lazy Loading

Best for: Large datasets, memory-constrained environments

```csharp
public class PaginatedTaskLoader
{
    private readonly LacrmClient _lacrm;
    private readonly GetTasksParams _params;
    private int _currentPage = 0;
    private List<Enduro.Lacrm.Models.Task> _currentBatch;

    public PaginatedTaskLoader(LacrmClient lacrm, GetTasksParams parameters)
    {
        _lacrm = lacrm;
        _params = parameters;
        _currentBatch = new List<Enduro.Lacrm.Models.Task>();
    }

    public async Task<List<Enduro.Lacrm.Models.Task>> GetNextPage()
    {
        _currentPage++;
        _params.Page = _currentPage;
        _params.MaxNumberOfResults = 50;

        var response = await _lacrm.GetTasks(_params);
        _currentBatch = response.Result.ToList();

        return _currentBatch;
    }

    public bool HasMorePages => _currentBatch.Count >= 50;
}

// Usage
var loader = new PaginatedTaskLoader(lacrm, new GetTasksParams());

while (await loader.GetNextPage() is var tasks && tasks.Any())
{
    foreach (var task in tasks)
    {
        Console.WriteLine($"Processing: {task.Name}");
    }

    if (!loader.HasMorePages)
        break;
}
```

### Parallel Page Loading

Best for: Maximum speed with adequate resources

```csharp
public async Task<List<T>> GetAllPagesParallel<T>(
    Func<int, Task<IEnumerable<T>>> fetchPage,
    int totalPages)
{
    var tasks = Enumerable.Range(1, totalPages)
        .Select(page => fetchPage(page))
        .ToArray();

    var results = await Task.WhenAll(tasks);

    return results.SelectMany(r => r).ToList();
}

// First, get total count
var firstPage = await lacrm.GetTasks(new GetTasksParams 
{ 
    MaxNumberOfResults = 100, 
    Page = 1 
});

var totalPages = (int)Math.Ceiling(
    (firstPage.TotalResults ?? 0) / 100.0);

// Load all pages in parallel
var allTasks = await GetAllPagesParallel(
    async page =>
    {
        var response = await lacrm.GetTasks(new GetTasksParams
        {
            MaxNumberOfResults = 100,
            Page = page
        });
        return response.Result;
    },
    totalPages
);
```

## Contact-Specific Pagination

### Get All Tasks for Contact

```csharp
public async Task<List<Enduro.Lacrm.Models.Task>> GetAllContactTasks(
    string contactId)
{
    var allTasks = new List<Enduro.Lacrm.Models.Task>();
    var page = 1;

    while (true)
    {
        var response = await _lacrm.GetTasksAttachedToContact(
            contactId,
            maxNumberOfResults: 100,
            page: page
        );

        allTasks.AddRange(response.Result);

        if (response.Result.Count() < 100)
            break;

        page++;
    }

    return allTasks;
}
```

### Get All Notes for Contact

```csharp
public async Task<List<Note>> GetAllContactNotes(string contactId)
{
    var allNotes = new List<Note>();
    var page = 1;

    while (true)
    {
        var response = await _lacrm.GetNotesAttachedToContact(
            contactId,
            maxNumberOfResults: 100,
            page: page
        );

        allNotes.AddRange(response.Result);

        if (response.Result.Count() < 100)
            break;

        page++;
    }

    return allNotes;
}
```

## Pipeline Report Pagination

```csharp
public async Task<List<PipelineItem>> GetCompletePipelineReport(
    string pipelineId,
    string sortBy = "Priority")
{
    var allItems = new List<PipelineItem>();
    var page = 1;

    while (true)
    {
        var response = await _lacrm.GetPipelineReport(
            pipelineId: pipelineId,
            sortBy: sortBy,
            numRows: 100,
            page: page
        );

        allItems.AddRange(response.Result);

        if (response.Result.Count() < 100)
            break;

        page++;
    }

    return allItems;
}
```

## Progress Reporting

### With Progress Callback

```csharp
public async Task<List<T>> GetAllWithProgress<T>(
    Func<int, int, Task<(IEnumerable<T> Items, int Total)>> fetchPage,
    Action<int, int> onProgress,
    int pageSize = 100)
{
    var allItems = new List<T>();
    var page = 1;
    var totalProcessed = 0;
    var total = 0;

    while (true)
    {
        var (items, totalCount) = await fetchPage(page, pageSize);
        var itemsList = items.ToList();
        
        allItems.AddRange(itemsList);
        totalProcessed += itemsList.Count;
        total = totalCount;

        onProgress(totalProcessed, total);

        if (itemsList.Count < pageSize)
            break;

        page++;
    }

    return allItems;
}

// Usage
var tasks = await GetAllWithProgress<Enduro.Lacrm.Models.Task>(
    async (page, pageSize) =>
    {
        var parameters = new GetTasksParams
        {
            MaxNumberOfResults = pageSize,
            Page = page
        };
        var response = await lacrm.GetTasks(parameters);
        return (response.Result, response.TotalResults ?? 0);
    },
    (processed, total) =>
    {
        var percent = (processed * 100) / total;
        Console.WriteLine($"Progress: {processed}/{total} ({percent}%)");
    }
);
```

## Performance Considerations

### Optimal Page Sizes

```csharp
// Small pages - more requests, less memory
var tasks1 = await lacrm.GetTasksAttachedToContact(
    "contact-id",
    maxNumberOfResults: 25  // Good for UI pagination
);

// Medium pages - balanced
var tasks2 = await lacrm.GetTasksAttachedToContact(
    "contact-id",
    maxNumberOfResults: 100  // Good for batch processing
);

// Large pages - fewer requests, more memory
var tasks3 = await lacrm.GetTasksAttachedToContact(
    "contact-id",
    maxNumberOfResults: 500  // Good for bulk operations
);
```

### Caching Strategy

```csharp
public class CachedPaginationService
{
    private readonly Dictionary<string, (DateTime Cached, List<Task> Data)> _cache;
    private readonly TimeSpan _cacheExpiry = TimeSpan.FromMinutes(5);

    public async Task<List<Task>> GetTasksCached(string contactId)
    {
        var cacheKey = $"tasks_{contactId}";

        if (_cache.TryGetValue(cacheKey, out var cached) &&
            DateTime.UtcNow - cached.Cached < _cacheExpiry)
        {
            return cached.Data;
        }

        var tasks = await GetAllContactTasks(contactId);
        _cache[cacheKey] = (DateTime.UtcNow, tasks);

        return tasks;
    }
}
```

## Best Practices

1. **Choose appropriate page sizes**: Balance between request count and memory usage
2. **Handle empty pages**: Check for zero results
3. **Respect API limits**: Don't use excessively large page sizes
4. **Use parallel loading carefully**: May hit rate limits
5. **Cache when appropriate**: Reduce redundant API calls
6. **Show progress**: For large datasets, keep users informed
7. **Handle errors per page**: Don't let one page failure stop entire operation
8. **Consider memory**: Don't load massive datasets into memory at once
9. **Test with large datasets**: Ensure pagination works at scale
10. **Use async streams**: For processing items as they arrive

## Common Patterns

### Process Items as They Load

```csharp
public async Task ProcessTasksInPages(
    string contactId,
    Func<Task, Task> processor)
{
    var page = 1;

    while (true)
    {
        var response = await _lacrm.GetTasksAttachedToContact(
            contactId,
            maxNumberOfResults: 50,
            page: page
        );

        foreach (var task in response.Result)
        {
            await processor(task);
        }

        if (response.Result.Count() < 50)
            break;

        page++;
    }
}

// Usage
await ProcessTasksInPages("contact-id", async task =>
{
    Console.WriteLine($"Processing: {task.Name}");
    // Do something with each task
    await Task.Delay(10);  // Simulate processing
});
```

### Batch Processing with Delays

```csharp
public async Task<List<T>> GetAllWithDelay<T>(
    Func<int, Task<IEnumerable<T>>> fetchPage,
    int maxPages,
    int delayMs = 1000)
{
    var allItems = new List<T>();

    for (int page = 1; page <= maxPages; page++)
    {
        var items = await fetchPage(page);
        allItems.AddRange(items);

        if (page < maxPages)
            await Task.Delay(delayMs);  // Rate limiting
    }

    return allItems;
}
```

## See Also

- [Getting Started](getting-started.md)
- [Tasks API](tasks.md)
- [Events API](events.md)
- [Notes API](notes.md)
- [Error Handling](error-handling.md)
