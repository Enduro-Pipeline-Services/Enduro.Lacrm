using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class CreatePipelineResponse : LacrmResponse
    {
        public string PipelineItemId { get; set; } = null!;
        public string OpportunityId { get; set; } = null!;
    }
}