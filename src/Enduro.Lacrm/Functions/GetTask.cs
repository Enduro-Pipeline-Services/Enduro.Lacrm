using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetTask : ILacrmFunction<GetTaskParams>
    {
        public GetTask(string taskId)
        {
            Parameters = new GetTaskParams(taskId);
        }

        public GetTask(GetTaskParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetTask";
        public GetTaskParams Parameters { get; }
    }
}
