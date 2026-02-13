# Migration Guide: v1.0.1 to v2.0.0

This guide helps you upgrade from Enduro.Lacrm v1.0.1 to v2.0.0. Version 2.0 introduces 30+ new API functions while maintaining backward compatibility with existing code.

## What's New in v2.0.0

### Major Features

- **30+ New Functions**: Complete CRUD operations for tasks, events, notes, pipelines, and groups
- **Enhanced Task Management**: Edit, delete, retrieve single/multiple tasks
- **Complete Event Operations**: Full lifecycle management for calendar events
- **Note Management**: Edit, delete, and retrieve notes
- **Pipeline Operations**: Get and delete pipeline items
- **Group Management**: Complete group CRUD operations

### Breaking Changes

**None.** Version 2.0.0 is fully backward compatible. All existing v1.x code will continue to work without modifications.

## Installation

Update your package reference:

```xml
<PackageReference Include="Enduro.Lacrm" Version="2.0.0" />
```

Or via command line:

```bash
dotnet add package Enduro.Lacrm --version 2.0.0
```

## New API Functions

### Tasks (6 functions total)

#### Previously Available (v1.x)
- `CreateTask` ✓ (no changes)

#### New in v2.0
- `EditTask` - Update existing tasks
- `DeleteTask` - Remove tasks
- `GetTask` - Retrieve single task by ID
- `GetTasks` - Retrieve multiple tasks with filtering
- `GetTasksAttachedToContact` - Get all tasks for a contact

**Migration Example:**

```csharp
// v1.x - Could only create tasks
var response = await lacrm.CreateTask(
    contactId, dueDate, name, description);

// v2.0 - Can now edit them
var editParams = new EditTaskParams(response.TaskId)
{
    Name = "Updated Task Name",
    IsCompleted = true
};
await lacrm.EditTask(editParams);

// v2.0 - And retrieve them
var task = await lacrm.GetTask(response.TaskId);
Console.WriteLine($"Task status: {task.Task.IsCompleted}");

// v2.0 - Or delete them
await lacrm.DeleteTask(response.TaskId);
```

### Events (6 functions total)

#### Previously Available (v1.x)
- `CreateEvent` ✓ (no changes)

#### New in v2.0
- `EditEvent` - Update existing events
- `DeleteEvent` - Remove events
- `GetEvent` - Retrieve single event by ID
- `GetEvents` - Retrieve multiple events with filtering
- `GetEventsAttachedToContact` - Get all events for a contact

**Migration Example:**

```csharp
// v1.x - Could only create events
var response = await lacrm.CreateEvent(
    date, startTime, endTime, name);

// v2.0 - Can now reschedule them
var editParams = new EditEventParams(response.EventId)
{
    Date = "2024-12-20",
    StartTime = "15:00",
    EndTime = "16:00"
};
await lacrm.EditEvent(editParams);

// v2.0 - Retrieve event details
var evt = await lacrm.GetEvent(response.EventId);

// v2.0 - Get all events for a contact
var contactEvents = await lacrm.GetEventsAttachedToContact(contactId);
```

### Notes (6 functions total)

#### Previously Available (v1.x)
- `CreateNote` ✓ (no changes)

#### New in v2.0
- `EditNote` - Update existing notes
- `DeleteNote` - Remove notes
- `GetNote` - Retrieve single note by ID
- `GetNotes` - Retrieve multiple notes with filtering
- `GetNotesAttachedToContact` - Get all notes for a contact

**Migration Example:**

```csharp
// v1.x - Could only create notes
var response = await lacrm.CreateNote(contactId, noteText);

// v2.0 - Can now edit them
var editParams = new EditNoteParams(response.NoteId)
{
    Note = noteText + "\n\nUpdated: " + DateTime.Today
};
await lacrm.EditNote(editParams);

// v2.0 - Retrieve note history
var notes = await lacrm.GetNotesAttachedToContact(contactId);
foreach (var note in notes.Result)
{
    Console.WriteLine($"{note.DateCreated}: {note.NoteText}");
}

// v2.0 - Delete old notes
await lacrm.DeleteNote(oldNoteId);
```

