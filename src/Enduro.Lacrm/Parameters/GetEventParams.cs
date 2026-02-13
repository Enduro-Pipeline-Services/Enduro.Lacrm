using JetBrains.Annotations;

namespace Enduro.Lacrm.Parameters
{
    [PublicAPI]
    public class GetEventParams : Parameter
    {
        public GetEventParams(string eventId)
        {
            EventId = eventId;
            Validators.Add(ValidateEventId);
        }

        public string EventId { get; set; }

        public ParameterValidationResponse ValidateEventId()
        {
            return EventId != null
                ? new ParameterValidationResponse(true)
                : new ParameterValidationResponse(false,
                    new ParameterError(nameof(EventId), "EventId cannot be null."));
        }
    }
}
