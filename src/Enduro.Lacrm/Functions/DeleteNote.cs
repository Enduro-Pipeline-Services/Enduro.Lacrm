using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class DeleteNote : ILacrmFunction<DeleteNoteParams>
    {
        public DeleteNote(string noteId)
        {
            Parameters = new DeleteNoteParams(noteId);
        }

        public DeleteNote(DeleteNoteParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "DeleteNote";
        public DeleteNoteParams Parameters { get; }
    }
}
