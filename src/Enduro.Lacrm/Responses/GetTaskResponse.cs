using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetTaskResponse : LacrmResponse
    {
        public Models.Task? Task { get; set; }
    }
}
