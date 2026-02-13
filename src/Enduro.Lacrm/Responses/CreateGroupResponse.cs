using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class CreateGroupResponse : LacrmResponse
    {
        public string? GroupId { get; set; }
    }
}
