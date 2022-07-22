using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
    public class GetPublisherSpec : Specification<Publisher>, ISingleResultSpecification
    {
        public GetPublisherSpec(Guid id)
        {
            Query.Where(p => p.Id == id)
                 .Include(p => p.Books);
        }

        public GetPublisherSpec(string name)
        {
            Query.Where(p => p.Name == name)
                 .Include(p => p.Books);
        }
    }
}
