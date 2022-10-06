using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Queries
{
    public class GenreQueryService : IGenreQueryService
    {
        private readonly AppDbContext _ctx;
        private IQueryable<Genre> Genres;

        public GenreQueryService(AppDbContext ctx)
        {
            _ctx = ctx;
            Genres = _ctx.Genres.AsQueryable();
        }

        public async Task<int> GetTotalGenresCountAsync()
        {
            return await Genres.CountAsync();
        }

        public async Task<int> GetFilteredGenresCountAsync(string searchValue)
        {
            ApplySearchFilter(searchValue);
            return await Genres.CountAsync();
        }

        public async Task<IEnumerable<Genre>> GetGenresPaginatedAsync(string sortColumn, string sortDirection, int start, int length)
        {
            ApplySorting(sortColumn, sortDirection);
            ApplyPagination(start, length);
            IncludeRelatedEntites();

            var genres = await Genres.ToListAsync();
            return genres;
        }

        private void ApplySearchFilter(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToLower();
                Genres = Genres.Where(x => x.Name.ToLower().Contains(value));
            }
        }

        private void ApplySorting(string column, string direction)
        {
            if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(direction))
            {
                Genres = Genres.OrderBy(column, direction == "asc");
            }
        }

        private void ApplyPagination(int skip, int take)
        {
            Genres = Genres.Skip(skip).Take(take);
        }

        private void IncludeRelatedEntites()
        {
            Genres = Genres.Include(b => b.BookLinks)
                           .ThenInclude(l => l.Book);
        }
    }
}
