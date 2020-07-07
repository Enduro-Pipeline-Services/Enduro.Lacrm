using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class EditContactParams : CreateContactParams
    {
        public EditContactParams(string contactId)
        {
            ContactId = contactId;
        }

        public string ContactId { get; set; }
        public string? AssignedTo { get; set; }
        
        public override ParameterValidationResponse ValidateName()
        {
            var multiNameFieldsNull =
                Salutation == null &&
                FirstName == null &&
                MiddleName == null &&
                LastName == null;

            if (FullName == null && multiNameFieldsNull)
                return new ParameterValidationResponse(true);

            return base.ValidateName();
        }
    }
}