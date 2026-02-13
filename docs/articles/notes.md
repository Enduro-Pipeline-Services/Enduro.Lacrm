# Notes API Guide

The Notes API allows you to create, edit, retrieve, and delete notes attached to contacts in Less Annoying CRM. Notes are a simple way to record information, interactions, and important details about your contacts.

## Overview

The Enduro.Lacrm library provides six note-related functions:

- `CreateNote` - Add a new note to a contact
- `EditNote` - Update an existing note
- `DeleteNote` - Remove a note
- `GetNote` - Retrieve a single note by ID
- `GetNotes` - Retrieve multiple notes with filtering
- `GetNotesAttachedToContact` - Get all notes for a specific contact

## Creating Notes

### Basic Note Creation

```csharp
var response = await lacrm.CreateNote(
    contactId: "contact-id-here",
    note: "Client expressed interest in premium plan during call"
);

Console.WriteLine($"Created note ID: {response.NoteId}");
```

### Multi-line Notes

```csharp
var note = @"Call Summary:
- Discussed Q4 performance
- Client satisfied with results
- Interested in expanding contract
- Follow up in 2 weeks";

var response = await lacrm.CreateNote("contact-id-here", note);
```

### Note from Meeting

```csharp
var meetingNotes = $@"Meeting Date: {DateTime.Today:yyyy-MM-dd}
Attendees: John Doe, Jane Smith
Topics:
1. Contract renewal discussion
2. New feature requests
3. Timeline for implementation

Action Items:
- Send proposal by end of week
- Schedule follow-up demo";

await lacrm.CreateNote("contact-id-here", meetingNotes);
```

## Editing Notes

Use `EditNote` to update the content of an existing note:

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new EditNoteParams("note-id-here")
{
    Note = "Updated note content with additional information"
};

var response = await lacrm.EditNote(parameters);
Console.WriteLine($"Note updated: {response.Success}");
```

### Append to Existing Note

```csharp
// First get the existing note
var getResponse = await lacrm.GetNote("note-id-here");
var existingNote = getResponse.Note.NoteText;

// Append new information
var updatedNote = existingNote + "\n\nUpdate: Client confirmed interest in premium features";

var parameters = new EditNoteParams("note-id-here")
{
    Note = updatedNote
};

await lacrm.EditNote(parameters);
```

### Correct a Note

```csharp
var parameters = new EditNoteParams("note-id-here")
{
    Note = "Corrected: Meeting scheduled for 2024-12-15 (not 12-14)"
};

await lacrm.EditNote(parameters);
```

## Retrieving Notes

### Get a Single Note

```csharp
var response = await lacrm.GetNote("note-id-here");
var note = response.Note;

Console.WriteLine($"Note ID: {note.NoteId}");
Console.WriteLine($"Contact: {note.ContactId}");
Console.WriteLine($"Created: {note.DateCreated}");
Console.WriteLine($"Content:\n{note.NoteText}");
```

### Get Multiple Notes with Filters

Use `GetNotes` to retrieve and filter notes:

```csharp
using Enduro.Lacrm.Parameters;

var parameters = new GetNotesParams
{
    ContactId = "contact-id-here",
    MaxNumberOfResults = 50,
    Page = 1,
    MinDate = "2024-01-01",
    MaxDate = "2024-12-31"
};

var response = await lacrm.GetNotes(parameters);
foreach (var note in response.Result)
{
    Console.WriteLine($"{note.DateCreated}: {note.NoteText.Substring(0, 50)}...");
}
```

### Get Recent Notes

Get notes created in the last 30 days:

```csharp
var minDate = DateTime.Today.AddDays(-30).ToString("yyyy-MM-dd");

var parameters = new GetNotesParams
{
    MinDate = minDate,
    MaxNumberOfResults = 100
};

var response = await lacrm.GetNotes(parameters);
Console.WriteLine($"Notes in last 30 days: {response.Result.Count()}");
```

### Get Notes for Specific Date Range

```csharp
var parameters = new GetNotesParams
{
    MinDate = "2024-10-01",
    MaxDate = "2024-10-31",
    ContactId = "contact-id-here"
};

