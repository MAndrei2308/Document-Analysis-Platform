namespace ContentCreationTool.Api.Application.DTOs
{
    public class BaseTextDocumentDto
    {
        public string FileName { get; set; }
        public string ExtractedText { get; set; }
        public string? Summary { get; set; }

        // Relationships
        public Guid ContentItemId { get; set; }
    }
}
