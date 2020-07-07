using System.Collections.Generic;
using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class CreateEvent : ILacrmFunction<CreateEventParams>
    {
        public CreateEvent(CreateEventParams parameters)
        {
            Parameters = parameters;
        }

        public CreateEvent(
            string date,
            string startTime,
            string endTime,
            string name,
            string? description = null,
            IEnumerable<string>? contacts = null,
            IEnumerable<string>? users = null)
        {
            Parameters = new CreateEventParams(
                date,
                startTime,
                endTime,
                name,
                description,
                contacts,
                users);
        }

        public string Function => "CreateEvent";
        public CreateEventParams Parameters { get; }
    }
}