# Events API Guide

The Events API enables you to create, edit, retrieve, and delete calendar events in Less Annoying CRM. Events can have multiple attendees (contacts and users) and include time-based scheduling.

## Overview

The Enduro.Lacrm library provides six event-related functions:

- `CreateEvent` - Create a new calendar event
- `EditEvent` - Update an existing event
- `DeleteEvent` - Remove an event
- `GetEvent` - Retrieve a single event by ID
- `GetEvents` - Retrieve multiple events with filtering
- `GetEventsAttachedToContact` - Get all events for a specific contact

## Creating Events

### Basic Event Creation

```csharp
var response = await lacrm.CreateEvent(
    date: "2024-12-15",
    startTime: "14:00",
    endTime: "15:00",
    name: "Client Meeting"
);

Console.WriteLine($"Created event ID: {response.EventId}");
```

### Event with Description

```csharp
var response = await lacrm.CreateEvent(
    date: "2024-12-15",
    startTime: "10:00",
    endTime: "11:00",
    name: "Product Demo",
    description: "Demonstrate new features to potential client"
);
```

### Event with Contact Attendees

Associate one or more contacts with an event:

```csharp
var contactIds = new[] { "contact-id-1", "contact-id-2" };

var response = await lacrm.CreateEvent(
    date: "2024-12-20",
    startTime: "09:00",
    endTime: "10:00",
    name: "Team Standup",
    description: "Daily team sync",
    contacts: contactIds
);
```

### Event with User Attendees

Include users from your LACRM account:

```csharp
var userCodes = new[] { "USER123", "USER456" };

var response = await lacrm.CreateEvent(
    date: "2024-12-18",
    startTime: "13:00",
    endTime: "14:00",
    name: "Strategy Planning",
    users: userCodes
);
```

### Event with Both Contacts and Users

```csharp
var response = await lacrm.CreateEvent(
    date: "2024-12-22",
    startTime: "15:00",
    endTime: "16:30",
    name: "Quarterly Business Review",
    description: "Review Q4 performance and plan for Q1",
    contacts: new[] { "contact-id-1" },
    users: new[] { "USER123", "USER456" }
);
```

### Date and Time Formats

- **Date**: `YYYY-MM-DD` format
- **Time**: 24-hour format `HH:MM`

```csharp
// Valid formats
date: "2024-12-31"
startTime: "09:00"    // 9:00 AM
endTime: "17:30"      // 5:30 PM

// Using DateTime
var eventDate = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd");
var startTime = new TimeSpan(14, 0, 0).ToString(@"hh\:mm");
var endTime = new TimeSpan(15, 30, 0).ToString(@"hh\:mm");
```

## Editing Events

Use `EditEvent` to update any field of an existing event. Only include fields you want to change.

### Update Event Time

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new EditEventParams("event-id-here")
{
    Date = "2024-12-16",
    StartTime = "15:00",
    EndTime = "16:00"
};

var response = await lacrm.EditEvent(parameters);
Console.WriteLine($"Event updated: {response.Success}");
```

### Update Event Name and Description

```csharp
var parameters = new EditEventParams("event-id-here")
{
    Name = "Updated Meeting Title",
    Description = "Revised meeting agenda and objectives"
};

await lacrm.EditEvent(parameters);
```

### Update Attendees

```csharp
var parameters = new EditEventParams("event-id-here")
{
    ContactIds = new[] { "contact-1", "contact-2", "contact-3" },
    UserIds = new[] { "USER123" }
};

await lacrm.EditEvent(parameters);
```

### Reschedule Event

```csharp
var newDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd");

var parameters = new EditEventParams("event-id-here")
{
    Date = newDate,
    StartTime = "10:00",
    EndTime = "11:30"
};

await lacrm.EditEvent(parameters);
```

### Update Multiple Fields

```csharp
var parameters = new EditEventParams("event-id-here")
{
    Name = "Rescheduled Client Meeting",
    Date = "2025-01-10",
    StartTime = "14:00",
    EndTime = "15:30",
    Description = "Moved from December to January",
    ContactIds = new[] { "contact-id-1", "contact-id-2" }
};

await lacrm.EditEvent(parameters);
```

## Retrieving Events

### Get a Single Event

Retrieve complete details for one event:

```csharp
var response = await lacrm.GetEvent("event-id-here");
var evt = response.Event;

Console.WriteLine($"Event: {evt.Name}");
Console.WriteLine($"Date: {evt.StartDate}");
Console.WriteLine($"Description: {evt.Description}");

