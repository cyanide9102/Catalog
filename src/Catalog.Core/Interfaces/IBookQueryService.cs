namespace Catalog.Core.Interfaces
{
    public interface IBookQueryService
    {
        Task<object> GetBookList(int draw, string? searchTerm = "", string? sortColumn = "", string? sortColumnDirection = "", int skip = 0, int pageSize = -1);
    }
}
