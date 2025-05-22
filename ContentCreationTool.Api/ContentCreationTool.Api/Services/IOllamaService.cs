using ContentCreationTool.Api.Domain.Enums;

namespace ContentCreationTool.Api.Services
{
    public interface IOllamaService
    {
        Task<string> AskAsync(string prompt, LLMModelType modelType);
    }
}
