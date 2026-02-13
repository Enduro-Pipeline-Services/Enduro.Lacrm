using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Task
    {
        public string? TaskId { get; set; }
        public string? Name { get; set; }
        public string? DueDate { get; set; }
        public string? AssignedTo { get; set; }
        public string? Description { get; set; }
        public string? ContactId { get; set; }
        public bool? IsCompleted { get; set; }
        public string? DateCompleted { get; set; }
        public string? CalendarId { get; set; }
        public string? DateCreated { get; set; }
        public AssignedToMetaData? AssignedToMetaData { get; set; }
        public ContactMetaData? ContactMetaData { get; set; }
    }

    [PublicAPI]
    public class AssignedToMetaData
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    [PublicAPI]
    public class ContactMetaData
    {
        public string? Name { get; set; }
        public string? AssignedTo { get; set; }
    }
}
