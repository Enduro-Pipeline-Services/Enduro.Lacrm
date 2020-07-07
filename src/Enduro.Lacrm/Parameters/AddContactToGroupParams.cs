using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class AddContactToGroupParams : ContactIdBase
    {
        private string? _groupName;

        public string GroupName
        {
            get => _groupName ?? "";
            set => _groupName = value.Replace(" ", "_");
        }

        public AddContactToGroupParams(string contactId) : base(contactId)
        {
            GroupName = "";
        }

        public AddContactToGroupParams(string contactId, string groupName) 
            : base(contactId)
        {
            GroupName = groupName;
        }
    }
}