### Pipelines (7 functions total)

#### Previously Available (v1.x)
- `CreatePipeline` ✓ (no changes)
- `UpdatePipelineItem` ✓ (no changes)
- `GetPipelineItemsAttachedToContact` ✓ (no changes)
- `GetPipelineReport` ✓ (no changes)
- `GetPipelineSettings` ✓ (no changes)

#### New in v2.0
- `GetPipelineItem` - Retrieve single pipeline item
- `DeletePipelineItem` - Remove from pipeline

**Migration Example:**

```csharp
// v1.x - Could create and update
var response = await lacrm.CreatePipeline(
    contactId, pipelineId, statusId);

// v2.0 - Can now get individual items
var item = await lacrm.GetPipelineItem(response.PipelineItemId);
Console.WriteLine($"Current status: {item.PipelineItem.StatusName}");

// v2.0 - And delete them
await lacrm.DeletePipelineItem(response.PipelineItemId);
```

### Groups (5 functions total)

#### Previously Available (v1.x)
- `AddContactGroup` ✓ (no changes)

#### New in v2.0
- `GetGroups` - List all groups
- `CreateGroup` - Create new groups
- `DeleteGroup` - Remove groups
- `RemoveContactFromGroup` - Remove contact from group

**Migration Example:**

```csharp
// v1.x - Could only add contacts to existing groups
await lacrm.AddContactGroup(contactId, "VIP");

// v2.0 - Can now create groups programmatically
await lacrm.CreateGroup("VIP Customers");
await lacrm.CreateGroup("Premium Tier");

// v2.0 - List all available groups
var groups = await lacrm.GetGroups();
foreach (var group in groups.Groups)
{
    Console.WriteLine($"- {group.Name}");
}

// v2.0 - Remove contacts from groups
await lacrm.RemoveContactFromGroup(contactId, "Old Group");

// v2.0 - Clean up unused groups
await lacrm.DeleteGroup("Archived Campaign");
```

### Contacts (5 functions total)

#### No changes from v1.x
- `CreateContact` ✓
- `GetContact` ✓
- `EditContact` ✓
- `DeleteContact` ✓
- `SearchContacts` ✓

All contact functions remain unchanged and fully compatible.

### Other Functions

#### No changes from v1.x
- `GetUserInfo` ✓
- `GetCustomFields` ✓

## Upgrade Patterns

### Pattern 1: Complete Task Lifecycle

In v1.x, you could only create tasks. In v2.0, you can manage their entire lifecycle:

```csharp
// v1.x workflow
var task = await lacrm.CreateTask(contactId, dueDate, name, desc);
// That's it - couldn't do anything else

// v2.0 enhanced workflow
var task = await lacrm.CreateTask(contactId, dueDate, name, desc);

// Update it
await lacrm.EditTask(new EditTaskParams(task.TaskId) 
{ 
    IsCompleted = true 
});

// Check status
var current = await lacrm.GetTask(task.TaskId);

// Get all tasks
var allTasks = await lacrm.GetTasksAttachedToContact(contactId);

// Clean up completed tasks
if (current.Task.IsCompleted == true)
{
    await lacrm.DeleteTask(task.TaskId);
}
```

### Pattern 2: Event Management

```csharp
// v1.x - Create and forget
await lacrm.CreateEvent(date, start, end, name);

// v2.0 - Full lifecycle
var evt = await lacrm.CreateEvent(date, start, end, name);

// Reschedule if needed
await lacrm.EditEvent(new EditEventParams(evt.EventId)
{
    Date = newDate,
    StartTime = newTime
});

// Review upcoming events
var upcoming = await lacrm.GetEvents(new GetEventsParams
{
    MinDate = DateTime.Today.ToString("yyyy-MM-dd"),
    MaxDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd")
});

// Cancel event
await lacrm.DeleteEvent(evt.EventId);
```

