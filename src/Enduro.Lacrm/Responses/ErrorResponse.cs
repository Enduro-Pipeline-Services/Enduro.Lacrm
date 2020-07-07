using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class ErrorResponse : LacrmResponse
    {
        public string? Error { get; set; }
    }
}