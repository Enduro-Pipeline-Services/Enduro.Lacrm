using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetEventsParams : Parameter
    {
        public GetEventsParams()
        {
        }

        public SortDirection? SortDirection { get; set; }
        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public IEnumerable<string>? UserIds { get; set; }
        public IEnumerable<string>? CalendarIds { get; set; }
        public string? ContactId { get; set; }
        public int? MaxNumberOfResults { get; set; } = 500;
        public int? Page { get; set; }
    }
}
