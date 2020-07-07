using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class EditContact : ILacrmFunction<EditContactParams>
    {
        public EditContact(EditContactParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "EditContact";
        public EditContactParams Parameters { get; }
    }
}