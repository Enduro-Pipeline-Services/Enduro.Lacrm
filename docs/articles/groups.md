# Groups API Guide

The Groups API allows you to organize contacts into groups for better categorization and bulk operations. Groups help segment your contacts for targeted communication and reporting.

## Overview

The Enduro.Lacrm library provides five group-related functions:

- `GetGroups` - Retrieve all groups
- `CreateGroup` - Create a new group
- `DeleteGroup` - Remove a group
- `AddContactGroup` - Add a contact to a group
- `RemoveContactFromGroup` - Remove a contact from a group

## Understanding Groups

Groups are flexible categories for organizing contacts:

- **Tag-like**: Contacts can belong to multiple groups
- **Custom**: Create groups specific to your business needs
- **Searchable**: Use groups to filter and segment contacts
- **No hierarchy**: Groups are flat, not nested

Common group uses:
- Customer segments (Small Business, Enterprise, etc.)
- Lead sources (Website, Referral, Trade Show)
- Industries (Technology, Healthcare, Finance)
- Marketing campaigns (Newsletter 2024, Webinar Attendees)
- Status indicators (VIP, At Risk, Active)

## Getting Groups

### List All Groups

```csharp
var response = await lacrm.GetGroups();

Console.WriteLine("Available Groups:");
foreach (var group in response.Groups)
{
    Console.WriteLine($"- {group.Name} (ID: {group.GroupId})");
}
```

### Find Specific Group

```csharp
var response = await lacrm.GetGroups();
var vipGroup = response.Groups
    .FirstOrDefault(g => g.Name == "VIP Customers");

if (vipGroup != null)
{
    Console.WriteLine($"VIP Group ID: {vipGroup.GroupId}");
}
```

### Cache Group IDs

For better performance, cache group IDs:

```csharp
public class GroupCache
{
    private readonly LacrmClient _lacrm;
    private Dictionary<string, string> _groupIds;

    public GroupCache(LacrmClient lacrm)
    {
        _lacrm = lacrm;
        _groupIds = new Dictionary<string, string>();
    }

    public async Task Initialize()
    {
        var response = await _lacrm.GetGroups();
        _groupIds = response.Groups
            .ToDictionary(g => g.Name, g => g.GroupId);
    }

    public string GetGroupId(string groupName)
    {
        return _groupIds.TryGetValue(groupName, out var id) ? id : null;
    }
}
```

## Creating Groups

### Create Basic Group

```csharp
var response = await lacrm.CreateGroup("Enterprise Customers");
Console.WriteLine($"Created group ID: {response.GroupId}");
```

### Create Multiple Groups

```csharp
var groupNames = new[]
{
    "Q4 2024 Leads",
    "Newsletter Subscribers",
    "Webinar Attendees",
    "Product Beta Users"
};

foreach (var name in groupNames)
{
    try
    {
        var response = await lacrm.CreateGroup(name);
        Console.WriteLine($"Created: {name} ({response.GroupId})");
    }
    catch (ApiException ex)
    {
        Console.WriteLine($"Failed to create {name}: {ex.Message}");
    }
}
```

### Create Group with Error Handling

```csharp
public async Task<string> EnsureGroupExists(string groupName)
{
    // Check if group already exists
    var groups = await _lacrm.GetGroups();
    var existing = groups.Groups
        .FirstOrDefault(g => g.Name.Equals(groupName, 
            StringComparison.OrdinalIgnoreCase));

    if (existing != null)
    {
        Console.WriteLine($"Group already exists: {groupName}");
        return existing.GroupId;
    }

    // Create new group
    var response = await _lacrm.CreateGroup(groupName);
    Console.WriteLine($"Created new group: {groupName}");
    return response.GroupId;
}
```

## Adding Contacts to Groups

### Add Single Contact

```csharp
var response = await lacrm.AddContactGroup(
    contactId: "contact-id-here",
    groupName: "VIP Customers"
);

if (response.Success)
{
    Console.WriteLine("Contact added to group");
}
```

### Add Contact to Multiple Groups

```csharp
var contactId = "contact-id-here";
var groups = new[] { "Newsletter", "Active Customers", "West Region" };

foreach (var group in groups)
{
    try
    {
        await lacrm.AddContactGroup(contactId, group);
        Console.WriteLine($"Added to {group}");
    }
    catch (ApiException ex)
    {
        Console.WriteLine($"Failed to add to {group}: {ex.Message}");
    }
}
```

