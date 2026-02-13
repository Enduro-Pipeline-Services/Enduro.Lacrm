using Enduro.Lacrm.Parameters;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Functions
{
    [PublicAPI]
    public class GetNotes : ILacrmFunction<GetNotesParams>
    {
        public GetNotes(GetNotesParams parameters)
        {
            Parameters = parameters;
        }

        public string Function => "GetNotes";
        public GetNotesParams Parameters { get; }
    }
}
