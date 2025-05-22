using ContentCreationTool.Api.Domain.Enums;

namespace ContentCreationTool.Api.Application.DTOs
{
    public class LLMRequestDto
    {
        public string Prompt { get; set; } = string.Empty;
        public LLMModelType ModelType { get; set; } = LLMModelType.Mistral;
    }
}
