using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetPipelineItemsAttachedToContactParams : ContactIdBase
    {
        public GetPipelineItemsAttachedToContactParams(string contactId)
            : base(contactId)
        {
        }
    }
}