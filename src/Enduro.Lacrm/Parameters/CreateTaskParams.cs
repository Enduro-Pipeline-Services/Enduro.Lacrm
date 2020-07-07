using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class CreateTaskParams : ContactIdBase
    {
        public string DueDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? AssignedTo { get; set; }
        
        public CreateTaskParams(
            string contactId,
            string dueDate,
            string name,
            string description,
            string? assignedTo = null) : base(contactId)
        {
            DueDate = dueDate;
            Name = name;
            Description = description;
            AssignedTo = assignedTo;
        }
    }
}