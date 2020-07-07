using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class AddContactToGroup : ILacrmFunction<AddContactToGroupParams>
    {
        public AddContactToGroup(string contactId, string groupName)
        {
            Parameters = new AddContactToGroupParams(contactId, groupName);
        }
        
        public AddContactToGroup(AddContactToGroupParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "AddContactToGroup";
        public AddContactToGroupParams Parameters { get; }
    }
}