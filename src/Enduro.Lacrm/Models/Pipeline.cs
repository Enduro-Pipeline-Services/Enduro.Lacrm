using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Pipeline
    {
        public string? PipelineItemId { get; set; }
        public string? PipelineId { get; set; }
        public string? StatusId { get; set; }
        public string? ContactId { get; set; }
        
        public IEnumerable<PipelineInfo>? PipelineInfo { get; set; }
        public IEnumerable<PipelineStatusInfo>? PipelineStatusInfo { get; set; }
        public object[]? ContactInfo { get; set; }
        
        public int? Priority { get; set; }
        public DateTime? LastUpdate { get; set; }
        public DateTime? ArchivedDate { get; set; }
        
        public object[]? CustomFields { get; set; }
    }

    [PublicAPI]
    public class PipelineInfo
    {
        public string PipelineId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
    }

    [PublicAPI]
    public class PipelineStatusInfo
    {
        public string StatusId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string IsActive { get; set; } = null!;
        public string Color { get; set; } = null!;
    }

    [PublicAPI]
    public class PipelineCustomField
    {
        private object? _value;

        public string CustomFieldId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public JsonValueKind JsonValueKind => !(_value is JsonElement el) ? 
            JsonValueKind.Undefined : el.ValueKind;

        public object? Value
        {
            get
            {
                if (!(_value is JsonElement el))
                    return _value;
        
                switch (el.ValueKind)
                {
                    case JsonValueKind.Array:
                        return el.EnumerateArray()
                            .Select(p => p.GetString());
                    case JsonValueKind.String:
                        return el.GetString();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set => _value = value;
        }
    }
}