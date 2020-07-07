using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class CreateEventResponse : LacrmResponse
    {
        public string? EventId { get; set; }
    }
}