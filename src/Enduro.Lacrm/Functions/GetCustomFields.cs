using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetCustomFields : ILacrmFunction<GetCustomFieldsParams>
    {
        public GetCustomFields()
        {
            Parameters = new GetCustomFieldsParams();
        }

        public string Function => "GetCustomFields";
        public GetCustomFieldsParams Parameters { get; }
    }
}