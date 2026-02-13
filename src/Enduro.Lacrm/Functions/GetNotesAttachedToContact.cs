using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetNotesAttachedToContact : ILacrmFunction<GetNotesAttachedToContactParams>
    {
        public GetNotesAttachedToContact(string contactId)
        {
            Parameters = new GetNotesAttachedToContactParams(contactId);
        }

        public GetNotesAttachedToContact(GetNotesAttachedToContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetNotesAttachedToContact";
        public GetNotesAttachedToContactParams Parameters { get; }
    }
}
