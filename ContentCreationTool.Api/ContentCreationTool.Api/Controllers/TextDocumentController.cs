using ContentCreationTool.Api.Application.DTOs;
using ContentCreationTool.Api.Application.Repositories;
using ContentCreationTool.Api.Domain.Entities;
using ContentCreationTool.Api.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ContentCreationTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextDocumentController : ControllerBase
    {
        private readonly ITextDocumentRepository textDocumentRepository;
        private readonly IContentItemRepository contentItemRepository;
        private readonly ILogger<TextDocumentController> logger;

        public TextDocumentController(ITextDocumentRepository textDocumentRepository, IContentItemRepository contentItemRepository, ILogger<TextDocumentController> logger)
        {
            this.textDocumentRepository = textDocumentRepository;
            this.contentItemRepository = contentItemRepository;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTextDocuments()
        {
            logger.LogInformation("Fetching all text documents from the database.");
            var textDocuments = await textDocumentRepository.GetAllTextDocumentsAsync();
            if (textDocuments == null || !textDocuments.Any())
            {
                logger.LogWarning("No text documents found.");
                return NotFound(new { Message = "No text documents found." });
            }
            logger.LogInformation($"Found {textDocuments.Count()} text documents.");
            return Ok(textDocuments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTextDocumentById(Guid id)
        {
            if (id == Guid.Empty)
            {
                logger.LogWarning("Received empty GUID for text document ID.");
                return BadRequest(new { Message = "Invalid text document ID." });
            }
            logger.LogInformation($"Fetching text document with ID: {id}");
            var textDocument = await textDocumentRepository.GetTextDocumentByIdAsync(id);
            if (textDocument == null)
            {
                logger.LogWarning($"Text document with ID: {id} not found.");
                return NotFound(new { Message = $"Text document with ID: {id} not found." });
            }
            logger.LogInformation($"Found text document with ID: {id}");
            return Ok(textDocument);
        }

        [HttpPost]
        public async Task<IActionResult> AddTextDocument([FromBody] BaseTextDocumentDto textDocumentDto)
        {
            logger.LogInformation("Adding new text document.");

            if (textDocumentDto == null)
            {
                logger.LogWarning("Received null text document.");
                return BadRequest(new { Message = "Text document cannot be null." });
            }
            var existingContentItem = await contentItemRepository.GetContentItemByIdAsync(textDocumentDto.ContentItemId);
            if (existingContentItem == null)
            {
                logger.LogWarning($"Content item with ID: {textDocumentDto.ContentItemId} not found.");
                return NotFound(new { Message = $"Content item with ID: {textDocumentDto.ContentItemId} not found." });
            }

            var textDocument = new TextDocument
            {
                FileName = textDocumentDto.FileName,
                ExtractedText = textDocumentDto.ExtractedText,
                Summary = textDocumentDto.Summary,
                ContentItemId = textDocumentDto.ContentItemId
            };
            logger.LogInformation($"Adding new text document: {textDocument.FileName}");
            var newId = await textDocumentRepository.AddTextDocumentAsync(textDocument);
            textDocument.Id = newId;
            logger.LogInformation($"Text document added with ID: {newId}");
            return CreatedAtAction(nameof(GetTextDocumentById), new { id = textDocument.Id }, textDocument);
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadTextDocument([FromForm] IFormFile file, [FromForm] Guid contentItemId)
        {
            logger.LogInformation("Uploading new text document.");

            if (file == null || file.Length == 0)
            {
                logger.LogWarning("Received empty file for upload.");
                return BadRequest(new { Message = "File cannot be null or empty." });
            }

            if (contentItemId == Guid.Empty)
            {
                logger.LogWarning("Received empty GUID for content item ID.");
                return BadRequest(new { Message = "Invalid content item ID." });
            }

            try
            {
                var textDocument = await textDocumentRepository.UploadTextDocumentAsync(file, contentItemId);
                if (textDocument == null)
                {
                    logger.LogWarning("Failed to upload text document.");
                    return BadRequest(new { Message = "Failed to upload text document." });
                }
                logger.LogInformation($"Text document uploaded successfully with ID: {textDocument.Id}");
                return CreatedAtAction(nameof(GetTextDocumentById), new { id = textDocument.Id }, textDocument);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while uploading text document.");
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while uploading the text document." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTextDocument(Guid id, [FromBody] TextDocumentDto textDocumentDto)
        {
            if (id != textDocumentDto.Id)
            {
                logger.LogWarning("Text document ID in the URL does not match the ID in the body.");
                return BadRequest(new { Message = "ID mismatch." });
            }

            logger.LogInformation($"Updating text document with ID: {id}");

            if (textDocumentDto == null)
            {
                logger.LogWarning("Received null text document.");
                return BadRequest(new { Message = "Text document cannot be null." });
            }
            var existingTextDocument = await textDocumentRepository.GetTextDocumentByIdAsync(id);
            if (existingTextDocument == null)
            {
                logger.LogWarning($"Text document with ID: {id} not found.");
                return NotFound(new { Message = $"Text document with ID: {id} not found." });
            }

            var existingContentItem = await contentItemRepository.GetContentItemByIdAsync(textDocumentDto.ContentItemId);
            if (existingContentItem == null)
            {
                logger.LogWarning($"Content item with ID: {textDocumentDto.ContentItemId} not found.");
                return NotFound(new { Message = $"Content item with ID: {textDocumentDto.ContentItemId} not found." });
            }

            existingTextDocument.FileName = textDocumentDto.FileName;
            existingTextDocument.ExtractedText = textDocumentDto.ExtractedText;
            existingTextDocument.Summary = textDocumentDto.Summary;
            existingTextDocument.ContentItemId = textDocumentDto.ContentItemId;

            var result = await textDocumentRepository.UpdateTextDocumentAsync(existingTextDocument);
            if (!result)
            {
                logger.LogWarning($"Text document with ID: {id} not found for update.");
                return NotFound(new { Message = $"Text document with ID: {id} not found." });
            }
            logger.LogInformation($"Text document with ID: {id} updated successfully.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTextDocument(Guid id)
        {
            if (id == Guid.Empty)
            {
                logger.LogWarning("Received empty GUID for text document ID.");
                return BadRequest(new { Message = "Invalid text document ID." });
            }
            logger.LogInformation($"Deleting text document with ID: {id}");
            var result = await textDocumentRepository.DeleteTextDocumentAsync(id);
            if (!result)
            {
                logger.LogWarning($"Text document with ID: {id} not found for deletion.");
                return NotFound(new { Message = $"Text document with ID: {id} not found." });
            }
            logger.LogInformation($"Text document with ID: {id} deleted successfully.");
            return NoContent();
        }
    }
}
