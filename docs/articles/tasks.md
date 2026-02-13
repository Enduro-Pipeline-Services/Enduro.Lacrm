# Tasks API Guide

The Tasks API allows you to create, edit, retrieve, and delete tasks (to-dos) in Less Annoying CRM. Tasks are activities assigned to users with due dates.

## Overview

The Enduro.Lacrm library provides six task-related functions:

- `CreateTask` - Create a new task
- `EditTask` - Update an existing task
- `DeleteTask` - Remove a task
- `GetTask` - Retrieve a single task by ID
- `GetTasks` - Retrieve multiple tasks with filtering
- `GetTasksAttachedToContact` - Get all tasks for a specific contact

## Creating Tasks

### Basic Task Creation

```csharp
var response = await lacrm.CreateTask(
    contactId: "1234567890",
    dueDate: "2024-12-31",
    name: "Follow up call",
    description: "Quarterly check-in with client"
);

Console.WriteLine($"Created task ID: {response.TaskId}");
```

### Task with Assignment

Assign a task to a specific user by providing their User Code:

```csharp
var response = await lacrm.CreateTask(
    contactId: "1234567890",
    dueDate: "2024-12-31",
    name: "Review contract",
    description: "Review and approve new contract terms",
    assignedTo: "USER123"  // User Code of the assignee
);
```

### Date Format

Due dates must be in `YYYY-MM-DD` format:

```csharp
// Correct formats
"2024-12-31"
"2024-01-01"
"2024-06-15"

// Using DateTime
var dueDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");
var response = await lacrm.CreateTask(contactId, dueDate, name, description);
```

## Editing Tasks

Use `EditTask` to update any field of an existing task. Only provide the fields you want to change.

### Update Task Name and Due Date

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new EditTaskParams("task-id-here")
{
    Name = "Updated task name",
    DueDate = "2025-01-15"
};

var response = await lacrm.EditTask(parameters);
Console.WriteLine($"Task updated: {response.Success}");
```

### Mark Task as Complete

```csharp
var parameters = new EditTaskParams("task-id-here")
{
    IsCompleted = true
};

await lacrm.EditTask(parameters);
```

### Reassign Task

```csharp
var parameters = new EditTaskParams("task-id-here")
{
    AssignedTo = "USER456"  // New assignee's User Code
};

await lacrm.EditTask(parameters);
```

### Update Description

```csharp
var parameters = new EditTaskParams("task-id-here")
{
    Description = "Updated description with additional context"
};

await lacrm.EditTask(parameters);
```

### Move Task to Different Contact

```csharp
var parameters = new EditTaskParams("task-id-here")
{
    ContactId = "new-contact-id"
};

await lacrm.EditTask(parameters);
```

### Update Multiple Fields

```csharp
var parameters = new EditTaskParams("task-id-here")
{
    Name = "Revised task name",
    DueDate = "2025-02-01",
    Description = "Updated description",
    AssignedTo = "USER789",
    IsCompleted = false
};

await lacrm.EditTask(parameters);
```

## Retrieving Tasks

### Get a Single Task

Retrieve complete details for one task:

```csharp
var response = await lacrm.GetTask("task-id-here");
var task = response.Task;

Console.WriteLine($"Task: {task.Name}");
Console.WriteLine($"Due: {task.DueDate}");
Console.WriteLine($"Assigned to: {task.AssignedToMetaData?.FirstName} {task.AssignedToMetaData?.LastName}");
Console.WriteLine($"Contact: {task.ContactMetaData?.Name}");
Console.WriteLine($"Completed: {task.IsCompleted}");
Console.WriteLine($"Description: {task.Description}");
```

### Get Multiple Tasks with Filters

Use `GetTasks` with parameters to filter and sort tasks:

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new GetTasksParams
{
    AssignedTo = "USER123",          // Filter by assignee
    MaxNumberOfResults = 50,          // Limit results
    Page = 1,                         // Page number
    IncludeCompleted = false,         // Exclude completed tasks
    MinDueDate = "2024-12-01",       // Tasks due after this date
    MaxDueDate = "2024-12-31"        // Tasks due before this date
};

var response = await lacrm.GetTasks(parameters);
foreach (var task in response.Result)
{
    Console.WriteLine($"{task.DueDate}: {task.Name} ({task.ContactMetaData?.Name})");
}
```

### Filter by Due Date Range

Get tasks due in the next 7 days:

```csharp
var today = DateTime.Today;
var nextWeek = today.AddDays(7);

var parameters = new GetTasksParams
{
    MinDueDate = today.ToString("yyyy-MM-dd"),
    MaxDueDate = nextWeek.ToString("yyyy-MM-dd"),
    IncludeCompleted = false
};

var response = await lacrm.GetTasks(parameters);
Console.WriteLine($"Tasks due in next 7 days: {response.Result.Count()}");
```

### Get Overdue Tasks

```csharp
var parameters = new GetTasksParams
{
    MaxDueDate = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"),
    IncludeCompleted = false
};

var response = await lacrm.GetTasks(parameters);
Console.WriteLine($"Overdue tasks: {response.Result.Count()}");
```

### Get Tasks Assigned to Current User

```csharp
var userInfo = await lacrm.GetUserInfo();
var userCode = userInfo.UserCode;

var parameters = new GetTasksParams
{
    AssignedTo = userCode,
    IncludeCompleted = false
};

var response = await lacrm.GetTasks(parameters);
```

