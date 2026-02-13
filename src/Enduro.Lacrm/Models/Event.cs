using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Event
    {
        public string? EventId { get; set; }
        public string? Name { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public bool? IsAllDay { get; set; }
        public bool? IsRecurring { get; set; }
        public string? RecurrenceEventId { get; set; }
        public string? RecurrenceRule { get; set; }
        public string? EndRecurrenceDate { get; set; }
        public int? SeriesNumber { get; set; }
        public string? DateCreated { get; set; }
        public string? DateUpdated { get; set; }
        public IEnumerable<string>? ContactIds { get; set; }
        public IEnumerable<string>? UserIds { get; set; }
        public IEnumerable<Attendee>? Attendees { get; set; }
        public string? CalendarId { get; set; }
        public IEnumerable<ContactMetaData>? ContactMetaData { get; set; }
        public IEnumerable<UserMetaData>? UserMetaData { get; set; }
        public CalendarMetaData? CalendarMetaData { get; set; }
    }

    [PublicAPI]
    public class CalendarMetaData
    {
        public string? Name { get; set; }
    }
}
