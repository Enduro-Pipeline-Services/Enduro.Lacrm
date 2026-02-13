# Getting Started with Enduro LACRM

This guide will help you get up and running with the Enduro.Lacrm library in your .NET application.

## Prerequisites

- .NET 6.0 or higher
- A Less Annoying CRM account
- API credentials (API Token and User Code)

## Installation

Install the Enduro.Lacrm package via NuGet:

### Using .NET CLI

```bash
dotnet add package Enduro.Lacrm
```

### Using Package Manager Console

```powershell
Install-Package Enduro.Lacrm
```

### Using PackageReference

Add the following to your `.csproj` file:

```xml
<PackageReference Include="Enduro.Lacrm" Version="2.0.0" />
```

## Authentication

To use the LACRM API, you need two pieces of information:

1. **API Token** - Your unique API authentication token
2. **User Code** - Your user identification code

### Obtaining Your Credentials

1. Log in to your Less Annoying CRM account
2. Navigate to **Settings** → **API** (or go directly to https://www.lessannoyingcrm.com/app/Settings/Api)
3. Click **Generate new API token**
4. Copy both the **API Token** and **User Code**

> **Security Note**: Never commit API credentials to source control. Use environment variables, user secrets, or a secure configuration management system.

### Storing Credentials Securely

#### Using User Secrets (Development)

```bash
dotnet user-secrets init
dotnet user-secrets set "Lacrm:ApiToken" "your-api-token-here"
dotnet user-secrets set "Lacrm:UserCode" "your-user-code-here"
```

#### Using Environment Variables

```bash
# Windows
set LACRM_API_TOKEN=your-api-token-here
set LACRM_USER_CODE=your-user-code-here

# Linux/macOS
export LACRM_API_TOKEN=your-api-token-here
export LACRM_USER_CODE=your-user-code-here
```

#### Using appsettings.json (Not Recommended for Production)

```json
{
  "Lacrm": {
    "ApiToken": "your-api-token-here",
    "UserCode": "your-user-code-here",
    "ApiUrl": "https://api.lessannoyingcrm.com"
  }
}
```

## Basic Usage

### Creating a Client Instance

```csharp
using Enduro.Lacrm;
using System.Net.Http;

var httpClient = new HttpClient();
var options = new Options
{
    ApiToken = "YOUR_API_TOKEN",
    UserCode = "YOUR_USER_CODE",
    ApiUrl = "https://api.lessannoyingcrm.com"
};

var lacrm = new LacrmClient(httpClient, options);
```

### Dependency Injection Setup

For ASP.NET Core applications, register the client in `Program.cs` or `Startup.cs`:

```csharp
using Enduro.Lacrm;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient
builder.Services.AddHttpClient();

// Register LACRM options
builder.Services.AddSingleton(sp => new Options
{
    ApiToken = builder.Configuration["Lacrm:ApiToken"],
    UserCode = builder.Configuration["Lacrm:UserCode"],
    ApiUrl = "https://api.lessannoyingcrm.com"
});

// Register LacrmClient
builder.Services.AddScoped<LacrmClient>();

var app = builder.Build();
```

Then inject `LacrmClient` into your services:

```csharp
public class ContactService
{
    private readonly LacrmClient _lacrm;

    public ContactService(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task<IEnumerable<Contact>> SearchContactsAsync(string searchTerm)
    {
        var response = await _lacrm.SearchContacts(searchTerm);
        return response.Result;
    }
}
```

## Common Examples

### Searching for Contacts

```csharp
// Simple search
var response = await lacrm.SearchContacts("John Doe");
foreach (var contact in response.Result)
{
    Console.WriteLine($"{contact.FullName} - {contact.Email?.FirstOrDefault()?.Text}");
}

// Advanced search with options
var response = await lacrm.SearchContacts(
    searchTerms: "acme corp",
    numRows: 50,
    sort: "Name",
    recordType: "Company"
);
```

### Creating a Contact

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new CreateContactParams
{
    FirstName = "John",
    LastName = "Doe",
    CompanyName = "Acme Corp",
    Email = new[] { new EmailParams { Text = "john@acme.com" } },
    Phone = new[] { new PhoneParams { Text = "555-1234" } }
};

var response = await lacrm.CreateContact(parameters);
Console.WriteLine($"Created contact with ID: {response.ContactId}");
```

### Getting a Contact

```csharp
var contactId = "1234567890";
var response = await lacrm.GetContact(contactId);
var contact = response.Contact;

Console.WriteLine($"Name: {contact.FullName}");
Console.WriteLine($"Company: {contact.CompanyName}");
Console.WriteLine($"Email: {contact.Email?.FirstOrDefault()?.Text}");
```

### Creating a Task

```csharp
var response = await lacrm.CreateTask(
    contactId: "1234567890",
    dueDate: "2024-12-31",
    name: "Follow up call",
    description: "Quarterly business review"
);

Console.WriteLine($"Created task with ID: {response.TaskId}");
```

### Creating an Event

```csharp
var response = await lacrm.CreateEvent(
    date: "2024-12-15",
    startTime: "14:00",
    endTime: "15:00",
    name: "Client Meeting",
    description: "Discuss Q4 results",
    contacts: new[] { "1234567890" }
);

Console.WriteLine($"Created event with ID: {response.EventId}");
```

### Adding a Note to a Contact

```csharp
var response = await lacrm.CreateNote(
    contactId: "1234567890",
    note: "Client expressed interest in premium plan"
);

Console.WriteLine($"Created note with ID: {response.NoteId}");
```

## Error Handling

Always wrap API calls in try-catch blocks to handle errors gracefully:

```csharp
using Enduro.Lacrm.Exceptions;

try
{
    var response = await lacrm.SearchContacts("test");
    // Process results
}
catch (ValidationException ex)
{
    // Handle parameter validation errors
    Console.WriteLine($"Validation failed: {ex.Message}");
    foreach (var error in ex.ValidationResponse.Errors)
    {
        Console.WriteLine($"  {error.ParameterName}: {error.Message}");
    }
}
catch (ApiException ex)
{
    // Handle API errors
    Console.WriteLine($"API error: {ex.Message}");
}
catch (HttpException ex)
{
    // Handle HTTP communication errors
    Console.WriteLine($"HTTP error: {ex.Message}");
}
```

See [Error Handling](error-handling.md) for comprehensive error handling patterns.

## Cancellation Support

All API methods support cancellation tokens:

```csharp
var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(30));

try
{
    var response = await lacrm.SearchContacts("test", cancellationToken: cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Request was cancelled");
}
```

## Next Steps

- [Contacts API Guide](contacts.md) - Complete guide to managing contacts
- [Tasks API Guide](tasks.md) - Working with tasks
- [Events API Guide](events.md) - Managing calendar events
- [Notes API Guide](notes.md) - Adding and managing notes
- [Pipelines API Guide](pipelines.md) - Pipeline management
- [Groups API Guide](groups.md) - Working with contact groups
- [Error Handling](error-handling.md) - Comprehensive error handling
- [Pagination](pagination.md) - Working with large result sets

## Additional Resources

- [LACRM Official API Documentation](https://lessannoyingcrm.com/help/topic/API)
- [API Reference](../api/index.md)
- [GitHub Repository](https://github.com/yourusername/Enduro-Lacrm)
