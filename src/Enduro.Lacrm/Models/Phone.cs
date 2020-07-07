using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Phone : ContactMethodBase
    {
        public string? Clean { get; set; }

        public Phone()
        {
        }

        public Phone(string text, string type) : base(text, type)
        {
        }
    }
}