### Add Multiple Contacts to Group

```csharp
public async Task AddContactsToGroup(
    IEnumerable<string> contactIds, 
    string groupName)
{
    foreach (var contactId in contactIds)
    {
        try
        {
            await _lacrm.AddContactGroup(contactId, groupName);
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Failed to add {contactId}: {ex.Message}");
        }
    }
    
    Console.WriteLine($"Added {contactIds.Count()} contacts to {groupName}");
}
```

### Group Contacts by Criteria

```csharp
public async Task GroupContactsByRegion()
{
    // Search for all contacts
    var contacts = await _lacrm.SearchContacts("*", numRows: 500);

    foreach (var contact in contacts.Result)
    {
        if (contact.Address?.Any() == true)
        {
            var state = contact.Address.First().State;
            
            if (!string.IsNullOrEmpty(state))
            {
                var regionGroup = GetRegionFromState(state);
                await _lacrm.AddContactGroup(contact.ContactId, regionGroup);
            }
        }
    }
}

private string GetRegionFromState(string state)
{
    var westCoast = new[] { "CA", "OR", "WA" };
    var eastCoast = new[] { "NY", "MA", "PA", "NJ" };
    
    if (westCoast.Contains(state)) return "West Region";
    if (eastCoast.Contains(state)) return "East Region";
    
    return "Central Region";
}
```

## Removing Contacts from Groups

### Remove Single Contact

```csharp
var response = await lacrm.RemoveContactFromGroup(
    contactId: "contact-id-here",
    groupName: "Old Group Name"
);

if (response.Success)
{
    Console.WriteLine("Contact removed from group");
}
```

### Remove from Multiple Groups

```csharp
var contactId = "contact-id-here";
var groupsToRemove = new[] { "Old Campaign", "Inactive" };

foreach (var group in groupsToRemove)
{
    await lacrm.RemoveContactFromGroup(contactId, group);
}
```

### Move Contact Between Groups

```csharp
public async Task MoveContactToGroup(
    string contactId, 
    string oldGroup, 
    string newGroup)
{
    await _lacrm.RemoveContactFromGroup(contactId, oldGroup);
    await _lacrm.AddContactGroup(contactId, newGroup);
    
    Console.WriteLine($"Moved contact from {oldGroup} to {newGroup}");
}
```

## Deleting Groups

### Delete by Name

```csharp
var response = await lacrm.DeleteGroup("Old Campaign");

if (response.Success)
{
    Console.WriteLine("Group deleted");
}
```

### Delete by ID

```csharp
var response = await lacrm.DeleteGroup("group-id-here");
```

### Delete Multiple Groups

```csharp
var groupsToDelete = new[] { "2023 Campaign", "Old Leads", "Archived" };

foreach (var group in groupsToDelete)
{
    try
    {
        await lacrm.DeleteGroup(group);
        Console.WriteLine($"Deleted: {group}");
    }
    catch (ApiException ex)
    {
        Console.WriteLine($"Failed to delete {group}: {ex.Message}");
    }
}
```

### Delete with Confirmation

```csharp
public async Task DeleteGroupWithConfirmation(string groupName)
{
    Console.WriteLine($"Are you sure you want to delete group '{groupName}'?");
    Console.WriteLine("This will NOT delete the contacts, only remove the group.");
    Console.WriteLine("Type 'yes' to confirm:");
    
    var confirm = Console.ReadLine();
    
    if (confirm?.ToLower() == "yes")
    {
        await _lacrm.DeleteGroup(groupName);
        Console.WriteLine("Group deleted");
    }
    else
    {
        Console.WriteLine("Deletion cancelled");
    }
}
```

## Complete Group Management Example

