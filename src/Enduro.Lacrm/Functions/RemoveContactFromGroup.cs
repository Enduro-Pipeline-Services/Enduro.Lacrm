using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class RemoveContactFromGroup : ILacrmFunction<RemoveContactFromGroupParams>
    {
        public RemoveContactFromGroup(string contactId, string groupName)
        {
            Parameters = new RemoveContactFromGroupParams(contactId, groupName);
        }
        
        public RemoveContactFromGroup(RemoveContactFromGroupParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "RemoveContactFromGroup";
        public RemoveContactFromGroupParams Parameters { get; }
    }
}
