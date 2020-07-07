using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Address
    {
        public Address() {}
        public Address(
            string street, 
            string city, 
            string state, 
            string zip, 
            string type, 
            string country)
        {
            Street = street;
            City = city;
            State = state;
            Zip = zip;
            Type = type;
            Country = country;
        }

        public string? Street { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Zip { get; set; }
        public string? Type { get; set; }
        public string? Country { get; set; }
        public string? TypeId { get; set; }
    }
}