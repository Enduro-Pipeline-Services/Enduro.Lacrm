using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class DeletePipelineItemParams : Parameter
    {
        public DeletePipelineItemParams(string pipelineItemId)
        {
            PipelineItemId = pipelineItemId;
            Validators.Add(ValidatePipelineItemId);
        }

        public string PipelineItemId { get; set; }

        public ParameterValidationResponse ValidatePipelineItemId()
        {
            return PipelineItemId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(PipelineItemId), "PipelineItemId cannot be null."));
        }
    }
}
