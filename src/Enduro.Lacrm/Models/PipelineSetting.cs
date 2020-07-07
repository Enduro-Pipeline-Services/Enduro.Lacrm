using System.Collections.Generic;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class PipelineSetting
    {
        public PipelineSetting()
        {
            Statuses = new HashSet<Status>();
            Fields = new HashSet<PipelineSettingCustomField>();
        }

        public string? PipelineId { get; set; }
        public string? Name { get; set; }
        public string? DateCreated { get; set; }
        public IEnumerable<Status> Statuses { get; set; }
        public IEnumerable<PipelineSettingCustomField> Fields { get; set; }
    }

    [PublicAPI]
    public class Status
    {
        public string? StatusId { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }

    [PublicAPI]
    public class PipelineSettingCustomField
    {
        public PipelineSettingCustomField()
        {
            Options = new Dictionary<string, string>();
        }

        public string? CustomFieldId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public bool IsRequired { get; set; }
        public Dictionary<string, string> Options { get; set; }
    }
}