using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Attendee
    {
        public bool? IsUser { get; set; }
        public string? AttendeeId { get; set; }
        public string? AttendanceStatus { get; set; }
    }
}
