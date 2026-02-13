#nullable disable
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetTasksResponse : LacrmResponse
    {
        public GetTasksResponse()
        {
            Results = new List<Models.Task>();
        }

        public bool HasMoreResults { get; set; }
        public IEnumerable<Models.Task> Results { get; set; }
    }
}
