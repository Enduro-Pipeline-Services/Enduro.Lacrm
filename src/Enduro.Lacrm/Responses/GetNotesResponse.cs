#nullable disable
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetNotesResponse : LacrmResponse
    {
        public GetNotesResponse()
        {
            Results = new List<Models.Note>();
        }

        public bool HasMoreResults { get; set; }
        public IEnumerable<Models.Note> Results { get; set; }
    }
}
