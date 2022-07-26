using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface IBookQueryService
    {
        Task<int> GetTotalBooksCountAsync();
        Task<int> GetFilteredBooksCountAsync(string searchValue);
        Task<IEnumerable<Book>> GetBooksPaginatedAsync(string sortColumn, string sortDirection, int start, int length);
    }
}