// List attendees
if (evt.Attendees != null)
{
    Console.WriteLine("Attendees:");
    foreach (var attendee in evt.Attendees)
    {
        Console.WriteLine($"  - Attendee ID: {attendee.AttendeeId}");
    }
}
```

### Get Multiple Events with Filters

Use `GetEvents` to retrieve and filter events:

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new GetEventsParams
{
    MinDate = "2024-12-01",
    MaxDate = "2024-12-31",
    ContactId = "contact-id-here",
    MaxNumberOfResults = 50,
    Page = 1
};

var response = await lacrm.GetEvents(parameters);
foreach (var evt in response.Result)
{
    Console.WriteLine($"{evt.StartDate}: {evt.Name}");
}
```

### Get Events in Date Range

Get all events for the current month:

```csharp
var firstDay = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
var lastDay = firstDay.AddMonths(1).AddDays(-1);

var parameters = new GetEventsParams
{
    MinDate = firstDay.ToString("yyyy-MM-dd"),
    MaxDate = lastDay.ToString("yyyy-MM-dd"),
    MaxNumberOfResults = 100
};

var response = await lacrm.GetEvents(parameters);
Console.WriteLine($"Events this month: {response.Result.Count()}");
```

### Get Upcoming Events

Get events for the next 7 days:

```csharp
var parameters = new GetEventsParams
{
    MinDate = DateTime.Today.ToString("yyyy-MM-dd"),
    MaxDate = DateTime.Today.AddDays(7).ToString("yyyy-MM-dd")
};

var response = await lacrm.GetEvents(parameters);

foreach (var evt in response.Result.OrderBy(e => e.StartDate))
{
    Console.WriteLine($"{evt.StartDate}: {evt.Name}");
}
```

### Get Today's Events

```csharp
var today = DateTime.Today.ToString("yyyy-MM-dd");

var parameters = new GetEventsParams
{
    MinDate = today,
    MaxDate = today
};

var response = await lacrm.GetEvents(parameters);
Console.WriteLine($"Events today: {response.Result.Count()}");
```

## Get Events for a Contact

Retrieve all events associated with a specific contact:

### Basic Usage

```csharp
var response = await lacrm.GetEventsAttachedToContact("contact-id-here");

foreach (var evt in response.Result)
{
    Console.WriteLine($"{evt.StartDate}: {evt.Name}");
}
```

### With Pagination

```csharp
var response = await lacrm.GetEventsAttachedToContact(
    contactId: "contact-id-here",
    maxNumberOfResults: 25,
    page: 1
);

Console.WriteLine($"Total events: {response.TotalResults}");
Console.WriteLine($"Current page: {response.Page}");
```

### Get All Events for Contact

```csharp
var contactId = "contact-id-here";
var page = 1;
var allEvents = new List<Enduro.Lacrm.Models.Event>();

while (true)
{
    var response = await lacrm.GetEventsAttachedToContact(
        contactId,
        maxNumberOfResults: 100,
        page: page
    );

    allEvents.AddRange(response.Result);

    if (response.Result.Count() < 100)
        break;

    page++;
}

Console.WriteLine($"Total events for contact: {allEvents.Count}");
```

### Filter Contact Events by Date

Combine with LINQ to filter after retrieval:

```csharp
var response = await lacrm.GetEventsAttachedToContact("contact-id-here");

var upcomingEvents = response.Result
    .Where(e => DateTime.Parse(e.StartDate) >= DateTime.Today)
    .OrderBy(e => e.StartDate);

foreach (var evt in upcomingEvents)
{
    Console.WriteLine($"{evt.StartDate}: {evt.Name}");
}
```

## Deleting Events

Remove an event permanently:

```csharp
var response = await lacrm.DeleteEvent("event-id-here");

if (response.Success)
{
    Console.WriteLine("Event deleted successfully");
}
```

### Delete with Confirmation

```csharp
var eventId = "event-id-here";
var evt = await lacrm.GetEvent(eventId);

Console.WriteLine($"Delete event '{evt.Event.Name}' on {evt.Event.Date}? (y/n)");
var confirm = Console.ReadLine();

if (confirm?.ToLower() == "y")
{
    await lacrm.DeleteEvent(eventId);
    Console.WriteLine("Event deleted");
}
```

## Complete Event Management Example

