using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetTasksAttachedToContactParams : ContactIdBase
    {
        public GetTasksAttachedToContactParams(string contactId) : base(contactId)
        {
        }

        public int? MaxNumberOfResults { get; set; } = 500;
        public int? Page { get; set; }
    }
}
