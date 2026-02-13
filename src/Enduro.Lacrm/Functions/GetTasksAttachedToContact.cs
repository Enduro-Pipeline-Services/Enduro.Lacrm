using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetTasksAttachedToContact : ILacrmFunction<GetTasksAttachedToContactParams>
    {
        public GetTasksAttachedToContact(string contactId)
        {
            Parameters = new GetTasksAttachedToContactParams(contactId);
        }

        public GetTasksAttachedToContact(GetTasksAttachedToContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetTasksAttachedToContact";
        public GetTasksAttachedToContactParams Parameters { get; }
    }
}
