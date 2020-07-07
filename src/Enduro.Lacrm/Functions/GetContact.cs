using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetContact : ILacrmFunction<GetContactParams>
    {
        public GetContact(string contactId)
        {
            Parameters = new GetContactParams(contactId);
        }
        
        public GetContact(GetContactParams parameters)
        {
            Parameters = parameters;
        }
        
        public string Function => "GetContact";
        public GetContactParams Parameters { get; }
    }
}