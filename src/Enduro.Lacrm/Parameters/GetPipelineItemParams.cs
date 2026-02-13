using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetPipelineItemParams : Parameter
    {
        public GetPipelineItemParams(string pipelineItemId)
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