var response = await lacrm.GetNotes(parameters);
```

## Get Notes for a Contact

Retrieve all notes associated with a specific contact:

### Basic Usage

```csharp
var response = await lacrm.GetNotesAttachedToContact("contact-id-here");

foreach (var note in response.Result)
{
    Console.WriteLine($"[{note.DateCreated}] {note.NoteText}");
    Console.WriteLine("---");
}
```

### With Pagination

```csharp
var response = await lacrm.GetNotesAttachedToContact(
    contactId: "contact-id-here",
    maxNumberOfResults: 25,
    page: 1
);

Console.WriteLine($"Total notes: {response.TotalResults}");
Console.WriteLine($"Current page: {response.Page}");
Console.WriteLine($"Notes on this page: {response.Result.Count()}");
```

### Get All Notes for Contact

```csharp
var contactId = "contact-id-here";
var page = 1;
var allNotes = new List<Enduro.Lacrm.Models.Note>();

while (true)
{
    var response = await lacrm.GetNotesAttachedToContact(
        contactId,
        maxNumberOfResults: 100,
        page: page
    );

    allNotes.AddRange(response.Result);

    if (response.Result.Count() < 100)
        break;

    page++;
}

Console.WriteLine($"Total notes for contact: {allNotes.Count}");
```

### Display Contact History

```csharp
var response = await lacrm.GetNotesAttachedToContact("contact-id-here");

Console.WriteLine("Contact History:");
Console.WriteLine("================");

foreach (var note in response.Result.OrderByDescending(n => n.DateCreated))
{
    Console.WriteLine($"\n{note.DateCreated}");
    Console.WriteLine(new string('-', 40));
    Console.WriteLine(note.NoteText);
}
```

## Deleting Notes

Remove a note permanently:

```csharp
var response = await lacrm.DeleteNote("note-id-here");