## Get Tasks for a Contact

Retrieve all tasks associated with a specific contact:

### Basic Usage

```csharp
var response = await lacrm.GetTasksAttachedToContact("contact-id-here");

foreach (var task in response.Result)
{
    var status = task.IsCompleted == true ? "✓" : "○";
    Console.WriteLine($"{status} {task.Name} (Due: {task.DueDate})");
}
```

### With Pagination

```csharp
var response = await lacrm.GetTasksAttachedToContact(
    contactId: "contact-id-here",
    maxNumberOfResults: 25,
    page: 1
);

Console.WriteLine($"Total tasks: {response.TotalResults}");
Console.WriteLine($"Current page: {response.Page}");
Console.WriteLine($"Tasks on this page: {response.Result.Count()}");
```

### Iterate Through All Pages

```csharp
var contactId = "contact-id-here";
var page = 1;
var allTasks = new List<Enduro.Lacrm.Models.Task>();

while (true)
{
    var response = await lacrm.GetTasksAttachedToContact(
        contactId,
        maxNumberOfResults: 100,
        page: page
    );

    allTasks.AddRange(response.Result);

    if (response.Result.Count() < 100)
        break;  // Last page

    page++;
}

Console.WriteLine($"Total tasks for contact: {allTasks.Count}");
```

## Deleting Tasks

Remove a task permanently:

```csharp
var response = await lacrm.DeleteTask("task-id-here");

if (response.Success)
{
    Console.WriteLine("Task deleted successfully");
}
```

### Delete with Error Handling

```csharp
try
{
    await lacrm.DeleteTask("task-id-here");
    Console.WriteLine("Task deleted");
}
catch (ApiException ex)
{
    Console.WriteLine($"Failed to delete task: {ex.Message}");
}
```

## Complete Task Management Example

Here's a complete example demonstrating task lifecycle management:

```csharp
using Enduro.Lacrm;
using Enduro.Lacrm.Parameters;
using Enduro.Lacrm.Exceptions;

public class TaskManager
{
    private readonly LacrmClient _lacrm;

    public TaskManager(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task ManageTaskLifecycle(string contactId)
    {
        try
        {
            // 1. Create a task
            var createResponse = await _lacrm.CreateTask(
                contactId: contactId,
                dueDate: DateTime.Today.AddDays(7).ToString("yyyy-MM-dd"),
                name: "Client follow-up",
                description: "Discuss renewal options"
            );

            var taskId = createResponse.TaskId;
            Console.WriteLine($"Created task: {taskId}");

            // 2. Get the task
            var getResponse = await _lacrm.GetTask(taskId);
            Console.WriteLine($"Task name: {getResponse.Task.Name}");

            // 3. Update the task
            var editParams = new EditTaskParams(taskId)
            {
                DueDate = DateTime.Today.AddDays(3).ToString("yyyy-MM-dd"),
                Description = "Urgent: Discuss renewal options ASAP"
            };

            await _lacrm.EditTask(editParams);
            Console.WriteLine("Task updated");

            // 4. Mark as complete
            var completeParams = new EditTaskParams(taskId)
            {
                IsCompleted = true
            };

            await _lacrm.EditTask(completeParams);
            Console.WriteLine("Task marked complete");

            // 5. Optional: Delete the task
            // await _lacrm.DeleteTask(taskId);
        }
        catch (ValidationException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"API error: {ex.Message}");
        }
    }

    public async Task<List<Enduro.Lacrm.Models.Task>> GetUpcomingTasks(int days)
    {
        var parameters = new GetTasksParams
        {
            MinDueDate = DateTime.Today.ToString("yyyy-MM-dd"),
            MaxDueDate = DateTime.Today.AddDays(days).ToString("yyyy-MM-dd"),
            IncludeCompleted = false,
            MaxNumberOfResults = 100
        };

        var response = await _lacrm.GetTasks(parameters);
        return response.Result.ToList();
    }
}
```

## Task Model Properties

The `Task` model includes the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `TaskId` | `string?` | Unique task identifier |
| `Name` | `string?` | Task name/title |
| `DueDate` | `string?` | Due date (YYYY-MM-DD) |
| `AssignedTo` | `string?` | User Code of assignee |
| `Description` | `string?` | Task description |
| `ContactId` | `string?` | Associated contact ID |
| `IsCompleted` | `bool?` | Completion status |
| `DateCompleted` | `string?` | Completion date |
| `CalendarId` | `string?` | Calendar identifier |
| `DateCreated` | `string?` | Creation date |
| `AssignedToMetaData` | `AssignedToMetaData?` | Assignee details (FirstName, LastName) |
| `ContactMetaData` | `ContactMetaData?` | Contact details (Name, AssignedTo) |

## Best Practices

1. **Use meaningful task names**: Keep names concise but descriptive
2. **Set realistic due dates**: Consider workload when assigning dates
3. **Add detailed descriptions**: Include context and action items
4. **Filter appropriately**: Use date ranges and filters to avoid large result sets
5. **Handle pagination**: Use pagination for contacts with many tasks
6. **Mark tasks complete**: Update `IsCompleted` instead of deleting to maintain history
7. **Use cancellation tokens**: For operations that may take time

## See Also

- [Getting Started](getting-started.md)
- [Contacts API](contacts.md)
- [Events API](events.md)
- [Error Handling](error-handling.md)
- [Pagination](pagination.md)
