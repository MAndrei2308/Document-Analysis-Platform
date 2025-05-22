using ContentCreationTool.Api.Domain.Entities;

namespace ContentCreationTool.Api.Application.Repositories
{
    public interface IImageDocumentRepository
    {
        Task<ImageDocument> GetImageDocumentByIdAsync(Guid id);
        Task<IEnumerable<ImageDocument>> GetAllImageDocumentsAsync();
        Task<Guid> AddImageDocumentAsync(ImageDocument imageDocument);
        Task<bool> UpdateImageDocumentAsync(ImageDocument imageDocument);
        Task<bool> DeleteImageDocumentAsync(Guid id);
    }
}
