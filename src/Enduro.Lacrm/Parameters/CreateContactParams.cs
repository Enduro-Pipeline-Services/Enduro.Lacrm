using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class CreateContactParams : Parameter
    {
        public CreateContactParams()
        {
            Email = new HashSet<Email>();
            Phone = new HashSet<Phone>();
            Address = new HashSet<Address>();
            Website = new HashSet<Website>();
            CustomFields = new Dictionary<string, string>();

            Validators.Add(ValidateName);
            Validators.Add(ValidateCompany);
        }

        public CreateContactParams(
            string fullName,
            string companyId,
            IEnumerable<Email> email,
            IEnumerable<Phone> phone,
            IEnumerable<Address> address,
            IEnumerable<Website> website,
            string? birthday,
            Dictionary<string, string> customFields)
            : this()
        {
            FullName = fullName;
            CompanyId = companyId;
            Email = email;
            Phone = phone;
            Address = address;
            Website = website;
            Birthday = birthday;
            CustomFields = customFields;
        }

        public CreateContactParams(
            string? salutation,
            string? firstName,
            string? middleName,
            string? lastName,
            string? companyName,
            string? title,
            string? industry,
            string? numEmployees,
            IEnumerable<Email> email,
            IEnumerable<Phone> phone,
            IEnumerable<Address> address,
            IEnumerable<Website> website,
            string? birthday,
            Dictionary<string, string> customFields)
            : this()
        {
            Salutation = salutation;
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            CompanyName = companyName;
            Title = title;
            Industry = industry;
            NumEmployees = numEmployees;
            Email = email;
            Phone = phone;
            Address = address;
            Website = website;
            Birthday = birthday;
            CustomFields = customFields;
        }

        // According to LACRM's API, you can only use one name selection method.
        // You can enter an entire name in the 'FullName' field,
        // e.g. Mr. John C. Doe. You can also split that name into its requisite
        // fields by hand. However, you must ensure that you only use one
        // method. At minimum, a First and Last name must be provided.
        public string? FullName { get; set; }

        public string? Salutation { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }

        // For assigning a company to a contact, you can either provide the 
        // company name or the company ID. 
        // To use an existing company, provide either the name or the ID. To
        // create a new company, provide a unique company name. You can also
        // populate the 'Title', 'Industry', and 'NumEmployees' fields.
        public string? CompanyName { get; set; }

        // These fields are only used for creation.
        public string? Title { get; set; }
        public string? Industry { get; set; }
        public string? NumEmployees { get; set; }

        public string? CompanyId { get; set; }

        public IEnumerable<Email> Email { get; set; }
        public IEnumerable<Phone> Phone { get; set; }
        public IEnumerable<Address> Address { get; set; }
        public IEnumerable<Website> Website { get; set; }
        public string? Birthday { get; set; }
        public Dictionary<string, string> CustomFields { get; set; }

        public virtual ParameterValidationResponse ValidateName()
        {
            var fullNameFieldNull =
                FullName == null;

            var multiNameFieldsNull =
                Salutation == null &&
                FirstName == null &&
                MiddleName == null &&
                LastName == null;

            if (fullNameFieldNull ^ multiNameFieldsNull)
                return new ParameterValidationResponse(true);

            var errors = new HashSet<ParameterError>
            {
                new ParameterError(nameof(FullName),
                    "Only one naming method allowed. FullName must be null " +
                    "if other name fields are populated")
            };
            if (Salutation != null)
                errors.Add(new ParameterError(nameof(Salutation),
                    "Only one naming method allowed. 'Salutation' must be null " +
                    "if 'FullName' is populated"));

            if (FirstName != null)
                errors.Add(new ParameterError(nameof(FirstName),
                    "Only one naming method allowed. 'FirstName' must be null " +
                    "if 'FullName' is populated"));

            if (MiddleName != null)
                errors.Add(new ParameterError(nameof(MiddleName),
                    "Only one naming method allowed. 'MiddleName' must be null " +
                    "if 'FullName' is populated"));

            if (LastName != null)
                errors.Add(new ParameterError(nameof(LastName),
                    "Only one naming method allowed. 'LastName' must be null " +
                    "if 'FullName' is populated"));

            return new ParameterValidationResponse(false, errors);
        }

        public virtual ParameterValidationResponse ValidateCompany()
        {
            var companyNameNull = CompanyName == null;
            var companyIdNull = CompanyId == null;
            if (companyIdNull ^ companyNameNull)
                return new ParameterValidationResponse(true);

            var error = new HashSet<ParameterError>
            {
                new ParameterError(
                    nameof(CompanyName),
                    "Only one company selection method allowed. 'CompanyName' " +
                    "must be null if 'CompanyId' is populated"),
                new ParameterError(
                    nameof(CompanyId),
                    "Only one company selection method allowed. 'CompanyId' " +
                    "must be null if 'CompanyName' is populated")
            };

            return new ParameterValidationResponse(false, error);
        }
    }
}