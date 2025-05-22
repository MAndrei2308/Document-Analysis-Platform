using ContentCreationTool.Api.Application.DTOs;
using ContentCreationTool.Api.Domain.Enums;
using ContentCreationTool.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContentCreationTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LLMController : ControllerBase
    {
        private readonly IOllamaService ollamaService;
        private readonly ILogger<LLMController> logger;

        public LLMController(IOllamaService ollamaService, ILogger<LLMController> logger)
        {
            this.ollamaService = ollamaService;
            this.logger = logger;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] LLMRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Prompt))
            {
                logger.LogWarning("Received empty prompt.");
                return BadRequest(new { Message = "Prompt cannot be empty." });
            }
            logger.LogInformation($"Asking LLM with prompt: {request.Prompt}");
            try
            {
                var response = await ollamaService.AskAsync(request.Prompt, request.ModelType);
                return Ok(new { Response = response});
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while asking LLM.");
                return StatusCode(500, new { Message = "Internal server error." });
            }
        }
    }
}
