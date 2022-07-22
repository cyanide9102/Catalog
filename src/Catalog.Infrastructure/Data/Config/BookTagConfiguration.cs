using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Config
{
    public class BookTagConfiguration : IEntityTypeConfiguration<BookTag>
    {
        public void Configure(EntityTypeBuilder<BookTag> builder)
        {
            builder.HasKey(x => new { x.BookId, x.TagId });

            builder.HasOne(x => x.Book)
                   .WithMany(x => x.TagLinks)
                   .HasForeignKey(x => x.BookId);

            builder.HasOne(x => x.Tag)
                   .WithMany(x => x.BookLinks)
                   .HasForeignKey(x => x.TagId);
        }
    }
}
