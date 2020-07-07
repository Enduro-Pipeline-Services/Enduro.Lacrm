using System.Collections.Generic;
using Enduro.Lacrm.Models;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Responses
{
    [PublicAPI]
    public class GetPipelineItemsAttachedToContactResponse : LacrmResponse
    {
        public IEnumerable<Pipeline> Result { get; set; } = null!;
    }
}