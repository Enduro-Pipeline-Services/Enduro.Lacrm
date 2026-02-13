# Enduro LACRM Documentation

Welcome to the comprehensive documentation for **Enduro.Lacrm**, a powerful .NET wrapper library for the [Less Annoying CRM API](https://lessannoyingcrm.com/help/topic/API).

## Overview

Enduro.Lacrm is a feature-rich, strongly-typed C# library that simplifies integration with Less Annoying CRM. With over 50 API functions, it provides complete coverage of LACRM's capabilities including contacts, tasks, events, notes, pipelines, and groups.

## Key Features

- **50+ API Functions** - Complete coverage of LACRM API endpoints
- **Strongly Typed** - Full IntelliSense support with typed models
- **Async/Await** - Modern async programming patterns throughout
- **Comprehensive Error Handling** - Detailed exceptions for validation and API errors
- **Pagination Support** - Built-in support for paginated results
- **XML Documentation** - Full documentation for all public APIs
- **Pipeline Management** - Complete pipeline CRUD operations
- **Group Management** - Full group and contact-group association support

## Quick Start

### Installation

```bash
dotnet add package Enduro.Lacrm
```

### Basic Usage

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

// Search for contacts
var contacts = await lacrm.SearchContacts("John Doe");
Console.WriteLine($"Found {contacts.Result.Count()} contacts");

// Create a task
await lacrm.CreateTask(
    contactId: "12345",
    dueDate: "2024-12-31",
    name: "Follow up call",
    description: "Quarterly check-in"
);
```

## Documentation Sections

### Getting Started
- [Installation & Setup](articles/getting-started.md)
- [Authentication](articles/getting-started.md#authentication)
- [Basic Usage Examples](articles/getting-started.md#basic-usage)

### API Guides
- [Contacts API](articles/contacts.md) - Creating, editing, searching contacts
- [Tasks API](articles/tasks.md) - Managing tasks and to-dos
- [Events API](articles/events.md) - Calendar events and scheduling
- [Notes API](articles/notes.md) - Adding notes to contacts
- [Pipelines API](articles/pipelines.md) - Pipeline management and reporting
- [Groups API](articles/groups.md) - Managing contact groups

### Advanced Topics
- [Error Handling](articles/error-handling.md) - Exception handling and validation
- [Pagination](articles/pagination.md) - Working with paginated results
- [Migration Guide](articles/migration-v1-to-v2.md) - Upgrading from v1.x to v2.0

### API Reference
- [Complete API Reference](api/index.md) - Full method and class documentation

## Version 2.0 Highlights

Version 2.0 introduces comprehensive API coverage with 30+ new functions:

- **Tasks**: Edit, Delete, Get single/multiple, Get tasks by contact
- **Events**: Edit, Delete, Get single/multiple, Get events by contact  
- **Notes**: Edit, Delete, Get single/multiple, Get notes by contact
- **Pipelines**: Get single pipeline item, Delete pipeline items
- **Groups**: Get all groups, Create group, Delete group, Remove contact from group

See the [Migration Guide](articles/migration-v1-to-v2.md) for complete details.

## Support

- **GitHub**: [Repository Link](https://github.com/yourusername/Enduro-Lacrm)
- **Issues**: [Report Issues](https://github.com/yourusername/Enduro-Lacrm/issues)
- **LACRM API Docs**: [Official API Documentation](https://lessannoyingcrm.com/help/topic/API)

## License

This project is licensed under the MIT License - see the LICENSE file for details.
