using ContentCreationTool.Api.Domain.Entities;

namespace ContentCreationTool.Api.Application.Repositories
{
    public interface IContentItemRepository
    {
        Task<ContentItem> GetContentItemByIdAsync(Guid id);
        Task<IEnumerable<ContentItem>> GetAllContentItemsAsync();
        Task<Guid> AddContentItemAsync(ContentItem contentItem);
        Task<bool> UpdateContentItemAsync(ContentItem contentItem);
        Task<bool> DeleteContentItemAsync(Guid id);
        Task<string> ProcessFileAsync(IFormFile uploadedFile);
        Task<string> ExtractTextFromPdfAsync(Stream pdfStream);

    }
}
