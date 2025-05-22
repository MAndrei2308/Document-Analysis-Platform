using System.Text.Json.Serialization;

namespace ContentCreationTool.Api.Domain.Entities
{
    public class TextDocument
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public string ExtractedText { get; set; }
        public string? Summary { get; set; }

        // Relationships
        public Guid ContentItemId { get; set; }
        [JsonIgnore]
        public ContentItem ContentItem { get; set; }
    }
}
