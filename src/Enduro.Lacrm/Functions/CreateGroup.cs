using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class CreateGroup : ILacrmFunction<CreateGroupParams>
    {
        public CreateGroup(string groupName)
        {
            Parameters = new CreateGroupParams(groupName);
        }
        
        public CreateGroup(CreateGroupParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "CreateGroup";
        public CreateGroupParams Parameters { get; }
    }
}
