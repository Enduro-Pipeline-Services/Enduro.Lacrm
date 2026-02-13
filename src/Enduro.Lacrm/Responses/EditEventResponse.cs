using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class EditEventResponse : LacrmResponse
    {
        public string? EventId { get; set; }
    }
}
