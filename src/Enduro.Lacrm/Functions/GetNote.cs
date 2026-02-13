using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetNote : ILacrmFunction<GetNoteParams>
    {
        public GetNote(string noteId)
        {
            Parameters = new GetNoteParams(noteId);
        }

        public GetNote(GetNoteParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetNote";
        public GetNoteParams Parameters { get; }
    }
}
