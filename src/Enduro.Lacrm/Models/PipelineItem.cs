using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class PipelineItem
    {
        public string? PipelineItemId { get; set; }
        public string? ContactId { get; set; }
        public string? PipelineId { get; set; }
        public string? StatusId { get; set; }
        public int? Priority { get; set; }
        public string? Note { get; set; }
        public Dictionary<string, object>? CustomFields { get; set; }
        public string? DateCreated { get; set; }
        public string? DateUpdated { get; set; }
        public PipelineItemMetaData? PipelineMetaData { get; set; }
        public PipelineItemStatusMetaData? StatusMetaData { get; set; }
        public ContactMetaData? ContactMetaData { get; set; }
    }

    [PublicAPI]
    public class PipelineItemMetaData
    {
        public string? Name { get; set; }
        public string? Image { get; set; }
    }

    [PublicAPI]
    public class PipelineItemStatusMetaData
    {
        public string? Name { get; set; }
        public string? Color { get; set; }
        public string? IsActive { get; set; }
    }
}
