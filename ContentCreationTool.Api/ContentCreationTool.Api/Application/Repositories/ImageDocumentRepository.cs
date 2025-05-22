using ContentCreationTool.Api.Domain.Entities;
using ContentCreationTool.Api.Infrastructure;

namespace ContentCreationTool.Api.Application.Repositories
{
    public class ImageDocumentRepository : IImageDocumentRepository
    {
        private readonly ApplicationDbContext context;

        public ImageDocumentRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public Task<Guid> AddImageDocumentAsync(ImageDocument imageDocument)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteImageDocumentAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ImageDocument>> GetAllImageDocumentsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<ImageDocument> GetImageDocumentByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateImageDocumentAsync(ImageDocument imageDocument)
        {
            throw new NotImplementedException();
        }
    }
}
