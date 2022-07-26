using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface IGenreQueryService
    {
        Task<int> GetTotalGenresCountAsync();
        Task<int> GetFilteredGenresCountAsync(string searchValue);
        Task<IEnumerable<Genre>> GetGenresPaginatedAsync(string sortColumn, string sortDirection, int start, int length);
    }
}
