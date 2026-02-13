using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetNotesAttachedToContactParams : ContactIdBase
    {
        public GetNotesAttachedToContactParams(string contactId) : base(contactId)
        {
        }

        public int? MaxNumberOfResults { get; set; } = 500;
        public int? Page { get; set; }
    }
}
