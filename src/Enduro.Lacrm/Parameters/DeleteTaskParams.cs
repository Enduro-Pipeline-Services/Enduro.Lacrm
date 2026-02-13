using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class DeleteTaskParams : Parameter
    {
        public DeleteTaskParams(string taskId)
        {
            TaskId = taskId;
            Validators.Add(ValidateTaskId);
        }

        public string TaskId { get; set; }

        public ParameterValidationResponse ValidateTaskId()
        {
            return TaskId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(TaskId), "TaskId cannot be null."));
        }
    }
}
