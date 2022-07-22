using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Config
{
    public class BookGenreConfiguration : IEntityTypeConfiguration<BookGenre>
    {
        public void Configure(EntityTypeBuilder<BookGenre> builder)
        {
            builder.HasKey(x => new { x.BookId, x.GenreId });

            builder.HasOne(x => x.Book)
                   .WithMany(x => x.GenreLinks)
                   .HasForeignKey(x => x.BookId);

            builder.HasOne(x => x.Genre)
                   .WithMany(x => x.BookLinks)
                   .HasForeignKey(x => x.GenreId);
        }
    }
}
