using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetUserInfo : ILacrmFunction<GetUserInfoParams>
    {
        public GetUserInfo()
        {
            Parameters = new GetUserInfoParams();
        }
        public GetUserInfo(GetUserInfoParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetUserInfo";
        public GetUserInfoParams Parameters { get; }
    }
}