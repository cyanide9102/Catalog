using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Data.Queries
{
    public class BookQueryService : IBookQueryService
    {
        private readonly AppDbContext _ctx;

        public BookQueryService(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<object> GetBookList(int draw, string? searchTerm = "", string? sortColumn = "", string? sortColumnDirection = "", int skip = 0, int pageSize = -1)
        {
            var books = _ctx.Books.AsQueryable();

            int recordsTotal = books.Count();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                books = books.Where(x => x.Title.ToLower().Contains(searchTerm.ToLower()) || x.Description.ToLower().Contains(searchTerm.ToLower()) || x.Price.ToString().ToLower().Contains(searchTerm.ToLower()));
            }

            int recordsFiltered = books.Count();

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortColumnDirection))
            {
                books = books.OrderBy(sortColumn, sortColumnDirection);
            }

            var data = await books.Skip(skip).Take(pageSize).ToListAsync();
            var returnObj = new { draw, recordsTotal, recordsFiltered, data };
            return returnObj;
        }
    }
}
