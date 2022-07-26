using Catalog.Core.Entities;

namespace Catalog.Core.Interfaces
{
    public interface ITagQueryService
    {
        Task<int> GetTotalTagsCountAsync();
        Task<int> GetFilteredTagsCountAsync(string searchValue);
        Task<IEnumerable<Tag>> GetTagsPaginatedAsync(string sortColumn, string sortDirection, int start, int length);
    }
}
