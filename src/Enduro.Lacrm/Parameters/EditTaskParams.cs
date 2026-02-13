using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class EditTaskParams : Parameter
    {
        public EditTaskParams(string taskId)
        {
            TaskId = taskId;
            Validators.Add(ValidateTaskId);
        }

        public string TaskId { get; set; }
        public string? Name { get; set; }
        public string? DueDate { get; set; }
        public string? AssignedTo { get; set; }
        public string? CalendarId { get; set; }
        public string? Description { get; set; }
        public string? ContactId { get; set; }
        public bool? IsCompleted { get; set; }

        public ParameterValidationResponse ValidateTaskId()
        {
            return TaskId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(TaskId), "TaskId cannot be null."));
        }
    }
}
