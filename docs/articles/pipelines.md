# Pipelines API Guide

The Pipelines API allows you to manage sales pipelines and opportunities in Less Annoying CRM. Pipelines help track deals through various stages from lead to close.

## Overview

The Enduro.Lacrm library provides seven pipeline-related functions:

- `CreatePipeline` - Add a contact to a pipeline
- `UpdatePipelineItem` - Update pipeline item status and fields
- `GetPipelineItem` - Retrieve a single pipeline item
- `DeletePipelineItem` - Remove a contact from a pipeline
- `GetPipelineItemsAttachedToContact` - Get all pipeline items for a contact
- `GetPipelineReport` - Get pipeline report with filtering
- `GetPipelineSettings` - Retrieve pipeline configuration

## Understanding Pipelines

### Pipeline Structure

- **Pipeline**: A sales process (e.g., "Sales Pipeline", "Partnerships")
- **Status**: A stage in the pipeline (e.g., "Lead", "Qualified", "Closed Won")
- **Pipeline Item**: A contact's position in a pipeline

### Getting Pipeline Configuration

Before working with pipelines, retrieve available pipelines and statuses:

```csharp
var response = await lacrm.GetPipelineSettings();

foreach (var pipeline in response.Pipelines)
{
    Console.WriteLine($"Pipeline: {pipeline.Name} (ID: {pipeline.PipelineId})");
    
    foreach (var status in pipeline.Statuses)
    {
        Console.WriteLine($"  Status: {status.Name} (ID: {status.StatusId})");
    }
}
```

## Creating Pipeline Items

### Add Contact to Pipeline

```csharp
var response = await lacrm.CreatePipeline(
    contactId: "contact-id-here",
    pipelineId: "pipeline-id",
    statusId: "status-id-initial",
    note: "New lead from website contact form"
);

Console.WriteLine($"Created pipeline item: {response.PipelineItemId}");
```

### Create with Priority

Priority is an integer (typically 1-5, with 1 being highest):

```csharp
var response = await lacrm.CreatePipeline(
    contactId: "contact-id-here",
    pipelineId: "pipeline-id",
    statusId: "status-id",
    note: "Hot lead - immediate follow up required",
    priority: 1  // Highest priority
);
```

### Create with Custom Fields

```csharp
var customFields = new Dictionary<string, string>
{
    { "Deal Size", "50000" },
    { "Expected Close", "2024-12-31" },
    { "Product Interest", "Enterprise Plan" }
};

var response = await lacrm.CreatePipeline(
    contactId: "contact-id-here",
    pipelineId: "pipeline-id",
    statusId: "status-id",
    note: "Qualified opportunity",
    priority: 2,
    customFields: customFields
);
```

## Updating Pipeline Items

### Move to Next Stage

```csharp
var response = await lacrm.UpdatePipelineItem(
    pipelineItemId: "item-id-here",
    statusId: "status-id-qualified",
    note: "Qualified - budget confirmed"
);
```

### Update Priority

```csharp
await lacrm.UpdatePipelineItem(
    pipelineItemId: "item-id-here",
    statusId: "current-status-id",
    priority: 1  // Increase priority
);
```

### Update Custom Fields

```csharp
var customFields = new Dictionary<string, string>
{
    { "Deal Size", "75000" },  // Updated deal size
    { "Expected Close", "2024-11-30" }
};

await lacrm.UpdatePipelineItem(
    pipelineItemId: "item-id-here",
    statusId: "current-status-id",
    customFields: customFields
);
```

### Add Note to Pipeline Item

```csharp
await lacrm.UpdatePipelineItem(
    pipelineItemId: "item-id-here",
    statusId: "current-status-id",
    note: "Follow-up call completed. Client requesting proposal."
);
```

### Mark as Closed Won

```csharp
// Get pipeline settings to find "Closed Won" status ID
var settings = await lacrm.GetPipelineSettings();
var salesPipeline = settings.Pipelines
    .First(p => p.Name == "Sales Pipeline");
var closedWonStatus = salesPipeline.Statuses
    .First(s => s.Name == "Closed Won");

await lacrm.UpdatePipelineItem(
    pipelineItemId: "item-id-here",
    statusId: closedWonStatus.StatusId,
    note: "Deal closed! Contract signed.",
    priority: null  // Clear priority
);
```

