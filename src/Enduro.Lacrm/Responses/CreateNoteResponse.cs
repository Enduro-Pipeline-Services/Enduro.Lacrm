using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class CreateNoteResponse : LacrmResponse
    {
        public string? NoteId { get; set; }
    }
}