```csharp
using Enduro.Lacrm;
using Enduro.Lacrm.Parameters;
using Enduro.Lacrm.Exceptions;

public class EventManager
{
    private readonly LacrmClient _lacrm;

    public EventManager(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task ScheduleMeetingWithClient(string contactId, DateTime meetingDate)
    {
        try
        {
            // Create event
            var createResponse = await _lacrm.CreateEvent(
                date: meetingDate.ToString("yyyy-MM-dd"),
                startTime: "14:00",
                endTime: "15:00",
                name: "Client Consultation",
                description: "Initial consultation meeting",
                contacts: new[] { contactId }
            );

            var eventId = createResponse.EventId;
            Console.WriteLine($"Meeting scheduled: {eventId}");

            // Get event details
            var getResponse = await _lacrm.GetEvent(eventId);
            Console.WriteLine($"Confirmed: {getResponse.Event.Name} on {getResponse.Event.StartDate}");

            return;
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Failed to schedule meeting: {ex.Message}");
        }
    }

    public async Task<List<Enduro.Lacrm.Models.Event>> GetUpcomingMeetings(int days)
    {
        var parameters = new GetEventsParams
        {
            MinDate = DateTime.Today.ToString("yyyy-MM-dd"),
            MaxDate = DateTime.Today.AddDays(days).ToString("yyyy-MM-dd"),
            MaxNumberOfResults = 100
        };

        var response = await _lacrm.GetEvents(parameters);
        return response.Result
            .OrderBy(e => e.StartDate)
            .ToList();
    }

    public async Task RescheduleMeeting(string eventId, DateTime newDate, string newStartTime)
    {
        var parameters = new EditEventParams(eventId)
        {
            Date = newDate.ToString("yyyy-MM-dd"),
            StartTime = newStartTime
        };

        await _lacrm.EditEvent(parameters);
        Console.WriteLine("Meeting rescheduled successfully");
    }
}
```

## Event Model Properties

The `Event` model includes the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `EventId` | `string?` | Unique event identifier |
| `Name` | `string?` | Event name/title |
| `Date` | `string?` | Event date (YYYY-MM-DD) |
| `StartTime` | `string?` | Start time (HH:MM) |
| `EndTime` | `string?` | End time (HH:MM) |
| `Description` | `string?` | Event description |
| `CalendarId` | `string?` | Calendar identifier |
| `DateCreated` | `string?` | Creation date |
| `Attendees` | `IEnumerable<Attendee>?` | List of attendees |

### Attendee Properties

| Property | Type | Description |
|----------|------|-------------|
| `ContactId` | `string?` | Contact ID (if attendee is a contact) |
| `UserId` | `string?` | User Code (if attendee is a user) |
| `Name` | `string?` | Attendee name |

## Best Practices

1. **Include descriptions**: Add context about the meeting purpose
2. **Set realistic durations**: Allow adequate time including buffer
3. **Add all relevant attendees**: Include both contacts and users
4. **Use 24-hour time format**: Avoid AM/PM confusion
5. **Handle time zones**: LACRM stores times in account timezone
6. **Validate dates**: Ensure dates are in the future when creating
7. **Provide context in names**: Use descriptive event names
8. **Paginate large sets**: Use pagination for contacts with many events

## Calendar Integration Tips

### Create Recurring Meeting Pattern

```csharp
// Create weekly meetings for next 4 weeks
for (int week = 0; week < 4; week++)
{
    var meetingDate = DateTime.Today.AddDays(week * 7);
    
    await lacrm.CreateEvent(
        date: meetingDate.ToString("yyyy-MM-dd"),
        startTime: "10:00",
        endTime: "11:00",
        name: "Weekly Team Sync",
        description: $"Week {week + 1} team meeting"
    );
}
```

### Find Scheduling Conflicts

```csharp
public async Task<bool> HasConflict(DateTime date, string startTime, string endTime)
{
    var dateStr = date.ToString("yyyy-MM-dd");
    var parameters = new GetEventsParams
    {
        MinDate = dateStr,
        MaxDate = dateStr
    };

    var response = await _lacrm.GetEvents(parameters);
    
    // Check for overlapping times
    return response.Result.Any(e => 
        (e.StartTime.CompareTo(startTime) <= 0 && e.EndTime.CompareTo(startTime) > 0) ||
        (e.StartTime.CompareTo(endTime) < 0 && e.EndTime.CompareTo(endTime) >= 0)
    );
}
```

## See Also

- [Getting Started](getting-started.md)
- [Tasks API](tasks.md)
- [Contacts API](contacts.md)
- [Error Handling](error-handling.md)
- [Pagination](pagination.md)