if (response.Success)
{
    Console.WriteLine("Note deleted successfully");
}
```

### Delete with Confirmation

```csharp
try
{
    var noteId = "note-id-here";
    var note = await lacrm.GetNote(noteId);
    
    Console.WriteLine($"Delete this note? {note.Note.NoteText.Substring(0, 50)}...");
    Console.WriteLine("(y/n)");
    
    var confirm = Console.ReadLine();
    if (confirm?.ToLower() == "y")
    {
        await lacrm.DeleteNote(noteId);
        Console.WriteLine("Note deleted");
    }
}
catch (ApiException ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
```

## Complete Note Management Example

```csharp
using Enduro.Lacrm;
using Enduro.Lacrm.Parameters;
using Enduro.Lacrm.Exceptions;

public class NoteManager
{
    private readonly LacrmClient _lacrm;

    public NoteManager(LacrmClient lacrm)
    {
        _lacrm = lacrm;
    }

    public async Task LogContactInteraction(
        string contactId, 
        string interactionType, 
        string details)
    {
        var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        var note = $"[{interactionType}] {timestamp}\n{details}";

        try
        {
            var response = await _lacrm.CreateNote(contactId, note);
            Console.WriteLine($"Interaction logged: {response.NoteId}");
        }
        catch (ApiException ex)
        {
            Console.WriteLine($"Failed to log interaction: {ex.Message}");
        }
    }

    public async Task<string> GetContactTimeline(string contactId)
    {
        var response = await _lacrm.GetNotesAttachedToContact(contactId);
        
        var timeline = new StringBuilder();
        timeline.AppendLine($"Timeline for Contact {contactId}");
        timeline.AppendLine(new string('=', 50));

        foreach (var note in response.Result.OrderByDescending(n => n.DateCreated))
        {
            timeline.AppendLine($"\n{note.DateCreated}");
            timeline.AppendLine(new string('-', 50));
            timeline.AppendLine(note.NoteText);
        }

        return timeline.ToString();
    }

    public async Task UpdateNoteWithFollowUp(string noteId, string followUp)
    {
        var response = await _lacrm.GetNote(noteId);
        var updatedNote = response.Note.NoteText + 
            $"\n\nFollow-up ({DateTime.Today:yyyy-MM-dd}): {followUp}";

        var parameters = new EditNoteParams(noteId)
        {
            Note = updatedNote
        };

        await _lacrm.EditNote(parameters);
    }
}
```

## Advanced Note Patterns

### Call Log Template

```csharp
public class CallLog
{
    public static async Task<string> LogPhoneCall(
        LacrmClient lacrm,
        string contactId,
        string caller,
        string duration,
        string summary)
    {
        var note = $@"📞 Phone Call Log
Date/Time: {DateTime.Now:yyyy-MM-dd HH:mm}
Caller: {caller}
Duration: {duration}

Summary:
{summary}

Status: Call completed";

        var response = await lacrm.CreateNote(contactId, note);
        return response.NoteId;
    }
}
```

### Email Tracking

```csharp
public async Task LogEmailSent(string contactId, string subject, string preview)
{
    var note = $@"📧 Email Sent
Subject: {subject}
Date: {DateTime.Today:yyyy-MM-dd}

Preview:
{preview}";

    await _lacrm.CreateNote(contactId, note);
}
```

### Meeting Minutes

```csharp
public async Task CreateMeetingMinutes(
    string contactId,
    string attendees,
    List<string> topics,
    List<string> actionItems)
{
    var note = new StringBuilder();
    note.AppendLine($"📋 Meeting Minutes - {DateTime.Today:yyyy-MM-dd}");
    note.AppendLine($"Attendees: {attendees}");
    note.AppendLine();
    
    note.AppendLine("Topics Discussed:");
    foreach (var topic in topics)
    {
        note.AppendLine($"• {topic}");
    }
    note.AppendLine();
    
    note.AppendLine("Action Items:");
    foreach (var item in actionItems)
    {
        note.AppendLine($"☐ {item}");
    }

    await _lacrm.CreateNote(contactId, note.ToString());
}
```

## Note Model Properties

The `Note` model includes the following properties:

| Property | Type | Description |
|----------|------|-------------|
| `NoteId` | `string?` | Unique note identifier |
| `ContactId` | `string?` | Associated contact ID |
| `NoteText` | `string?` | Note content |
| `DateCreated` | `string?` | Creation timestamp |
| `CreatedBy` | `string?` | User who created the note |

## Best Practices

1. **Be concise yet detailed**: Include relevant information without being verbose
2. **Use timestamps**: Especially for time-sensitive information
3. **Structure longer notes**: Use bullet points or sections for readability
4. **Include context**: Note the reason or event that prompted the note
5. **Log interactions**: Track all meaningful contact interactions
6. **Use consistent formatting**: Establish patterns for different note types
7. **Paginate for large sets**: Use pagination when retrieving many notes
8. **Don't duplicate**: Avoid creating redundant notes with the same information

## Search and Filter Tips

### Find Notes by Keyword (Client-side)

```csharp
var response = await lacrm.GetNotesAttachedToContact("contact-id-here");

var keyword = "premium";
var matchingNotes = response.Result
    .Where(n => n.NoteText.Contains(keyword, StringComparison.OrdinalIgnoreCase))
    .ToList();

Console.WriteLine($"Found {matchingNotes.Count} notes mentioning '{keyword}'");
```

### Get Notes from Last Interaction

```csharp
var response = await lacrm.GetNotesAttachedToContact("contact-id-here");

var mostRecentNote = response.Result
    .OrderByDescending(n => n.DateCreated)
    .FirstOrDefault();

if (mostRecentNote != null)
{
    Console.WriteLine($"Last interaction: {mostRecentNote.DateCreated}");
    Console.WriteLine(mostRecentNote.NoteText);
}
```

## See Also

- [Getting Started](getting-started.md)
- [Contacts API](contacts.md)
- [Tasks API](tasks.md)
- [Events API](events.md)
- [Error Handling](error-handling.md)
- [Pagination](pagination.md)
