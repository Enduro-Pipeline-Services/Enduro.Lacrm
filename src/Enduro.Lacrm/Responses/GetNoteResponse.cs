using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetNoteResponse : LacrmResponse
    {
        public Models.Note? Note { get; set; }
    }
}
