using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Config
{
    public class BookAuthorConfiguration : IEntityTypeConfiguration<BookAuthor>
    {
        public void Configure(EntityTypeBuilder<BookAuthor> builder)
        {
            builder.HasKey(x => new { x.BookId, x.AuthorId });

            builder.HasOne(x => x.Book)
                   .WithMany(x => x.AuthorLinks)
                   .HasForeignKey(x => x.BookId);

            builder.HasOne(x => x.Author)
                   .WithMany(x => x.BookLinks)
                   .HasForeignKey(x => x.AuthorId);
        }
    }
}
