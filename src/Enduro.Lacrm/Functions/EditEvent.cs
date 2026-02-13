using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class EditEvent : ILacrmFunction<EditEventParams>
    {
        public EditEvent(EditEventParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "EditEvent";
        public EditEventParams Parameters { get; }
    }
}
