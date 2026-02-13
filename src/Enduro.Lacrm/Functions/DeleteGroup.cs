using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class DeleteGroup : ILacrmFunction<DeleteGroupParams>
    {
        public DeleteGroup(string groupIdOrName)
        {
            Parameters = new DeleteGroupParams(groupIdOrName);
        }
        
        public DeleteGroup(DeleteGroupParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "DeleteGroup";
        public DeleteGroupParams Parameters { get; }
    }
}
