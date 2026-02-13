# Contacts API Guide

The Contacts API is the foundation of Less Annoying CRM, allowing you to create, edit, retrieve, search, and delete contacts. Contacts can be people or companies and can have multiple contact methods, custom fields, and associated data.

## Overview

The Enduro.Lacrm library provides five contact-related functions:

- `CreateContact` - Create a new contact
- `GetContact` - Retrieve a single contact by ID
- `EditContact` - Update an existing contact
- `DeleteContact` - Remove a contact
- `SearchContacts` - Find contacts by search terms

## Creating Contacts

### Basic Person Contact

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new CreateContactParams
{
    FirstName = "John",
    LastName = "Doe",
    Email = new[] 
    { 
        new EmailParams { Text = "john.doe@example.com" } 
    }
};

var response = await lacrm.CreateContact(parameters);
Console.WriteLine($"Created contact ID: {response.ContactId}");
```

### Company Contact

```csharp
var parameters = new CreateContactParams
{
    IsCompany = true,
    CompanyName = "Acme Corporation",
    Email = new[] 
    { 
        new EmailParams { Text = "info@acme.com" } 
    },
    Phone = new[]
    {
        new PhoneParams { Text = "555-1234", Type = "Work" }
    }
};

var response = await lacrm.CreateContact(parameters);
```

### Complete Contact with All Details

```csharp
var parameters = new CreateContactParams
{
    FirstName = "Jane",
    LastName = "Smith",
    CompanyName = "Tech Solutions Inc",
    BackgroundInfo = "Met at Tech Conference 2024. Interested in enterprise solutions.",
    Birthday = "1985-06-15",
    
    // Multiple emails
    Email = new[]
    {
        new EmailParams { Text = "jane.smith@techsolutions.com", Type = "Work" },
        new EmailParams { Text = "jane@personal.com", Type = "Personal" }
    },
    
    // Multiple phones
    Phone = new[]
    {
        new PhoneParams { Text = "555-1234", Type = "Work" },
        new PhoneParams { Text = "555-5678", Type = "Mobile" }
    },
    
    // Address
    Address = new[]
    {
        new AddressParams 
        { 
            Street = "123 Main Street",
            City = "San Francisco",
            State = "CA",
            Zip = "94102",
            Country = "USA"
        }
    },
    
    // Websites
    Website = new[]
    {
        new WebsiteParams { Text = "https://techsolutions.com" }
    }
};

var response = await lacrm.CreateContact(parameters);
```

### Contact with Custom Fields

```csharp
var parameters = new CreateContactParams
{
    FirstName = "Robert",
    LastName = "Johnson",
    CustomFields = new Dictionary<string, string>
    {
        { "Industry", "Technology" },
        { "Revenue", "500000" },
        { "Employee Count", "50" },
        { "Referral Source", "LinkedIn" }
    }
};

var response = await lacrm.CreateContact(parameters);
```

## Retrieving Contacts

### Get Single Contact

```csharp
var response = await lacrm.GetContact("contact-id-here");
var contact = response.Contact;

Console.WriteLine($"Name: {contact.FullName}");
Console.WriteLine($"Company: {contact.CompanyName}");
Console.WriteLine($"Assigned to: {contact.AssignedTo}");

// Display email addresses
if (contact.Email != null)
{
    foreach (var email in contact.Email)
    {
        Console.WriteLine($"Email ({email.Type}): {email.Text}");
    }
}

// Display phone numbers
if (contact.Phone != null)
{
    foreach (var phone in contact.Phone)
    {
        Console.WriteLine($"Phone ({phone.Type}): {phone.Text}");
    }
}
```

### Search for Contacts

Basic search by any text:

```csharp
var response = await lacrm.SearchContacts("John Doe");

foreach (var contact in response.Result)
{
    Console.WriteLine($"{contact.FullName} - {contact.CompanyName}");
}
```

### Advanced Search

```csharp
var response = await lacrm.SearchContacts(
    searchTerms: "technology",
    numRows: 50,           // Limit to 50 results
    sort: "Name",          // Sort by name
    recordType: "Company"  // Only companies
);

Console.WriteLine($"Found {response.Result.Count()} companies");
```

### Search with Different Sort Options

```csharp
// Sort options: "Name", "DateCreated", "DateModified"

// Most recently created
var recent = await lacrm.SearchContacts("*", sort: "DateCreated");

// Most recently modified
var modified = await lacrm.SearchContacts("*", sort: "DateModified");

// Alphabetical
var alpha = await lacrm.SearchContacts("*", sort: "Name");
```

### Filter by Record Type

```csharp
// Get only people
var people = await lacrm.SearchContacts("*", recordType: "Person");

// Get only companies
var companies = await lacrm.SearchContacts("*", recordType: "Company");

// Get all (default)
var all = await lacrm.SearchContacts("*");
```

## Editing Contacts

### Update Basic Information

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new EditContactParams("contact-id-here")
{
    FirstName = "John",
    LastName = "Doe Updated",
    CompanyName = "New Company Name"
};

var response = await lacrm.EditContact(parameters);
```

### Update Contact Methods

```csharp
var parameters = new EditContactParams("contact-id-here")
{
    Email = new[]
    {
        new EmailParams { Text = "newemail@example.com", Type = "Work" }
    },
    Phone = new[]
    {
        new PhoneParams { Text = "555-9999", Type = "Mobile" }
    }
};

await lacrm.EditContact(parameters);
```

### Update Background Info

```csharp
var parameters = new EditContactParams("contact-id-here")
{
    BackgroundInfo = "Updated notes: Client interested in premium features"
};

await lacrm.EditContact(parameters);
```

### Update Custom Fields

