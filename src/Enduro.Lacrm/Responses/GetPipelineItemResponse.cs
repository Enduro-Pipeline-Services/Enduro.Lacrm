using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetPipelineItemResponse : LacrmResponse
    {
        public Models.PipelineItem? PipelineItem { get; set; }
    }
}