## Retrieving Pipeline Items

### Get Single Pipeline Item

```csharp
var response = await lacrm.GetPipelineItem("item-id-here");
var item = response.PipelineItem;

Console.WriteLine($"Contact: {item.ContactName}");
Console.WriteLine($"Status: {item.StatusName}");
Console.WriteLine($"Priority: {item.Priority}");
Console.WriteLine($"Note: {item.Note}");
```

### Get Pipeline Items for Contact

```csharp
var response = await lacrm.GetPipelineItemsAttachedToContact("contact-id-here");

foreach (var item in response.Result)
{
    Console.WriteLine($"{item.PipelineName}: {item.StatusName}");
    if (item.Priority.HasValue)
    {
        Console.WriteLine($"  Priority: {item.Priority}");
    }
}
```

### Get Pipeline Report

```csharp
using Enduro.Lacrm.Parameters;

var response = await lacrm.GetPipelineReport(
    pipelineId: "pipeline-id",
    sortBy: "Priority",
    numRows: 50,
    page: 1
);

Console.WriteLine($"Total items in pipeline: {response.TotalResults}");

foreach (var item in response.Result)
{
    Console.WriteLine($"{item.ContactName} - {item.StatusName} (Priority: {item.Priority})");
}
```

### Filter Pipeline Report

```csharp
var response = await lacrm.GetPipelineReport(
    pipelineId: "pipeline-id",
    sortBy: "StatusOrder",
    sortDirection: "ASC",
    statusFilter: "status-id-qualified",  // Only qualified leads
    userFilter: "USER123"  // Only items assigned to specific user
);
```

### Sort Options

Available sort options for pipeline reports:

- `"Priority"` - Sort by priority
- `"StatusOrder"` - Sort by status position in pipeline
- `"ContactName"` - Sort by contact name
- `"DateCreated"` - Sort by creation date
- `"DateModified"` - Sort by last modification date

```csharp
// Get highest priority items first
var highPriority = await lacrm.GetPipelineReport(
    pipelineId: "pipeline-id",
    sortBy: "Priority",
    sortDirection: "ASC"  // Ascending (1, 2, 3...)
);
```

## Deleting Pipeline Items

Remove a contact from a pipeline:

```csharp
var response = await lacrm.DeletePipelineItem("item-id-here");

if (response.Success)
{
    Console.WriteLine("Pipeline item deleted");
}
```

## Complete Pipeline Management Example

