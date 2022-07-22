using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Config
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(b => b.Title).IsRequired().HasMaxLength(256);
            builder.Property(b => b.Description).HasMaxLength(1024);
            builder.Property(b => b.Price).IsRequired().HasColumnType("money");
            builder.Property(b => b.PublishedOn).HasColumnType("date");

            builder.HasOne(b => b.Publisher)
                   .WithMany(p => p.Books)
                   .HasForeignKey(b => b.PublisherId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .IsRequired(false);
        }
    }
}
