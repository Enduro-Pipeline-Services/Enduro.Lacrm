using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class SearchContactsParams : Parameter
    {
        public SearchContactsParams(string searchTerms,
            int? numRows = null,
            string? sort = null,
            string? recordType = null)
        {
            SearchTerms = searchTerms;
            NumRows = numRows;
            Sort = sort;
            RecordType = recordType;

            Validators.Add(ValidateSearchTerms);
        }

        public string SearchTerms { get; set; }
        public int? NumRows { get; set; }
        public string? Sort { get; set; }
        public string? RecordType { get; set; }

        public ParameterValidationResponse ValidateSearchTerms()
        {
            return SearchTerms == null
                ? new ParameterValidationResponse(false,
                    new[]
                    {
                        new ParameterError(
                            nameof(SearchTerms),
                            "SearchTerms cannot be null"),
                    })
                : new ParameterValidationResponse(true);
        }
    }
}