using ContentCreationTool.Api.Application.DTOs;
using ContentCreationTool.Api.Domain.Entities;
using ContentCreationTool.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace ContentCreationTool.Api.Application.Repositories
{
    public class TextDocumentRepository : ITextDocumentRepository
    {
        private readonly ApplicationDbContext context;

        public TextDocumentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Guid> AddTextDocumentAsync(TextDocument textDocument)
        {
            await context.TextDocuments.AddAsync(textDocument);
            await context.SaveChangesAsync();
            return textDocument.Id;
        }

        public async Task<bool> DeleteTextDocumentAsync(Guid id)
        {
            var textDocument = await context.TextDocuments.FindAsync(id);
            if (textDocument == null)
            {
                return false;
            }
            context.TextDocuments.Remove(textDocument);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TextDocumentDto>> GetAllTextDocumentsAsync() => await context.TextDocuments
                .Include(t => t.ContentItem)
                .Select(t => new TextDocumentDto
                {
                    Id = t.Id,
                    FileName = t.FileName,
                    ExtractedText = t.ExtractedText,
                    Summary = t.Summary,
                    ContentItemId = t.ContentItemId,
                })
                .ToListAsync();

        public async Task<TextDocumentDto> GetTextDocumentByIdAsync(Guid id) => await context.TextDocuments
                .Include(t => t.ContentItem)
                .Where(t => t.Id == id)
                .Select(t => new TextDocumentDto
                {
                    Id = t.Id,
                    FileName = t.FileName,
                    ExtractedText = t.ExtractedText,
                    Summary = t.Summary,
                    ContentItemId = t.ContentItemId,
                })
                .FirstOrDefaultAsync();

        public async Task<bool> UpdateTextDocumentAsync(TextDocumentDto textDocumentDto)
        {
            var existingTextDocument = await context.TextDocuments.FindAsync(textDocumentDto.Id);
            if (existingTextDocument == null)
            {
                return false;
            }
            context.Entry(existingTextDocument).CurrentValues.SetValues(textDocumentDto);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<TextDocument> UploadTextDocumentAsync(IFormFile file, Guid contentItemId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File cannot be null or empty.", nameof(file));
            }

            string extractedText;
            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
            {
                extractedText = await reader.ReadToEndAsync();
            }

            var textDocument = new TextDocument
            {
                FileName = file.FileName,
                ExtractedText = extractedText,
                Summary = string.Empty,
                ContentItemId = contentItemId
            };

            context.TextDocuments.Add(textDocument);
            await context.SaveChangesAsync();

            return textDocument;
        }
    }
}
