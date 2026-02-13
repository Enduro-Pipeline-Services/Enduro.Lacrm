using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class EditTask : ILacrmFunction<EditTaskParams>
    {
        public EditTask(EditTaskParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "EditTask";
        public EditTaskParams Parameters { get; }
    }
}