```csharp
using Enduro.Lacrm;
using Enduro.Lacrm.Exceptions;

public class PipelineManager
{
    private readonly LacrmClient _lacrm;
    private string _salesPipelineId;
    private Dictionary<string, string> _statusIds;

    public PipelineManager(LacrmClient lacrm)
    {
        _lacrm = lacrm;
        _statusIds = new Dictionary<string, string>();
    }

    public async Task Initialize()
    {
        var settings = await _lacrm.GetPipelineSettings();
        var salesPipeline = settings.Pipelines
            .First(p => p.Name == "Sales Pipeline");
        
        _salesPipelineId = salesPipeline.PipelineId;
        
        foreach (var status in salesPipeline.Statuses)
        {
            _statusIds[status.Name] = status.StatusId;
        }
    }

    public async Task<string> AddNewLead(
        string contactId, 
        int dealSize, 
        string source)
    {
        var customFields = new Dictionary<string, string>
        {
            { "Deal Size", dealSize.ToString() },
            { "Lead Source", source }
        };

        var response = await _lacrm.CreatePipeline(
            contactId: contactId,
            pipelineId: _salesPipelineId,
            statusId: _statusIds["Lead"],
            note: $"New lead from {source}",
            priority: 3,
            customFields: customFields
        );

        return response.PipelineItemId;
    }

    public async Task MoveThroughPipeline(string pipelineItemId)
    {
        // Lead → Qualified
        await _lacrm.UpdatePipelineItem(
            pipelineItemId,
            _statusIds["Qualified"],
            note: "Budget and need confirmed"
        );

        await Task.Delay(1000);

        // Qualified → Proposal
        await _lacrm.UpdatePipelineItem(
            pipelineItemId,
            _statusIds["Proposal"],
            note: "Proposal sent"
        );

        await Task.Delay(1000);

        // Proposal → Closed Won
        await _lacrm.UpdatePipelineItem(
            pipelineItemId,
            _statusIds["Closed Won"],
            note: "Contract signed!",
            priority: null
        );
    }

    public async Task<List<PipelineItem>> GetHighPriorityDeals()
    {
        var response = await _lacrm.GetPipelineReport(
            pipelineId: _salesPipelineId,
            sortBy: "Priority",
            sortDirection: "ASC",
            numRows: 10
        );

        return response.Result
            .Where(i => i.Priority.HasValue && i.Priority <= 2)
            .ToList();
    }

    public async Task GeneratePipelineSummary()
    {
        var response = await _lacrm.GetPipelineReport(
            pipelineId: _salesPipelineId,
            sortBy: "StatusOrder",
            numRows: 500
        );

        var summary = response.Result
            .GroupBy(i => i.StatusName)
            .Select(g => new
            {
                Status = g.Key,
                Count = g.Count(),
                TotalValue = g.Sum(i => 
                {
                    if (i.CustomFields?.ContainsKey("Deal Size") == true &&
                        int.TryParse(i.CustomFields["Deal Size"], out int value))
                        return value;
                    return 0;
                })
            });

        Console.WriteLine("Pipeline Summary:");
        foreach (var stage in summary)
        {
            Console.WriteLine($"{stage.Status}: {stage.Count} deals, " +
                            $"${stage.TotalValue:N0} total value");
        }
    }
}
```

## Pipeline Item Model Properties

| Property | Type | Description |
|----------|------|-------------|
| `PipelineItemId` | `string?` | Unique item identifier |
| `PipelineId` | `string?` | Pipeline identifier |
| `PipelineName` | `string?` | Pipeline name |
| `ContactId` | `string?` | Associated contact ID |
| `ContactName` | `string?` | Contact name |
| `StatusId` | `string?` | Current status ID |
| `StatusName` | `string?` | Current status name |
| `Priority` | `int?` | Priority (1-5, lower is higher) |
| `Note` | `string?` | Notes about the item |
| `DateCreated` | `string?` | Creation date |
| `DateModified` | `string?` | Last modification date |
| `CustomFields` | `Dictionary<string, string>?` | Custom field values |

## Best Practices

1. **Initialize settings**: Cache pipeline and status IDs to avoid repeated lookups
2. **Use meaningful notes**: Document why status changes occur
3. **Set appropriate priorities**: Use priority consistently across team
4. **Track custom fields**: Use custom fields for deal size, close date, etc.
5. **Regular cleanup**: Remove or archive old pipeline items
6. **Monitor reports**: Use pipeline reports for forecasting
7. **Automate updates**: Update pipeline items when creating tasks/events
8. **Handle pagination**: Pipeline reports can be large, use pagination

## Advanced Patterns

### Automated Stage Progression

```csharp
public async Task AutoProgressOnTaskComplete(string pipelineItemId, string taskId)
{
    var task = await _lacrm.GetTask(taskId);
    
    if (task.Task.IsCompleted == true)
    {
        await _lacrm.UpdatePipelineItem(
            pipelineItemId,
            _statusIds["Next Stage"],
            note: $"Automatically progressed due to task completion: {task.Task.Name}"
        );
    }
}
```

### Deal Scoring

```csharp
public int CalculateDealScore(PipelineItem item)
{
    var score = 0;
    
    // Priority contributes to score
    if (item.Priority.HasValue)
        score += (6 - item.Priority.Value) * 10;
    
    // Deal size contributes
    if (item.CustomFields?.ContainsKey("Deal Size") == true &&
        int.TryParse(item.CustomFields["Deal Size"], out int dealSize))
    {
        score += dealSize / 1000;  // 1 point per $1000
    }
    
    return score;
}
```

## See Also

- [Getting Started](getting-started.md)
- [Contacts API](contacts.md)
- [Tasks API](tasks.md)
- [Error Handling](error-handling.md)
- [Pagination](pagination.md)
