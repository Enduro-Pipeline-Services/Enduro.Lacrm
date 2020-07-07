using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class UpdatePipelineItemParams : ModifyPipelineBase
    {
        public UpdatePipelineItemParams(
            string pipelineItemId,
            string statusId,
            string? note = null,
            int? priority = null,
            Dictionary<string, string>? customFields = null)
            : base(statusId, note, priority, customFields)
        {
            PipelineItemId = pipelineItemId;
            
            Validators.Add(ValidatePipelineItemId);
        }

        public string PipelineItemId { get; }
        
        protected virtual ParameterValidationResponse ValidatePipelineItemId()
        {
            if (PipelineItemId != null)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(PipelineItemId),
                    "PipelineItemId cannot be null."));
        }
    }
}