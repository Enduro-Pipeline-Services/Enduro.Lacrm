using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Contact
    {
        public Contact()
        {
            Email = new HashSet<Email>();
            Phone = new HashSet<Phone>();
            Website = new HashSet<Website>();
            Address = new HashSet<Address>();
            CustomFields = new Dictionary<string, string>();
            ContactCustomFields = new Dictionary<string, string>();
        }

        public Contact(string firstName,
            string lastName,
            string companyName,
            IEnumerable<Email> email,
            IEnumerable<Phone> phone,
            IEnumerable<Website> website,
            IEnumerable<Address> address,
            string birthday,
            Dictionary<string, string> customFields)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            Website = website;
            Address = address;
            Birthday = birthday;
            CustomFields = customFields;
        }

        public string? ContactId { get; set; }
        public string? UserId { get; set; }
        public string? CompanyId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Salutation { get; set; }
        public string? Suffix { get; set; }
        public IEnumerable<Email>? Email { get; set; }
        public IEnumerable<Phone>? Phone { get; set; }
        public IEnumerable<Website>? Website { get; set; }
        public IEnumerable<Address>? Address { get; set; }
        public string? Title { get; set; }
        public string? BackgroundInfo { get; set; }
        public string? Birthday { get; set; }
        public string? IsCompany { get; set; }
        public string? CompanyName { get; set; }
        public string? CreationDate { get; set; }
        public string? EditedDate { get; set; }
        public object? OriginalGoogleId { get; set; }
        public string? Industry { get; set; }
        public string? NumEmployees { get; set; }


        public string? ContactData { get; set; }
        public string? EmployeeCount { get; set; }
        // TODO: Solve this mutability issue. May need to build DTOs.
        public object CustomFields { get; set; }
        public object? ContactCustomFields { get; set; }
    }
}