using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class CreatePipelineParams : ModifyPipelineBase
    {
        public CreatePipelineParams(
            string contactId,
            string pipelineId,
            string statusId,
            string? note = null,
            int? priority = null,
            Dictionary<string, string>? customFields = null)
            : base(statusId, note, priority, customFields)
        {
            ContactId = contactId;
            PipelineId = pipelineId;

            Validators.Add(ValidateContactId);
            Validators.Add(ValidatePipelineId);
        }

        public string ContactId { get; }
        public string PipelineId { get; }

        protected virtual ParameterValidationResponse ValidatePipelineId()
        {
            if (PipelineId != null)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(PipelineId),
                    "PipelineItemId cannot be null."));
        }

        protected virtual ParameterValidationResponse ValidateContactId()
        {
            return ContactId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(ContactId),
                        "ContactId cannot be null."));
        }
    }
}