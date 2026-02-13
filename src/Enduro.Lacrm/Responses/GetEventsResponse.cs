#nullable disable
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetEventsResponse : LacrmResponse
    {
        public GetEventsResponse()
        {
            Results = new List<Models.Event>();
        }

        public bool HasMoreResults { get; set; }
        public IEnumerable<Models.Event> Results { get; set; }
    }
}
