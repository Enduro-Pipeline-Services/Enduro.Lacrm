using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class DeleteNoteParams : Parameter
    {
        public DeleteNoteParams(string noteId)
        {
            NoteId = noteId;
            Validators.Add(ValidateNoteId);
        }

        public string NoteId { get; set; }

        public ParameterValidationResponse ValidateNoteId()
        {
            return NoteId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(NoteId), "NoteId cannot be null."));
        }
    }
}
