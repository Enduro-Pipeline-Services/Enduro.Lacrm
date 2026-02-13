using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class DeleteEvent : ILacrmFunction<DeleteEventParams>
    {
        public DeleteEvent(string eventId)
        {
            Parameters = new DeleteEventParams(eventId);
        }

        public DeleteEvent(DeleteEventParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "DeleteEvent";
        public DeleteEventParams Parameters { get; }
    }
}
