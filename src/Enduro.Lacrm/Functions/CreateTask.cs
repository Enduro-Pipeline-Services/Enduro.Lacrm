using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class CreateTask : ILacrmFunction<CreateTaskParams>
    {
        public CreateTask(CreateTaskParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "CreateTask";
        public CreateTaskParams Parameters { get; }
    }
}