namespace Enduro.Lacrm.Parameters
{
    public abstract class ContactIdBase : Parameter
    {
        protected ContactIdBase(string contactId)
        {
            ContactId = contactId;
            
            Validators.Add(ValidateData);
        }

        public string ContactId { get; set; }
        public ParameterValidationResponse ValidateData()
        {
            return ContactId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(ContactId), "ContactId cannot be null."));
        }
    }
}