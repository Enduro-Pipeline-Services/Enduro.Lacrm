using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetEventsAttachedToContact : ILacrmFunction<GetEventsAttachedToContactParams>
    {
        public GetEventsAttachedToContact(GetEventsAttachedToContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetEventsAttachedToContact";
        public GetEventsAttachedToContactParams Parameters { get; }
    }
}
