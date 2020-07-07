using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public abstract class ContactMethodBase
    {
        protected ContactMethodBase()
        {
        }

        protected ContactMethodBase(string text, string type)
        {
            Text = text;
            Type = type;
        }

        public string? Text { get; set; }
        public string? Type { get; set; }
        public string? TypeId { get; set; }
    }
}