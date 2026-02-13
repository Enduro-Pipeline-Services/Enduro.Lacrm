using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetPipelineItem : ILacrmFunction<GetPipelineItemParams>
    {
        public GetPipelineItem(string pipelineItemId)
        {
            Parameters = new GetPipelineItemParams(pipelineItemId);
        }

        public GetPipelineItem(GetPipelineItemParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetPipelineItem";
        public GetPipelineItemParams Parameters { get; }
    }
}