```csharp
using Enduro.Lacrm;
using Enduro.Lacrm.Exceptions;

public class GroupManager
{
    private readonly LacrmClient _lacrm;

    public GroupManager(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task OrganizeNewLead(
        string contactId, 
        string source, 
        string industry)
    {
        // Ensure groups exist
        await EnsureGroupExists("Active Leads");
        await EnsureGroupExists($"Source: {source}");
        await EnsureGroupExists($"Industry: {industry}");

        // Add to groups
        await _lacrm.AddContactGroup(contactId, "Active Leads");
        await _lacrm.AddContactGroup(contactId, $"Source: {source}");
        await _lacrm.AddContactGroup(contactId, $"Industry: {industry}");

        Console.WriteLine($"Contact organized into {source} / {industry} groups");
    }

    public async Task ConvertLeadToCustomer(string contactId)
    {
        // Remove from lead groups
        await _lacrm.RemoveContactFromGroup(contactId, "Active Leads");
        
        // Add to customer groups
        await _lacrm.AddContactGroup(contactId, "Active Customers");
        await _lacrm.AddContactGroup(contactId, $"Converted {DateTime.Today.Year}");
        
        Console.WriteLine("Contact converted to customer");
    }

    public async Task<string> EnsureGroupExists(string groupName)
    {
        var groups = await _lacrm.GetGroups();
        var existing = groups.Groups
            .FirstOrDefault(g => g.Name == groupName);

        if (existing != null)
            return existing.GroupId;

        var response = await _lacrm.CreateGroup(groupName);
        return response.GroupId;
    }

    public async Task CleanupOldCampaignGroups(int yearsBefore)
    {
        var cutoffYear = DateTime.Today.Year - yearsBefore;
        var groups = await _lacrm.GetGroups();

        var oldCampaigns = groups.Groups
            .Where(g => g.Name.Contains("Campaign") && 
                        g.Name.Any(char.IsDigit))
            .Where(g => 
            {
                var match = System.Text.RegularExpressions.Regex.Match(
                    g.Name, @"\d{4}");
                if (match.Success && int.TryParse(match.Value, out int year))
                    return year < cutoffYear;
                return false;
            });

        foreach (var group in oldCampaigns)
        {
            await _lacrm.DeleteGroup(group.GroupId);
            Console.WriteLine($"Deleted old campaign group: {group.Name}");
        }
    }

    public async Task CreateQuarterlyGroups(int year)
    {
        var quarters = new[]
        {
            $"Q1 {year} Leads",
            $"Q2 {year} Leads",
            $"Q3 {year} Leads",
            $"Q4 {year} Leads"
        };

        foreach (var quarter in quarters)
        {
            await _lacrm.CreateGroup(quarter);
            Console.WriteLine($"Created: {quarter}");
        }
    }

    public async Task GenerateGroupReport()
    {
        var groups = await _lacrm.GetGroups();
        
        Console.WriteLine($"\nTotal Groups: {groups.Groups.Count()}");
        Console.WriteLine("\nGroup Categories:");
        
        var categories = groups.Groups
            .GroupBy(g => 
            {
                if (g.Name.StartsWith("Source:")) return "Lead Sources";
                if (g.Name.StartsWith("Industry:")) return "Industries";
                if (g.Name.Contains("Campaign")) return "Campaigns";
                if (g.Name.Contains("Customer")) return "Customer Status";
                return "Other";
            });

        foreach (var category in categories)
        {
            Console.WriteLine($"\n{category.Key}:");
            foreach (var group in category)
            {
                Console.WriteLine($"  - {group.Name}");
            }
        }
    }
}
```

## Group Model Properties

| Property | Type | Description |
|----------|------|-------------|
| `GroupId` | `string?` | Unique group identifier |
| `Name` | `string?` | Group name |

## Best Practices

1. **Use consistent naming**: Establish naming conventions (e.g., "Source: LinkedIn", "Industry: Tech")
2. **Create groups proactively**: Set up groups before you need them
3. **Avoid over-categorization**: Too many groups becomes difficult to manage
4. **Archive old groups**: Delete outdated campaign or time-based groups
5. **Document group purposes**: Keep a reference of what each group represents
6. **Automate group assignment**: Assign groups based on contact attributes
7. **Review regularly**: Audit groups quarterly to remove unused ones
8. **Use descriptive names**: Names should clearly indicate the group's purpose

## Common Group Patterns

### Lead Source Tracking

```csharp
var leadSources = new[]
{
    "Source: Website",
    "Source: LinkedIn",
    "Source: Referral",
    "Source: Trade Show",
    "Source: Cold Outreach"
};
```

### Customer Lifecycle

```csharp
var lifecycle = new[]
{
    "Stage: Lead",
    "Stage: Qualified",
    "Stage: Customer",
    "Stage: Churned",
    "Stage: Reactivated"
};
```

### Engagement Level

```csharp
var engagement = new[]
{
    "Engagement: High",
    "Engagement: Medium",
    "Engagement: Low",
    "Engagement: At Risk"
};
```

## See Also

- [Getting Started](getting-started.md)
- [Contacts API](contacts.md)
- [Tasks API](tasks.md)
- [Error Handling](error-handling.md)
