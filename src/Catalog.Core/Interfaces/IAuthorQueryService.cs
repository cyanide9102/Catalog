using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface IAuthorQueryService
    {
        Task<int> GetTotalAuthorsCountAsync();
        Task<int> GetFilteredAuthorsCountAsync(string searchValue);
        Task<IEnumerable<Author>> GetAuthorsPaginatedAsync(string sortColumn, string sortDirection, int start, int length);
    }
}
