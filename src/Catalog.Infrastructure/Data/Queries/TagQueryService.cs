using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Queries
{
    public class TagQueryService : ITagQueryService
    {
        private readonly AppDbContext _ctx;
        private IQueryable<Tag> Tags;

        public TagQueryService(AppDbContext ctx)
        {
            _ctx = ctx;
            Tags = _ctx.Tags.AsQueryable();
        }

        public async Task<int> GetTotalTagsCountAsync()
        {
            return await Tags.CountAsync();
        }

        public async Task<int> GetFilteredTagsCountAsync(string searchValue)
        {
            ApplySearchFilter(searchValue);
            return await Tags.CountAsync();
        }

        public async Task<IEnumerable<Tag>> GetTagsPaginatedAsync(string sortColumn, string sortDirection, int start, int length)
        {
            ApplySorting(sortColumn, sortDirection);
            ApplyPagination(start, length);
            IncludeRelatedEntites();

            var tags = await Tags.ToListAsync();
            return tags;
        }

        private void ApplySearchFilter(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToLower();
                Tags = Tags.Where(x => x.Name.ToLower().Contains(value));
            }
        }

        private void ApplySorting(string column, string direction)
        {
            if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(direction))
            {
                Tags = Tags.OrderBy(column, direction == "asc");
            }
        }

        private void ApplyPagination(int skip, int take)
        {
            Tags = Tags.Skip(skip).Take(take);
        }

        private void IncludeRelatedEntites()
        {
            Tags = Tags.Include(b => b.BookLinks)
                       .ThenInclude(l => l.Book);
        }
    }
}
