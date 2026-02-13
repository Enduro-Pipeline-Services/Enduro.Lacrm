using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class RemoveContactFromGroupParams : ContactIdBase
    {
        private string? _groupName;

        public string GroupName
        {
            get => _groupName ?? "";
            set => _groupName = value.Replace(" ", "_");
        }

        public RemoveContactFromGroupParams(string contactId) : base(contactId)
        {
            GroupName = "";
        }

        public RemoveContactFromGroupParams(string contactId, string groupName) 
            : base(contactId)
        {
            GroupName = groupName;
        }
    }
}
