using System.Linq;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetPipelineReportParams : Parameter
    {
        public GetPipelineReportParams(
            string pipelineId,
            string sortBy,
            int? numRows = null,
            int? page = null,
            string? sortDirection = null,
            string? userFilter = null,
            string? statusFilter = null)
        {
            PipelineId = pipelineId;
            SortBy = sortBy;
            NumRows = numRows;
            Page = page;
            SortDirection = sortDirection;
            UserFilter = userFilter;
            StatusFilter = statusFilter;

            Validators.Add(ValidateSortBy);
            Validators.Add(ValidateSortDirection);
        }

        public string PipelineId { get; }
        public string SortBy { get; }
        public int? NumRows { get; }
        public int? Page { get; }
        public string? SortDirection { get; }
        public string? UserFilter { get; }
        public string? StatusFilter { get; }

        public virtual ParameterValidationResponse ValidateSortBy()
        {
            var allowedSorts = new[]
                {"Priority", "DateNote", "ContactName", "Status"};
            if (allowedSorts.Contains(SortBy))
                return new ParameterValidationResponse(true);
            
            return new ParameterValidationResponse(false,
                new ParameterError(nameof(SortBy), 
                    "SortBy can only be: " +
                    "'Priority', 'DateNote', 'ContactName', or 'Status'"));
        }

        public virtual ParameterValidationResponse ValidateSortDirection()
        {
            var allowed = new[] {null, "ASC", "DESC"};
            if (allowed.Contains(SortDirection))
                return new ParameterValidationResponse(true);
            
            return new ParameterValidationResponse(false,
                new ParameterError(nameof(SortDirection),
                    "SortDirection can only be: 'ASC', 'DESC', or null"));
        }
    }
}