using ContentCreationTool.Api.Domain.Entities;
using ContentCreationTool.Api.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Text;
using UglyToad.PdfPig;

namespace ContentCreationTool.Api.Application.Repositories
{
    public class ContentItemRepository : IContentItemRepository
    {
        private readonly ApplicationDbContext context;

        public ContentItemRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Guid> AddContentItemAsync(ContentItem contentItem)
        {
            await context.ContentItems.AddAsync(contentItem);
            await context.SaveChangesAsync();
            return contentItem.Id;
        }

        public async Task<bool> DeleteContentItemAsync(Guid id)
        {
            var contentItem = await context.ContentItems.FindAsync(id);
            if (contentItem == null)
            {
                return false;
            }
            context.ContentItems.Remove(contentItem);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<string> ExtractTextFromPdfAsync(Stream pdfStream)
        {
            using var memoryStream = new MemoryStream();
            await pdfStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            using var document = PdfDocument.Open(memoryStream);
            var text = new StringBuilder();

            foreach (var page in document.GetPages())
            {
                text.AppendLine(page.Text);
            }

            return text.ToString();
        }

        public async Task<IEnumerable<ContentItem>> GetAllContentItemsAsync() => await context.ContentItems
                .Include(c => c.TextDocument)
                .Include(c => c.ImageDocument)
                .ToListAsync();

        public async Task<ContentItem> GetContentItemByIdAsync(Guid id) => await context.ContentItems
                .Include(c => c.TextDocument)
                .Include(c => c.ImageDocument)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<string> ProcessFileAsync(IFormFile uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.Length <= 0)
            {
                return null;
            }
            var extension = Path.GetExtension(uploadedFile.FileName).ToLowerInvariant();

            using var stream = uploadedFile.OpenReadStream();

            if (extension == ".txt")
            {
                using var reader = new StreamReader(stream);
                return await reader.ReadToEndAsync();
            }
            else if (extension == ".pdf")
            {
                using var pdf = PdfDocument.Open(stream);
                var text = new StringBuilder();

                foreach (var page in pdf.GetPages())
                    text.AppendLine(page.Text);

                return text.ToString();
            }
            return string.Empty;
        }

        public async Task<bool> UpdateContentItemAsync(ContentItem contentItem)
        {
            var existingContentItem = await context.ContentItems.FindAsync(contentItem.Id);
            if (existingContentItem == null)
            {
                return false;
            }
            //context.ContentItems.Update(contentItem);
            //var result = await context.SaveChangesAsync();
            //return result > 0;

            context.Entry(existingContentItem).CurrentValues.SetValues(contentItem);
            await context.SaveChangesAsync();
            return true;
        }     
    }
}
