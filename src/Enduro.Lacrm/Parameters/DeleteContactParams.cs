using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class DeleteContactParams : ContactIdBase
    {
        public DeleteContactParams(string contactId) : base(contactId)
        {
        }
    }
}