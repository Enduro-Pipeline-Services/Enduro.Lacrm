using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetUserInfoResponse : LacrmResponse
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserId { get; set; }
        public string? AccountId { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
        public bool BlockExports { get; set; }
        public string? Timezone { get; set; }
        public string? DateUserCreated { get; set; }
        public string? DateAccountCreated { get; set; }
    }
}