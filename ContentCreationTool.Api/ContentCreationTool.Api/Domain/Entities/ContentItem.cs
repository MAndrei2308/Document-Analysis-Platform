using ContentCreationTool.Api.Domain.Enums;

namespace ContentCreationTool.Api.Domain.Entities
{
    public class ContentItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public LLMModelType ModelTypeUsed { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Relationships
        public TextDocument? TextDocument { get; set; }
        public ImageDocument? ImageDocument { get; set; }
    }
}
