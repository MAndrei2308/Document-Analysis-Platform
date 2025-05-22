using ContentCreationTool.Api.Application.DTOs;
using ContentCreationTool.Api.Domain.Entities;

namespace ContentCreationTool.Api.Application.Repositories
{
    public interface ITextDocumentRepository
    {
        Task<TextDocumentDto> GetTextDocumentByIdAsync(Guid id);
        Task<IEnumerable<TextDocumentDto>> GetAllTextDocumentsAsync();
        Task<Guid> AddTextDocumentAsync(TextDocument textDocument);

        Task<TextDocument> UploadTextDocumentAsync(IFormFile file, Guid contentItemId);
        Task<bool> UpdateTextDocumentAsync(TextDocumentDto textDocumentDto);
        Task<bool> DeleteTextDocumentAsync(Guid id);
    }
}
