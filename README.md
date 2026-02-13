# Less Annoying CRM .NET API Wrapper

[![NuGet](https://img.shields.io/nuget/v/Enduro.Lacrm.svg)](https://www.nuget.org/packages/Enduro.Lacrm/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A comprehensive, strongly-typed .NET wrapper for the [Less Annoying CRM API](https://lessannoyingcrm.com/help/topic/API) with over 50 functions covering all major CRM operations.

## Features

### 🚀 Complete API Coverage

- **37 API Functions** covering all major CRM operations
- **Strongly Typed** with full IntelliSense support
- **Async/Await** throughout for modern C# development
- **XML Documentation** on all public APIs

### 📋 Comprehensive Functionality

#### Contacts (5 functions)
- Create, retrieve, edit, and delete contacts
- Search contacts with advanced filtering
- Support for people and companies
- Multiple contact methods (email, phone, address, website)
- Custom fields support

#### Tasks (6 functions)
- Create, edit, and delete tasks
- Get single task or multiple tasks with filtering
- Retrieve all tasks for a contact
- Date range filtering and pagination
- Task assignment and completion tracking

#### Events (6 functions)
- Create, edit, and delete calendar events
- Get single event or multiple events with filtering
- Retrieve all events for a contact
- Support for multiple attendees (contacts and users)
- Date/time range filtering

#### Notes (6 functions)
- Create, edit, and delete notes
- Get single note or multiple notes with filtering
- Retrieve all notes for a contact
- Date range filtering and pagination
- Full note history tracking

#### Pipelines (7 functions)
- Add contacts to pipelines
- Update pipeline item status and priority
- Get individual pipeline items
- Delete pipeline items
- Get pipeline items for contact
- Comprehensive pipeline reports with filtering
- Retrieve pipeline settings and configuration

#### Groups (5 functions)
- List all groups
- Create and delete groups
- Add contacts to groups
- Remove contacts from groups
- Flexible contact categorization

#### Other (2 functions)
- Get user information
- Retrieve custom fields

### 🛡️ Robust Error Handling

- **ValidationException** - Parameter validation before API calls
- **ApiException** - API-level errors with detailed messages
- **HttpException** - HTTP communication errors
- Comprehensive error information for troubleshooting

### 📖 Extensive Documentation

- [Complete Documentation](docs/index.md)
- [Getting Started Guide](docs/articles/getting-started.md)
- API-specific guides:
  - [Contacts API](docs/articles/contacts.md)
  - [Tasks API](docs/articles/tasks.md)
  - [Events API](docs/articles/events.md)
  - [Notes API](docs/articles/notes.md)
  - [Pipelines API](docs/articles/pipelines.md)
  - [Groups API](docs/articles/groups.md)
- [Error Handling Guide](docs/articles/error-handling.md)
- [Pagination Guide](docs/articles/pagination.md)
- [Migration Guide v1 to v2](docs/articles/migration-v1-to-v2.md)

## Installation

Install via NuGet:

```bash
dotnet add package Enduro.Lacrm
```

Or via Package Manager:

```powershell
Install-Package Enduro.Lacrm
```

## Quick Start

### Basic Setup

```csharp
using Enduro.Lacrm;
using System.Net.Http;

var client = new HttpClient();
var options = new Options
{
    ApiToken = "YOUR_API_TOKEN",
    UserCode = "YOUR_USER_CODE",
    ApiUrl = "https://api.lessannoyingcrm.com"
};

var lacrm = new LacrmClient(client, options);
```

### Search for Contacts

```csharp
var response = await lacrm.SearchContacts("John Doe");
foreach (var contact in response.Result)
{
    Console.WriteLine($"{contact.FullName} - {contact.Email?.FirstOrDefault()?.Text}");
}
```

### Create and Manage Tasks

```csharp
// Create a task
var createResponse = await lacrm.CreateTask(
    contactId: "12345",
    dueDate: "2024-12-31",
    name: "Follow up call",
    description: "Discuss Q4 results"
);

// Edit the task
var editParams = new EditTaskParams(createResponse.TaskId)
{
    IsCompleted = true
};
await lacrm.EditTask(editParams);

// Get all tasks for a contact
var tasks = await lacrm.GetTasksAttachedToContact("12345");
```

### Manage Events

```csharp
// Create an event
var eventResponse = await lacrm.CreateEvent(
    date: "2024-12-15",
    startTime: "14:00",
    endTime: "15:00",
    name: "Client Meeting",
    description: "Quarterly review",
    contacts: new[] { "12345" }
);

// Reschedule the event
var editEventParams = new EditEventParams(eventResponse.EventId)
{
    Date = "2024-12-16",
    StartTime = "10:00"
};
await lacrm.EditEvent(editEventParams);

// Get upcoming events
var events = await lacrm.GetEvents(new GetEventsParams
{
    MinDate = DateTime.Today.ToString("yyyy-MM-dd"),
    MaxDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd")
});
```

### Work with Notes

```csharp
// Add a note
var noteResponse = await lacrm.CreateNote(
    contactId: "12345",
    note: "Client interested in premium features"
);

// Update the note
await lacrm.EditNote(new EditNoteParams(noteResponse.NoteId)
{
    Note = "Client confirmed purchase of premium features"
});

// Get all notes for contact
var notes = await lacrm.GetNotesAttachedToContact("12345");
foreach (var note in notes.Result)
{
    Console.WriteLine($"{note.DateCreated}: {note.NoteText}");
}
```

### Manage Groups

```csharp
// Create a group
await lacrm.CreateGroup("VIP Customers");

// Add contact to group
await lacrm.AddContactGroup("12345", "VIP Customers");

// Get all groups
var groups = await lacrm.GetGroups();
foreach (var group in groups.Groups)
{
    Console.WriteLine($"- {group.Name}");
}

// Remove contact from group
await lacrm.RemoveContactFromGroup("12345", "Old Group");
```

### Pipeline Management

```csharp
// Add contact to pipeline
var pipelineItem = await lacrm.CreatePipeline(
    contactId: "12345",
    pipelineId: "pipeline-id",
    statusId: "status-id",
    note: "New qualified lead",
    priority: 1
);

// Update pipeline status
await lacrm.UpdatePipelineItem(
    pipelineItemId: pipelineItem.PipelineItemId,
    statusId: "next-status-id",
    note: "Moved to proposal stage"
);

// Get pipeline report
var report = await lacrm.GetPipelineReport(
    pipelineId: "pipeline-id",
    sortBy: "Priority",
    numRows: 50
);
```

## Version 2.0 Changes

Version 2.0 adds 21 new functions while maintaining full backward compatibility:

- **Tasks**: 5 new functions (Edit, Delete, Get, GetMultiple, GetByContact)
- **Events**: 5 new functions (Edit, Delete, Get, GetMultiple, GetByContact)
- **Notes**: 5 new functions (Edit, Delete, Get, GetMultiple, GetByContact)
- **Pipelines**: 2 new functions (Get, Delete)
- **Groups**: 4 new functions (GetAll, Create, Delete, RemoveContact)

See the [Migration Guide](docs/articles/migration-v1-to-v2.md) for details.

## Error Handling

```csharp
using Enduro.Lacrm.Exceptions;

try
{
    var response = await lacrm.GetContact("contact-id");
}
catch (ValidationException ex)
{
    // Parameter validation failed
    foreach (var error in ex.ValidationResponse.Errors)
    {
        Console.WriteLine($"{error.ParameterName}: {error.Message}");
    }
}
catch (ApiException ex)
{
    // API returned an error
    Console.WriteLine($"API error: {ex.Message}");
}
catch (HttpException ex)
{
    // HTTP communication failed
    Console.WriteLine($"Communication error: {ex.Message}");
}
```

## Pagination

Many endpoints support pagination for large result sets:

```csharp
var page = 1;
var allTasks = new List<Task>();

while (true)
{
    var response = await lacrm.GetTasksAttachedToContact(
        contactId: "12345",
        maxNumberOfResults: 100,
        page: page
    );

    allTasks.AddRange(response.Result);

    if (response.Result.Count() < 100)
        break;

    page++;
}
```

## ASP.NET Core Integration

Register services in `Program.cs`:

```csharp
builder.Services.AddHttpClient();

builder.Services.AddSingleton(sp => new Options
{
    ApiToken = builder.Configuration["Lacrm:ApiToken"],
    UserCode = builder.Configuration["Lacrm:UserCode"],
    ApiUrl = "https://api.lessannoyingcrm.com"
});

builder.Services.AddScoped<LacrmClient>();
```

Inject into controllers:

```csharp
public class ContactsController : Controller
{
    private readonly LacrmClient _lacrm;

    public ContactsController(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task<IActionResult> Search(string query)
    {
        var response = await _lacrm.SearchContacts(query);
        return View(response.Result);
    }
}
```

## Documentation

Full documentation is available in the [docs](docs/) folder:

- **[Getting Started](docs/articles/getting-started.md)** - Installation, authentication, basic usage
- **[Contacts API](docs/articles/contacts.md)** - Creating, editing, searching contacts
- **[Tasks API](docs/articles/tasks.md)** - Complete task management guide
- **[Events API](docs/articles/events.md)** - Calendar event operations
- **[Notes API](docs/articles/notes.md)** - Note management and history
- **[Pipelines API](docs/articles/pipelines.md)** - Pipeline and opportunity management
- **[Groups API](docs/articles/groups.md)** - Contact categorization
- **[Error Handling](docs/articles/error-handling.md)** - Exception handling patterns
- **[Pagination](docs/articles/pagination.md)** - Working with large datasets
- **[Migration Guide](docs/articles/migration-v1-to-v2.md)** - Upgrading from v1.x

## Requirements

- .NET 6.0 or higher
- Less Annoying CRM account with API access

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Acknowledgments

Built for the [Less Annoying CRM](https://www.lessannoyingcrm.com/) platform.

## Support

- **Documentation**: [Complete API Documentation](docs/index.md)
- **Issues**: [GitHub Issues](https://github.com/yourusername/Enduro-Lacrm/issues)
- **LACRM API**: [Official API Documentation](https://lessannoyingcrm.com/help/topic/API)