using ContentCreationTool.Api.Domain.Enums;

namespace ContentCreationTool.Api.Application.DTOs
{
    public class AskTextDocumentDto
    {
        public Guid TextDocumentId { get; set; }
        public string Prompt { get; set; } = string.Empty;
        public LLMModelType ModelType { get; set; } = LLMModelType.Mistral;
    }
}
