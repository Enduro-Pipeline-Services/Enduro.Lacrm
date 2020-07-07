using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetPipelineReport : ILacrmFunction<GetPipelineReportParams>
    {
        public GetPipelineReport(
            string pipelineId,
            string sortBy,
            int? numRows = null,
            int? page = null,
            string? sortDirection = null,
            string? userFilter = null,
            string? statusFilter = null)
        {
            Parameters = new GetPipelineReportParams(
                pipelineId,
                sortBy,
                numRows,
                page,
                sortDirection,
                userFilter,
                statusFilter);
        }
        
        public GetPipelineReport(GetPipelineReportParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetPipelineReport";
        public GetPipelineReportParams Parameters { get; }
    }
}