using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Queries
{
    public class PublisherQueryService : IPublisherQueryService
    {
        private readonly AppDbContext _ctx;
        private IQueryable<Publisher> Publishers;

        public PublisherQueryService(AppDbContext ctx)
        {
            _ctx = ctx;
            Publishers = _ctx.Publishers.AsQueryable();
        }

        public async Task<int> GetTotalPublishersCountAsync()
        {
            return await Publishers.CountAsync();
        }

        public async Task<int> GetFilteredPublishersCountAsync(string searchValue)
        {
            ApplySearchFilter(searchValue);
            return await Publishers.CountAsync();
        }

        public async Task<IEnumerable<Publisher>> GetPublishersPaginatedAsync(string sortColumn, string sortDirection, int start, int length)
        {
            ApplySorting(sortColumn, sortDirection);
            ApplyPagination(start, length);
            IncludeRelatedEntites();

            var books = await Publishers.ToListAsync();
            return books;
        }

        private void ApplySearchFilter(string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.ToLower();
                Publishers = Publishers.Where(x => x.Name.ToLower().Contains(value) || !string.IsNullOrEmpty(x.Country) && x.Country.ToLower().Contains(value));
            }
        }

        private void ApplySorting(string column, string direction)
        {
            if (!string.IsNullOrEmpty(column) && !string.IsNullOrEmpty(direction))
            {
                Publishers = Publishers.OrderBy(column, direction == "asc");
            }
        }

        private void ApplyPagination(int skip, int take)
        {
            Publishers = Publishers.Skip(skip).Take(take);
        }

        private void IncludeRelatedEntites()
        {
            Publishers = Publishers.Include(b => b.Books);
        }
    }
}
