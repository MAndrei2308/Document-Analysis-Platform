using ContentCreationTool.Api.Domain.Enums;

namespace ContentCreationTool.Api.Application.DTOs
{
    public class BaseContentItemDto
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public LLMModelType ModelTypeUsed { get; set; }
    }
}
