using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface IPublisherQueryService
    {
        Task<int> GetTotalPublishersCountAsync();
        Task<int> GetFilteredPublishersCountAsync(string searchValue);
        Task<IEnumerable<Publisher>> GetPublishersPaginatedAsync(string sortColumn, string sortDirection, int start, int length);
    }
}
