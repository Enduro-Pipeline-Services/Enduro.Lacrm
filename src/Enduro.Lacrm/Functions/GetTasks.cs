using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetTasks : ILacrmFunction<GetTasksParams>
    {
        public GetTasks(GetTasksParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetTasks";
        public GetTasksParams Parameters { get; }
    }
}
