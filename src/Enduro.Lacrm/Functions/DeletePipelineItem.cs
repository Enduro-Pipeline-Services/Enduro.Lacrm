using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class DeletePipelineItem : ILacrmFunction<DeletePipelineItemParams>
    {
        public DeletePipelineItem(string pipelineItemId)
        {
            Parameters = new DeletePipelineItemParams(pipelineItemId);
        }

        public DeletePipelineItem(DeletePipelineItemParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "DeletePipelineItem";
        public DeletePipelineItemParams Parameters { get; }
    }
}
