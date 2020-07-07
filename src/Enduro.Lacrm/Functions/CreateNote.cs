using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class CreateNote : ILacrmFunction<CreateNoteParams>
    {
        public CreateNote(CreateNoteParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "CreateNote";
        public CreateNoteParams Parameters { get; }
    }
}