using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetCustomFieldsResponse : LacrmResponse
    {
        public GetCustomFieldsResponse()
        {
            Contact = new HashSet<CustomField>();
            Company = new HashSet<CustomField>();
            Pipeline = new HashSet<CustomField>();
        }
        
        public IEnumerable<CustomField> Contact { get; set; }
        public IEnumerable<CustomField> Company { get; set; }
        public IEnumerable<CustomField> Pipeline { get; set; }
    }
}