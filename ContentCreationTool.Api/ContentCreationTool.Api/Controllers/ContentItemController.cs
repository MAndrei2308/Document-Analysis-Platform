using ContentCreationTool.Api.Application;
using ContentCreationTool.Api.Application.DTOs;
using ContentCreationTool.Api.Application.Repositories;
using ContentCreationTool.Api.Domain.Entities;
using ContentCreationTool.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using UglyToad.PdfPig;

namespace ContentCreationTool.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentItemController : ControllerBase
    {
        private readonly IContentItemRepository contentItemRepository;
        private readonly ITextDocumentRepository textDocumentRepository;
        private readonly ILogger logger;
        private readonly IOllamaService ollamaService;

        public ContentItemController(IContentItemRepository contentItemRepository, ITextDocumentRepository textDocumentRepository, ILogger<ContentItemController> logger, IOllamaService ollamaService)
        {
            this.contentItemRepository = contentItemRepository;
            this.textDocumentRepository = textDocumentRepository;
            this.logger = logger;
            this.ollamaService = ollamaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllContentItems()
        {
            logger.LogInformation("Fetching all content items from the database.");
            
            var contentItems = await contentItemRepository.GetAllContentItemsAsync();
            if (contentItems == null || !contentItems.Any())
            {
                logger.LogWarning("No content items found.");
                return NotFound(new { Message = "No content items found." });
            }
            logger.LogInformation($"Found {contentItems.Count()} content items.");
            return Ok(contentItems);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetContentItemById(Guid id)
        {
            if (id == Guid.Empty)
            {
                logger.LogWarning("Received empty GUID for content item ID.");
                return BadRequest(new { Message = "Invalid content item ID." });
            }

            logger.LogInformation($"Fetching content item with ID: {id}");

            var contentItem = await contentItemRepository.GetContentItemByIdAsync(id);
            if (contentItem == null)
            {
                logger.LogWarning($"Content item with ID: {id} not found.");
                return NotFound(new { Message = $"Content item with ID: {id} not found." });
            }
            logger.LogInformation($"Found content item with ID: {id}");
            return Ok(contentItem);
        }

        //[HttpPost]
        //public async Task<IActionResult> AddContentItem([FromBody] BaseContentItemDto contentItemDto)
        //{
        //    logger.LogInformation("Adding a new content item.");

        //    if (contentItemDto == null)
        //    {
        //        logger.LogWarning("Received null content item.");
        //        return BadRequest(new { Message = "Content item cannot be null." });
        //    }
        //    var contentItem = new ContentItem
        //    {
        //        Title = contentItemDto.Title,
        //        Body = contentItemDto.Body,
        //        ModelTypeUsed = contentItemDto.ModelTypeUsed,
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    var newId = await contentItemRepository.AddContentItemAsync(contentItem);
        //    contentItem.Id = newId;
        //    logger.LogInformation($"Added content item with ID: {newId}");

        //    return CreatedAtAction(nameof(GetContentItemById), new { id = contentItem.Id }, contentItem);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddContentItem([FromBody] BaseContentItemDto contentItemDto)
        //{
        //    logger.LogInformation("Adding a new content item.");

        //    if (contentItemDto == null)
        //    {
        //        logger.LogWarning("Received null content item.");
        //        return BadRequest(new { Message = "Content item cannot be null." });
        //    }
        //    // 1. Save the content item to the database
        //    var contentItem = new ContentItem
        //    {
        //        Title = contentItemDto.Title,
        //        Body = contentItemDto.Body,
        //        ModelTypeUsed = contentItemDto.ModelTypeUsed,
        //        CreatedAt = DateTime.UtcNow
        //    };
        //    var newId = await contentItemRepository.AddContentItemAsync(contentItem);
        //    contentItem.Id = newId;
        //    logger.LogInformation($"Added content item with ID: {newId}");

        //    // 2. Return the created content item to ollama
        //    var responseFromOllama = await ollamaService.AskAsync(contentItemDto.Body, contentItemDto.ModelTypeUsed);

        //    // 3. Return the content item and response from ollama

        //    return CreatedAtAction(nameof(GetContentItemById), new { id = contentItem.Id }, new
        //    {
        //        ContentItem = contentItem,
        //        ResponseFromOllama = responseFromOllama
        //    });
        //}

        [HttpPost]
        public async Task<IActionResult> AddContentItem([FromForm] ContentItemUploadDto contentItemDto)
        {
            logger.LogInformation("Adding a new content item.");

            string extractedTextFromDocument = string.Empty;
            string summary = string.Empty;
            bool isFile = false;

            if (contentItemDto.UploadedFile != null)
            {
                isFile = true;
                extractedTextFromDocument = await contentItemRepository.ProcessFileAsync(contentItemDto.UploadedFile);
                if (contentItemDto.UploadedFile.Length > 5 * 1024) // 5kb
                {
                    // Sumarize the extracted text
                    summary = await ollamaService.AskAsync($"Create a summary for:\n{extractedTextFromDocument}", contentItemDto.ModelTypeUsed);

                    Console.WriteLine($"Summary: {summary}");
                }
            }
            

            var contentItem = new ContentItem
            {
                Title = contentItemDto.Title,
                Body = contentItemDto.Body,
                ModelTypeUsed = contentItemDto.ModelTypeUsed,
                CreatedAt = DateTime.UtcNow
            };

            var newId = await contentItemRepository.AddContentItemAsync(contentItem);
            contentItem.Id = newId;
            if (isFile)
            {
                var textDocument = new TextDocument
                {
                    FileName = contentItemDto.UploadedFile.FileName,
                    ExtractedText = extractedTextFromDocument,
                    Summary = summary,
                    ContentItemId = contentItem.Id
                };
                await textDocumentRepository.AddTextDocumentAsync(textDocument);
            }
            logger.LogInformation($"Added content item with ID: {newId}");

            // 2. Return the created content item to ollama
            var responseFromOllama = await ollamaService.AskAsync($"{contentItem.Body}\n{extractedTextFromDocument}", contentItemDto.ModelTypeUsed);

            // 3. Return the content item and response from ollama

            return CreatedAtAction(nameof(GetContentItemById), new { id = contentItem.Id }, new
            {
                ContentItem = contentItem,
                ResponseFromOllama = responseFromOllama
            });
        }


        [HttpPost("ask")]
        public async Task<IActionResult> AskTextDocument([FromBody] AskTextDocumentDto askTextDocumentDto)
        {
            if (askTextDocumentDto == null)
            {
                logger.LogWarning("Received null ask text document request.");
                return BadRequest(new { Message = "Ask text document request cannot be null." });
            }
            logger.LogInformation($"Asking text document with ID: {askTextDocumentDto.TextDocumentId}");
            var existingTextDocument = await textDocumentRepository.GetTextDocumentByIdAsync(askTextDocumentDto.TextDocumentId);
            if (existingTextDocument == null)
            {
                logger.LogWarning($"Text document with ID: {askTextDocumentDto.TextDocumentId} not found.");
                return NotFound(new { Message = $"Text document with ID: {askTextDocumentDto.TextDocumentId} not found." });
            }
            var ollamaResponse = await ollamaService.AskAsync($"The text: {existingTextDocument.ExtractedText}\nThe question about this text: {askTextDocumentDto.Prompt}", askTextDocumentDto.ModelType);
            return Ok(new { Response = ollamaResponse });
        }

        //private async Task<string> ProcessFileAsync(IFormFile uploadedFile)
        //{
        //    if (uploadedFile == null || uploadedFile.Length <= 0)
        //    {
        //        return null;
        //    }
        //    var extension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();

        //    using var stream = uploadedFile.OpenReadStream();

        //    if (extension == ".txt")
        //    {
        //        using var reader = new StreamReader(stream);
        //        return await reader.ReadToEndAsync();
        //    }
        //    else if (extension == ".pdf")
        //    {
        //        using var pdf = PdfDocument.Open(stream);
        //        var text = new StringBuilder();

        //        foreach (var page in pdf.GetPages())
        //            text.AppendLine(page.Text);

        //        return text.ToString();
        //    }
        //    return string.Empty;
        //}

        //public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream)
        //{
        //    using var memoryStream = new MemoryStream();
        //    await pdfStream.CopyToAsync(memoryStream);
        //    memoryStream.Position = 0;

        //    using var document = PdfDocument.Open(memoryStream);
        //    var text = new StringBuilder();

        //    foreach (var page in document.GetPages())
        //    {
        //        text.AppendLine(page.Text);
        //    }

        //    return text.ToString();
        //}

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContentItem(Guid id, [FromBody] ContentItemDto contentItemDto)
        {
            if (id != contentItemDto.Id)
            {
                logger.LogWarning($"ID mismatch: {id} != {contentItemDto.Id}");
                return BadRequest(new { Message = "ID mismatch." });
            }

            logger.LogInformation($"Updating content item with ID: {id}");

            if (contentItemDto == null)
            {
                logger.LogWarning("Received null content item.");
                return BadRequest(new { Message = "Content item cannot be null." });
            }

            var existingContentItem = await contentItemRepository.GetContentItemByIdAsync(id);
            if (existingContentItem == null)
            {
                logger.LogWarning($"Content item with ID: {id} not found.");
                return NotFound(new { Message = $"Content item with ID: {id} not found." });
            }

            existingContentItem.Title = contentItemDto.Title;
            existingContentItem.Body = contentItemDto.Body;
            existingContentItem.ModelTypeUsed = contentItemDto.ModelTypeUsed;

            var result = await contentItemRepository.UpdateContentItemAsync(existingContentItem);
            if (!result)
            {
                logger.LogWarning($"Content item with ID: {id} not found.");
                return NotFound(new { Message = $"Content item with ID: {id} not found." });
            }
            logger.LogInformation($"Updated content item with ID: {id}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContentItem(Guid id)
        {
            if (id == Guid.Empty)
            {
                logger.LogWarning("Received empty GUID for content item ID.");
                return BadRequest(new { Message = "Invalid content item ID." });
            }

            logger.LogInformation($"Deleting content item with ID: {id}");

            var result = await contentItemRepository.DeleteContentItemAsync(id);
            if (!result)
            {
                logger.LogWarning($"Content item with ID: {id} not found.");
                return NotFound(new { Message = $"Content item with ID: {id} not found." });
            }
            logger.LogInformation($"Deleted content item with ID: {id}");
            return NoContent();
        }
    }
}
