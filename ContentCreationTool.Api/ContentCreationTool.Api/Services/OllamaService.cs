using ContentCreationTool.Api.Domain.Enums;
using System.Text;
using System.Text.Json;

namespace ContentCreationTool.Api.Services
{
    public class OllamaService : IOllamaService
    {
        private readonly HttpClient httpClient;
        private readonly ILogger logger;

        public OllamaService(HttpClient httpClient, ILogger<OllamaService> logger)
        {
            this.httpClient = httpClient;
            this.logger = logger;
        }

        public async Task<string> AskAsync(string prompt, LLMModelType modelType)
        {
            var ollamaRequest = new
            {
                prompt = prompt,
                model = modelType.ToModelString(),
                stream = false
            };

            var content = new StringContent(JsonSerializer.Serialize(ollamaRequest), Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync("http://localhost:11434/api/generate", content);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var ollamaResponse = JsonDocument.Parse(responseString);

            var answer = ollamaResponse.RootElement.GetProperty("response").GetString();
            logger.LogInformation($"Ollama response: {answer}");

            return answer;
        }
    }
}
