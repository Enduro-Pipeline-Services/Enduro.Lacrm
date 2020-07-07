using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetPipelineReportResponse : LacrmResponse
    {
        public GetPipelineReportResponse()
        {
            Result = new HashSet<PipelineContact>();
        }
        public IEnumerable<PipelineContact> Result { get; set; }
    }
}