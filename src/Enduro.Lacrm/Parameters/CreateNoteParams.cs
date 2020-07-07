using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class CreateNoteParams : ContactIdBase
    {
        public CreateNoteParams(string contactId) :
            base(contactId)
        {
            Note = "";
        }

        public CreateNoteParams(string contactId, string note)
            : base(contactId)
        {
            Note = note;
        }

        public string Note { get; set; }
    }
}