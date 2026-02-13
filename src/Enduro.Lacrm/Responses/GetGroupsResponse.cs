using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetGroupsResponse : LacrmResponse
    {
        public List<Models.Group>? Groups { get; set; }
    }
}