```csharp
var parameters = new EditContactParams("contact-id-here")
{
    CustomFields = new Dictionary<string, string>
    {
        { "Status", "Active" },
        { "Last Contact Date", DateTime.Today.ToString("yyyy-MM-dd") }
    }
};

await lacrm.EditContact(parameters);
```

### Reassign Contact

```csharp
var parameters = new EditContactParams("contact-id-here")
{
    AssignedTo = "USER123"  // User Code of new owner
};

await lacrm.EditContact(parameters);
```

## Deleting Contacts

```csharp
var response = await lacrm.DeleteContact("contact-id-here");

if (response.Success)
{
    Console.WriteLine("Contact deleted successfully");
}
```

## Complete Contact Management Example

```csharp
using Enduro.Lacrm;
using Enduro.Lacrm.Parameters;
using Enduro.Lacrm.Exceptions;

public class ContactManager
{
    private readonly LacrmClient _lacrm;

    public ContactManager(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task<string> CreateFullContact(
        string firstName,
        string lastName,
        string email,
        string company)
    {
        try
        {
            var parameters = new CreateContactParams
            {
                FirstName = firstName,
                LastName = lastName,
                CompanyName = company,
                Email = new[]
                {
                    new EmailParams { Text = email, Type = "Work" }
                },
                BackgroundInfo = $"Created on {DateTime.Today:yyyy-MM-dd}"
            };

            var response = await _lacrm.CreateContact(parameters);
            Console.WriteLine($"Created contact: {response.ContactId}");
            
            return response.ContactId;
        }
        catch (ValidationException ex)
        {
            Console.WriteLine("Validation errors:");
            foreach (var error in ex.ValidationResponse.Errors)
            {
                Console.WriteLine($"  {error.ParameterName}: {error.Message}");
            }
            throw;
        }
    }

    public async Task<List<Contact>> FindContactsByCompany(string companyName)
    {
        var response = await _lacrm.SearchContacts(companyName);
        return response.Result
            .Where(c => c.CompanyName?.Contains(companyName, 
                StringComparison.OrdinalIgnoreCase) == true)
            .ToList();
    }

    public async Task UpdateContactEmail(string contactId, string newEmail)
    {
        var contact = await _lacrm.GetContact(contactId);
        
        var parameters = new EditContactParams(contactId)
        {
            Email = new[]
            {
                new EmailParams { Text = newEmail, Type = "Work" }
            }
        };

        await _lacrm.EditContact(parameters);
        Console.WriteLine($"Email updated for {contact.Contact.FullName}");
    }
}
```

## Working with Contact Data

### Extract Email Addresses

```csharp
var response = await lacrm.GetContact("contact-id-here");
var emails = response.Contact.Email?
    .Select(e => e.Text)
    .Where(e => !string.IsNullOrEmpty(e))
    .ToList();

if (emails?.Any() == true)
{
    Console.WriteLine("Email addresses:");
    foreach (var email in emails)
    {
        Console.WriteLine($"  {email}");
    }
}
```

### Check for Duplicate Contacts

```csharp
public async Task<bool> IsDuplicate(string email)
{
    var response = await _lacrm.SearchContacts(email);
    return response.Result.Any(c => 
        c.Email?.Any(e => e.Text?.Equals(email, 
            StringComparison.OrdinalIgnoreCase) == true) == true);
}
```

### Build Contact Summary

```csharp
public async Task<string> GetContactSummary(string contactId)
{
    var response = await lacrm.GetContact(contactId);
    var contact = response.Contact;

    var summary = new StringBuilder();
    summary.AppendLine($"Contact: {contact.FullName}");
    summary.AppendLine($"Company: {contact.CompanyName ?? "N/A"}");
    summary.AppendLine($"Assigned To: {contact.AssignedTo ?? "Unassigned"}");
    
    if (contact.Email?.Any() == true)
    {
        summary.AppendLine($"Email: {contact.Email.First().Text}");
    }
    
    if (contact.Phone?.Any() == true)
    {
        summary.AppendLine($"Phone: {contact.Phone.First().Text}");
    }

    return summary.ToString();
}
```

## Contact Model Properties

Key properties of the `Contact` model:

| Property | Type | Description |
|----------|------|-------------|
| `ContactId` | `string?` | Unique contact identifier |
| `FullName` | `string?` | Full name (computed) |
| `FirstName` | `string?` | First name |
| `LastName` | `string?` | Last name |
| `CompanyName` | `string?` | Company/organization name |
| `IsCompany` | `bool?` | True if company, false if person |
| `BackgroundInfo` | `string?` | Notes about contact |
| `Birthday` | `string?` | Birth date (YYYY-MM-DD) |
| `AssignedTo` | `string?` | User Code of owner |
| `Email` | `IEnumerable<Email>?` | Email addresses |
| `Phone` | `IEnumerable<Phone>?` | Phone numbers |
| `Address` | `IEnumerable<Address>?` | Physical addresses |
| `Website` | `IEnumerable<Website>?` | Website URLs |
| `CustomFields` | `Dictionary<string, string>?` | Custom field values |

## Best Practices

1. **Use custom fields**: Leverage custom fields for industry-specific data
2. **Normalize email/phone**: Standardize formats before storing
3. **Check duplicates**: Search before creating new contacts
4. **Provide context**: Use BackgroundInfo for relationship context
5. **Assign contacts**: Always assign to a user for accountability
6. **Keep data current**: Regularly update contact information
7. **Use search wisely**: Limit results and use specific search terms
8. **Handle nulls**: Contact methods may be null or empty

## See Also

- [Getting Started](getting-started.md)
- [Tasks API](tasks.md)
- [Notes API](notes.md)
- [Groups API](groups.md)
- [Error Handling](error-handling.md)
