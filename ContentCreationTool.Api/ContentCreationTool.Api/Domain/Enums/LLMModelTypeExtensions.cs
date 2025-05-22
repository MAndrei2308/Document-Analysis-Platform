using System.Collections.Generic;

namespace ContentCreationTool.Api.Domain.Enums
{
    public static class LLMModelTypeExtensions
    {
        private static readonly Dictionary<LLMModelType, string> ModelTypeStrings = new()
        {
            { LLMModelType.LLama3_2, "llama3.2" },
            { LLMModelType.LLama3_1, "llama3.1" },
            { LLMModelType.Gemma3, "gemma3" },
            { LLMModelType.DeepSeek_R1, "deepseek-r1" },
            { LLMModelType.Phi4_Mini, "phi4-mini" },
            { LLMModelType.Mistral, "mistral" }
        };

        public static string ToModelString(this LLMModelType modelType)
        {
            return ModelTypeStrings[modelType];
        }
    }
}
