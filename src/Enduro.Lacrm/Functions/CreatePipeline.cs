using System.Collections.Generic;
using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class CreatePipeline : ILacrmFunction<CreatePipelineParams>
    {
        public CreatePipeline(
            string contactId,
            string pipelineItemId,
            string statusId,
            string? note = null,
            int? priority = null,
            Dictionary<string, string>? customFields = null)
        {
            Parameters = new CreatePipelineParams(
                contactId,
                pipelineItemId,
                statusId,
                note,
                priority,
                customFields);
        }
        public CreatePipeline(CreatePipelineParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "CreatePipeline";
        public CreatePipelineParams Parameters { get; }
    }
}