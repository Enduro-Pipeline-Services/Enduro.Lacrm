using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetNotesParams : Parameter
    {
        public GetNotesParams()
        {
        }

        public SortDirection? SortDirection { get; set; } = Parameters.SortDirection.Descending;
        public string? DateFilterStart { get; set; }
        public string? DateFilterEnd { get; set; }
        public IEnumerable<string>? UserIds { get; set; }
        public string? ContactId { get; set; }
        public int? MaxNumberOfResults { get; set; } = 500;
        public int? Page { get; set; }
    }
}
