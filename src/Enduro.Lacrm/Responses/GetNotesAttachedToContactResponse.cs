#nullable disable
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetNotesAttachedToContactResponse : LacrmResponse
    {
        public GetNotesAttachedToContactResponse()
        {
            Results = new List<Models.Note>();
        }

        public bool HasMoreResults { get; set; }
        public IEnumerable<Models.Note> Results { get; set; }
    }
}
