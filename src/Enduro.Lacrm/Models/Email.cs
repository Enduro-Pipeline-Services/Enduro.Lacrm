using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Email : ContactMethodBase
    {
        public Email()
        {
        }

        public Email(string text, string type) : base(text, type)
        {
        }
    }
}