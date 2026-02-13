using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class DeleteTask : ILacrmFunction<DeleteTaskParams>
    {
        public DeleteTask(string taskId)
        {
            Parameters = new DeleteTaskParams(taskId);
        }

        public DeleteTask(DeleteTaskParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "DeleteTask";
        public DeleteTaskParams Parameters { get; }
    }
}
