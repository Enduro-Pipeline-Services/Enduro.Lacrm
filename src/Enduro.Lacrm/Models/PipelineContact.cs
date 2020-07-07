using System.Text.Json.Serialization;

namespace Enduro.Lacrm.Models
{
    public class PipelineContact : Contact
    {
        public string? EmployerName { get; set; }
        public string? CompanyEmployees { get; set; }
        public string? PipelineId { get; set; }
        public string? StatusId { get; set; }
        public string? Priority { get; set; }
        public string? LastNote { get; set; }
        public string? StartDate { get; set; }
        public string? LastUpdate { get; set; }
        public string? NumUpdates { get; set; }
        public string? StartStatusId { get; set; }
        public string? LastUpdatedBy { get; set; }
        public string? SortOrder { get; set; }
        public string? ShowInReports { get; set; }

        [JsonPropertyName("CustomOpportunityData_New")]
        public string? CustomOpportunityDataNew { get; set; }

        [JsonPropertyName("CustomContactData_New")]
        public string? CustomContactDataNew { get; set; }

        public string? PipelineItemId { get; set; }
        public string? StatusName { get; set; }
    }
}