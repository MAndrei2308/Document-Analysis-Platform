using ContentCreationTool.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentCreationTool.Api.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ContentItem> ContentItems { get; set; }
        public DbSet<TextDocument> TextDocuments { get; set; }
        public DbSet<ImageDocument> ImageDocuments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship 1 to 1 between ContentItem and TextDocument
            modelBuilder.Entity<ContentItem>()
                .HasOne(c => c.TextDocument)
                .WithOne(t => t.ContentItem)
                .HasForeignKey<TextDocument>(t => t.ContentItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship 1 to 1 between ContentItem and ImageDocument
            modelBuilder.Entity<ContentItem>()
                .HasOne(c => c.ImageDocument)
                .WithOne(i => i.ContentItem)
                .HasForeignKey<ImageDocument>(i => i.ContentItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
