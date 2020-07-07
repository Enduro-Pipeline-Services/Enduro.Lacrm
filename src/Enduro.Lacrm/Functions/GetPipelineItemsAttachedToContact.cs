using Enduro.Lacrm.Parameters;

namespace Enduro.Lacrm.Functions
{
    public class GetPipelineItemsAttachedToContact 
        : ILacrmFunction<GetPipelineItemsAttachedToContactParams>
    {
        public GetPipelineItemsAttachedToContact(string contactId)
        {
            Parameters = new GetPipelineItemsAttachedToContactParams(contactId);
        }
        
        public GetPipelineItemsAttachedToContact(
            GetPipelineItemsAttachedToContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetPipelineItemsAttachedToContact";
        public GetPipelineItemsAttachedToContactParams Parameters { get; }
    }
}