using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetContactResponse : LacrmResponse
    {
        
        public Contact? Contact { get; set; }
    }
}