using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetEvent : ILacrmFunction<GetEventParams>
    {
        public GetEvent(string eventId)
        {
            Parameters = new GetEventParams(eventId);
        }

        public GetEvent(GetEventParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetEvent";
        public GetEventParams Parameters { get; }
    }
}
