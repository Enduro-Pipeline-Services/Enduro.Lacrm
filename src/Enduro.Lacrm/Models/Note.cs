using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Enduro.Lacrm.Models
{
    [PublicAPI]
    public class Note
    {
        public string? NoteId { get; set; }
        public string? ContactId { get; set; }
        public string? UserId { get; set; }
        public string? DateCreated { get; set; }
        public string? DateDisplayedInHistory { get; set; }
        
        [JsonPropertyName("Note")]
        public string? NoteText { get; set; }
        
        public ContactMetaData? ContactMetaData { get; set; }
        public UserMetaData? UserMetaData { get; set; }
        public NotePipelineInfo? PipelineInfo { get; set; }
        public bool? IsRichText { get; set; }
    }

    [PublicAPI]
    public class UserMetaData
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    [PublicAPI]
    public class NotePipelineInfo
    {
        public string? PipelineId { get; set; }
        public string? PipelineItemId { get; set; }
        public string? StatusId { get; set; }
        public string? PreviousStatusId { get; set; }
        public PipelineMetaData? PipelineMetaData { get; set; }
        public StatusMetaData? StatusMetaData { get; set; }
    }

    [PublicAPI]
    public class PipelineMetaData
    {
        public string? Name { get; set; }
    }

    [PublicAPI]
    public class StatusMetaData
    {
        public string? Name { get; set; }
    }
}
