namespace ContentCreationTool.Api.Application.DTOs
{
    public class ContentItemUploadDto : BaseContentItemDto
    {
        public IFormFile? UploadedFile { get; set; }
    }
}
