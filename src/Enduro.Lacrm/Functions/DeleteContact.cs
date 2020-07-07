using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class DeleteContact : ILacrmFunction<DeleteContactParams>
    {
        public DeleteContact(string contactId)
        {
            Parameters = new DeleteContactParams(contactId);
        }
        
        public DeleteContact(DeleteContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "DeleteContact";
        public DeleteContactParams Parameters { get; }
    }
}