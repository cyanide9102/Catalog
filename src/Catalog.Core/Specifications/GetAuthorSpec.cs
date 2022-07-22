using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
    public class GetAuthorSpec : Specification<Author>, ISingleResultSpecification
    {
        public GetAuthorSpec(Guid id)
        {
            Query.Where(a => a.Id == id)
                 .Include(a => a.BookLinks)
                 .ThenInclude(x => x.Book);
        }
        public GetAuthorSpec(string name)
        {
            Query.Where(a => a.Name == name)
                 .Include(a => a.BookLinks)
                 .ThenInclude(x => x.Book);
        }
    }
}
