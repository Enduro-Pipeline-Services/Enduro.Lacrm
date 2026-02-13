using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetTasksParams : Parameter
    {
        public GetTasksParams()
        {
        }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public IEnumerable<string>? UserIds { get; set; }
        public string? ContactId { get; set; }
        public CompletionStatus? CompletionStatus { get; set; }
        public SortDirection? SortDirection { get; set; }
        public int? MaxNumberOfResults { get; set; } = 500;
        public int? Page { get; set; }
    }

    [PublicAPI]
    public enum CompletionStatus
    {
        Both,
        Incomplete,
        Complete
    }

    [PublicAPI]
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