### Pattern 3: Note History

```csharp
// v1.x - Just add notes
await lacrm.CreateNote(contactId, "Initial contact");
await lacrm.CreateNote(contactId, "Follow-up call");

// v2.0 - Track and manage history
var notes = await lacrm.GetNotesAttachedToContact(contactId);

// View timeline
foreach (var note in notes.Result.OrderByDescending(n => n.DateCreated))
{
    Console.WriteLine($"{note.DateCreated}: {note.NoteText}");
}

// Update a note
var latestNote = notes.Result.OrderByDescending(n => n.DateCreated).First();
await lacrm.EditNote(new EditNoteParams(latestNote.NoteId)
{
    Note = latestNote.NoteText + "\n\nUpdated with additional info"
});

// Remove old notes
var oldNotes = notes.Result
    .Where(n => DateTime.Parse(n.DateCreated) < DateTime.Today.AddYears(-1));
foreach (var old in oldNotes)
{
    await lacrm.DeleteNote(old.NoteId);
}
```

### Pattern 4: Group Management

```csharp
// v1.x - Limited to adding to existing groups
await lacrm.AddContactGroup(contactId, "Customers");

// v2.0 - Full group lifecycle
// Create groups dynamically
await lacrm.CreateGroup($"Q4 {DateTime.Today.Year} Leads");

// Get all groups to see what exists
var groups = await lacrm.GetGroups();
var activeGroups = groups.Groups
    .Where(g => !g.Name.Contains("Archive"))
    .ToList();

// Move contact between groups
await lacrm.RemoveContactFromGroup(contactId, "Leads");
await lacrm.AddContactGroup(contactId, "Customers");

// Cleanup old groups
await lacrm.DeleteGroup("2023 Campaign");
```

## Common Upgrade Scenarios

### Scenario 1: Task Dashboard

**v1.x Implementation:**

```csharp
// Could only create tasks, had to track them externally
public class TaskDashboard
{
    private readonly LacrmClient _lacrm;
    private readonly List<string> _taskIds = new();  // Manual tracking

    public async Task CreateTask(string contactId, string name)
    {
        var task = await _lacrm.CreateTask(contactId, 
            DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"), 
            name, "");
        _taskIds.Add(task.TaskId);  // Manual tracking
    }
}
```

**v2.0 Improvement:**

```csharp
// Can retrieve and manage tasks directly from API
public class TaskDashboard
{
    private readonly LacrmClient _lacrm;

    public async Task<List<Task>> GetMyTasks()
    {
        var userInfo = await _lacrm.GetUserInfo();
        var tasks = await _lacrm.GetTasks(new GetTasksParams
        {
            AssignedTo = userInfo.UserCode,
            IncludeCompleted = false
        });
        return tasks.Result.ToList();
    }

    public async Task CompleteTask(string taskId)
    {
        await _lacrm.EditTask(new EditTaskParams(taskId)
        {
            IsCompleted = true
        });
    }
}
```

### Scenario 2: Contact Timeline

**v1.x - Limited View:**

```csharp
// Could only add new items, not view history
await lacrm.CreateNote(contactId, "Met at conference");
await lacrm.CreateTask(contactId, dueDate, "Follow up", "");
await lacrm.CreateEvent(date, start, end, "Meeting");
```

**v2.0 - Complete Timeline:**

