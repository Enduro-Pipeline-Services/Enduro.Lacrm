using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetEvents : ILacrmFunction<GetEventsParams>
    {
        public GetEvents(GetEventsParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetEvents";
        public GetEventsParams Parameters { get; }
    }
}
