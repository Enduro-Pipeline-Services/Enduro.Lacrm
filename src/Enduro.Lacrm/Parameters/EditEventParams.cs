using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class EditEventParams : Parameter
    {
        public EditEventParams(string eventId)
        {
            EventId = eventId;
            Validators.Add(ValidateEventId);
        }

        public string EventId { get; set; }
        public string? Name { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public bool? IsAllDay { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public string? CalendarId { get; set; }
        public IEnumerable<Attendee>? Attendees { get; set; }
        public string? RecurrenceRule { get; set; }
        public string? EndRecurrenceDate { get; set; }

        public ParameterValidationResponse ValidateEventId()
        {
            return EventId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(EventId), "EventId cannot be null."));
        }
    }
}
