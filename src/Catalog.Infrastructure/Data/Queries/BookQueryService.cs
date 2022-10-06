using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Queries
{
    public class BookQueryService : IBookQueryService
    {
        private readonly AppDbContext _ctx;
        private IQueryable<Book> Books;

        public BookQueryService(AppDbContext ctx)
        {
            _ctx = ctx;

            Books = _ctx.Books.AsQueryable();
        }

        public async Task<int> GetTotalBooksCountAsync()
        {
            return await Books.CountAsync();
        }

        public async Task<int> GetFilteredBooksCountAsync(string searchValue)
        {
            ApplySearchFilter(searchValue);
            return await Books.CountAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksPaginatedAsync(string sortColumn, string sortDirection, int start, int length)
        {
            ApplySorting(sortColumn, sortDirection);
            ApplyPagination(start, length);
            IncludeRelatedEntites();

            var books = await Books.ToListAsync();
            return books;
        }

        private void ApplySearchFilter(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToLower();
                Books = Books.Where(x => x.Title.ToLower().Contains(value) || x.AuthorLinks.Any(l => l.Author.Name.ToLower().Contains(value)) || x.GenreLinks.Any(l => l.Genre.Name.ToLower().Contains(value)) || x.Publisher != null && x.Publisher.Name.Contains(value) || x.Price.ToString().ToLower().Contains(value));
            }
        }

        private void ApplySorting(string column, string direction)
        {
            if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(direction))
            {
                Books = Books.OrderBy(column, direction == "asc");
            }
        }

        private void ApplyPagination(int skip, int take)
        {
            Books = Books.Skip(skip).Take(take);
        }

        private void IncludeRelatedEntites()
        {
            Books = Books.Include(b => b.Publisher)
                         .Include(b => b.AuthorLinks)
                         .ThenInclude(l => l.Author)
                         .Include(b => b.GenreLinks)
                         .ThenInclude(l => l.Genre)
                         .Include(b => b.TagLinks)
                         .ThenInclude(l => l.Tag);
        }
    }
}
