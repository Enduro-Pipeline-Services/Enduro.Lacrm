using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class CreateTaskResponse : LacrmResponse
    {
        public string? TaskId { get; set; }
    }
}