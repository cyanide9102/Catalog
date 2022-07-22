using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
    public class GetGenreSpec : Specification<Genre>, ISingleResultSpecification
    {
        public GetGenreSpec(Guid id)
        {
            Query.Where(g => g.Id == id)
                 .Include(g => g.BookLinks)
                 .ThenInclude(x => x.Book);
        }

        public GetGenreSpec(string name)
        {
            Query.Where(g => g.Name == name)
                 .Include(g => g.BookLinks)
                 .ThenInclude(x => x.Book);
        }
    }
}
