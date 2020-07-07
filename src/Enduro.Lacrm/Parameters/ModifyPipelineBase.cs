using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class ModifyPipelineBase : Parameter
    {
        public ModifyPipelineBase(
            string statusId,
            string? note = null,
            int? priority = null,
            Dictionary<string, string>? customFields = null)
        {
            Note = note;
            StatusId = statusId;
            Priority = priority;
            CustomFields = customFields;

            Validators.Add(ValidateStatusId);
            Validators.Add(ValidatePriority);
        }

        public string? Note { get; }
        public string StatusId { get; }
        public int? Priority { get; }
        public Dictionary<string, string>? CustomFields { get; }

        protected virtual ParameterValidationResponse ValidateStatusId()
        {
            if (StatusId != null)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(StatusId),
                    "StatusId cannot be null."));
        }

        protected virtual ParameterValidationResponse ValidatePriority()
        {
            if (Priority == null || Priority >= 0 && Priority <= 3)
                return new ParameterValidationResponse(true);

            return new ParameterValidationResponse(false,
                new ParameterError(nameof(Priority),
                    "Priority, if set, must be set to 1, 2, or 3."));
        }
    }
}