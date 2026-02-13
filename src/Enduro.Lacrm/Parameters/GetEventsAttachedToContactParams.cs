using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetEventsAttachedToContactParams : Parameter
    {
        public GetEventsAttachedToContactParams(string contactId)
        {
            ContactId = contactId;
            Validators.Add(ValidateContactId);
        }

        public string ContactId { get; set; }
        public int? MaxNumberOfResults { get; set; } = 500;
        public int? Page { get; set; }

        public ParameterValidationResponse ValidateContactId()
        {
            return ContactId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(ContactId), "ContactId cannot be null."));
        }
    }
}
