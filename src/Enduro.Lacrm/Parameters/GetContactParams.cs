using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetContactParams : ContactIdBase
    {
        public GetContactParams(string contactId) : base(contactId)
        {
        }
    }
}