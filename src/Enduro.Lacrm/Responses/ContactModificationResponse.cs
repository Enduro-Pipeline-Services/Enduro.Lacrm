using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public abstract class ContactModificationResponse : LacrmResponse
    {
        public string ContactId { get; set; } = null!;
        public string CompanyId { get; set; } = null!;
    }
}