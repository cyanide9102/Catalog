using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
    public class GetTagSpec : Specification<Tag>, ISingleResultSpecification
    {
        public GetTagSpec(Guid id)
        {
            Query.Where(g => g.Id == id)
                 .Include(g => g.BookLinks)
                 .ThenInclude(x => x.Book);
        }

        public GetTagSpec(string name)
        {
            Query.Where(g => g.Name == name)
                 .Include(g => g.BookLinks)
                 .ThenInclude(x => x.Book);
        }
    }
}
