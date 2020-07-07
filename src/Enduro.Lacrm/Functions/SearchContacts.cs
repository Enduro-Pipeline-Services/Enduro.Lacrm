using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class SearchContacts : ILacrmFunction<SearchContactsParams>
    {
        public SearchContacts(SearchContactsParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "SearchContacts";
        public SearchContactsParams Parameters { get; }
    }
}