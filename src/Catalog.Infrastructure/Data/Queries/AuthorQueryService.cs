using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Queries
{
    public class AuthorQueryService : IAuthorQueryService
    {
        private readonly AppDbContext _ctx;
        private IQueryable<Author> Authors;

        public AuthorQueryService(AppDbContext ctx)
        {
            _ctx = ctx;

            Authors = _ctx.Authors.AsQueryable();
        }

        public async Task<int> GetTotalAuthorsCountAsync()
        {
            return await Authors.CountAsync();
        }

        public async Task<int> GetFilteredAuthorsCountAsync(string searchValue)
        {
            ApplySearchFilter(searchValue);
            return await Authors.CountAsync();
        }

        public async Task<IEnumerable<Author>> GetAuthorsPaginatedAsync(string sortColumn, string sortDirection, int start, int length)
        {
            ApplySorting(sortColumn, sortDirection);
            ApplyPagination(start, length);
            IncludeRelatedEntites();

            var books = await Authors.ToListAsync();
            return books;
        }

        private void ApplySearchFilter(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToLower();
                Authors = Authors.Where(x => x.Name.ToLower().Contains(value));
            }
        }

        private void ApplySorting(string column, string direction)
        {
            if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(direction))
            {
                Authors = Authors.OrderBy(column, direction);
            }
        }

        private void ApplyPagination(int skip, int take)
        {
            Authors = Authors.Skip(skip).Take(take);
        }

        private void IncludeRelatedEntites()
        {
            Authors = Authors.Include(b => b.BookLinks)
                             .ThenInclude(l => l.Book);
        }
    }
}
