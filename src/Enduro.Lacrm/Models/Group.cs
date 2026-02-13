using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Group
    {
        public string? GroupId { get; set; }
        public string? Name { get; set; }
        public int? ContactCount { get; set; }
    }
}
