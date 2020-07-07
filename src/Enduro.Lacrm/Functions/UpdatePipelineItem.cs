using System.Collections.Generic;
using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class UpdatePipelineItem : ILacrmFunction<UpdatePipelineItemParams>
    {
        public UpdatePipelineItem(
            string pipelineItemId, 
            string statusId,
            string? note = null,
            int? priority = null, 
            Dictionary<string, string>? customFields = null)
        {
            Parameters = new UpdatePipelineItemParams(
                pipelineItemId, statusId, note, priority, customFields);
        }
        public UpdatePipelineItem(UpdatePipelineItemParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "UpdatePipelineItem";
        public UpdatePipelineItemParams Parameters { get; }
    }
}