```csharp
public async Task<List<TimelineItem>> GetContactTimeline(string contactId)
{
    var timeline = new List<TimelineItem>();

    // Get all notes
    var notes = await _lacrm.GetNotesAttachedToContact(contactId);
    timeline.AddRange(notes.Result.Select(n => new TimelineItem
    {
        Date = n.DateCreated,
        Type = "Note",
        Description = n.NoteText
    }));

    // Get all tasks
    var tasks = await _lacrm.GetTasksAttachedToContact(contactId);
    timeline.AddRange(tasks.Result.Select(t => new TimelineItem
    {
        Date = t.DateCreated,
        Type = "Task",
        Description = t.Name
    }));

    // Get all events
    var events = await _lacrm.GetEventsAttachedToContact(contactId);
    timeline.AddRange(events.Result.Select(e => new TimelineItem
    {
        Date = e.Date,
        Type = "Event",
        Description = e.Name
    }));

    return timeline.OrderByDescending(t => t.Date).ToList();
}
```

### Scenario 3: Pipeline Cleanup

**v1.x - No cleanup capability:**

```csharp
// Could only add items to pipeline, not remove them
await lacrm.CreatePipeline(contactId, pipelineId, statusId);
```

**v2.0 - Full Management:**

```csharp
public async Task CleanupClosedDeals(string pipelineId)
{
    // Get pipeline report
    var report = await _lacrm.GetPipelineReport(
        pipelineId, "DateModified", numRows: 500);

    // Find old closed deals
    var oldClosed = report.Result
        .Where(i => i.StatusName.Contains("Closed") &&
                   DateTime.Parse(i.DateModified) < DateTime.Today.AddMonths(-6));

    // Remove them
    foreach (var item in oldClosed)
    {
        await _lacrm.DeletePipelineItem(item.PipelineItemId);
        Console.WriteLine($"Archived: {item.ContactName}");
    }
}
```

## Testing Your Migration

After upgrading, test these scenarios:

1. **Existing functionality still works**
   ```csharp
   // These should work exactly as before
   await lacrm.CreateContact(parameters);
   await lacrm.SearchContacts("test");
   await lacrm.CreateTask(contactId, date, name, desc);
   ```

2. **New functions are accessible**
   ```csharp
   // These are now available
   await lacrm.GetTasks(new GetTasksParams());
   await lacrm.EditEvent(eventParams);
   await lacrm.GetGroups();
   ```

3. **Error handling remains consistent**
   ```csharp
   try
   {
       await lacrm.EditTask(params);
   }
   catch (ValidationException ex) { }
   catch (ApiException ex) { }
   catch (HttpException ex) { }
   ```

## Performance Improvements

Version 2.0 enables more efficient patterns:

**Before (v1.x):**
```csharp
// Had to search to find items
var allContacts = await lacrm.SearchContacts("*", numRows: 1000);
// Then filter client-side - inefficient
var myContacts = allContacts.Result.Where(c => /* criteria */);
```

**After (v2.0):**
```csharp
// Can retrieve specific items directly
var tasks = await lacrm.GetTasks(new GetTasksParams
{
    AssignedTo = userCode,
    MinDueDate = startDate,
    MaxDueDate = endDate
});
```

## API Function Count Summary

| Category | v1.0.1 | v2.0.0 | New |
|----------|--------|--------|-----|
| Contacts | 5 | 5 | 0 |
| Tasks | 1 | 6 | +5 |
| Events | 1 | 6 | +5 |
| Notes | 1 | 6 | +5 |
| Pipelines | 5 | 7 | +2 |
| Groups | 1 | 5 | +4 |
| Other | 2 | 2 | 0 |
| **Total** | **16** | **37** | **+21** |

## Support

If you encounter issues during migration:

1. Check this guide for upgrade patterns
2. Review the [API documentation](../api/index.md)
3. See specific guides: [Tasks](tasks.md), [Events](events.md), [Notes](notes.md), [Groups](groups.md)
4. Report issues on [GitHub](https://github.com/yourusername/Enduro-Lacrm/issues)

## See Also

- [Getting Started](getting-started.md)
- [Error Handling](error-handling.md)
- [Pagination](pagination.md)
- [Tasks API Guide](tasks.md)
- [Events API Guide](events.md)
- [Notes API Guide](notes.md)
