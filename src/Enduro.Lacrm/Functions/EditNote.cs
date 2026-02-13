using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class EditNote : ILacrmFunction<EditNoteParams>
    {
        public EditNote(EditNoteParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "EditNote";
        public EditNoteParams Parameters { get; }
    }
}
