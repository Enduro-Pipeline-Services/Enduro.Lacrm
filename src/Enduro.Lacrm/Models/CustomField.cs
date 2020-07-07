using System.Collections.Generic;

namespace Enduro.Lacrm.Models
{
    public class CustomField
    {
        public string? CustomFieldId { get; set; }
        public string? Name { get; set; }
        public string? SortOrder { get; set; }
        public string? Type { get; set; }
        public IDictionary<string, CustomFieldOption>? Options { get; set; }
    }

    public class CustomFieldOption
    {
        public string? Option { get; set; }
        public string? OptionId { get; set; }
        public string? Color { get; set; }
    }
}