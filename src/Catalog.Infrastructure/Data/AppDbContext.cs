using Catalog.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Catalog.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors => Set<Author>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<Publisher> Publishers => Set<Publisher>();
        public DbSet<Tag> Tags => Set<Tag>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ChangeTracker.DetectChanges();
            foreach (var entry in ChangeTracker.Entries().ToList())
            {
                if (entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                // TODO: Implement auditing
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
