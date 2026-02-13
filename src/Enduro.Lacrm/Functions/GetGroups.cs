using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetGroups : ILacrmFunction<GetGroupsParams>
    {
        public GetGroups()
        {
            Parameters = new GetGroupsParams();
        }
        
        public GetGroups(GetGroupsParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetGroups";
        public GetGroupsParams Parameters { get; }
    }
}
