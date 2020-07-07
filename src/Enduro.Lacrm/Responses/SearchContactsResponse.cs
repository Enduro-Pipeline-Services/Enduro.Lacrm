#nullable disable
using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class SearchContactsResponse : LacrmResponse
    {
        public SearchContactsResponse()
        {
            Result = new HashSet<Contact>();
        }
        
        public IEnumerable<Contact> Result { get; set; }
    }
}