using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class CreateContact : ILacrmFunction<CreateContactParams>
    {
        public CreateContact(CreateContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "CreateContact";
        public CreateContactParams Parameters { get; }
    }
}