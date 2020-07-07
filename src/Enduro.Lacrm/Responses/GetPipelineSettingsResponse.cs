using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetPipelineSettingsResponse : LacrmResponse
    {
        public GetPipelineSettingsResponse()
        {
            Result = new HashSet<PipelineSetting>();
        }

        public GetPipelineSettingsResponse(IEnumerable<PipelineSetting> result)
        {
            Result = result;
        }

        public IEnumerable<PipelineSetting> Result { get; set; }
    }
}