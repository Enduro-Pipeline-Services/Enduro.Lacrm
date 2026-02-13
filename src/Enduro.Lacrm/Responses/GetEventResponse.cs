using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetEventResponse : LacrmResponse
    {
        public Models.Event? Event { get; set; }
    }
}
