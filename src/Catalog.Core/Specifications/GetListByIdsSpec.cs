using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
    public class GetListByIdsSpec<T> : Specification<T>, ISingleResultSpecification where T : EntityBase
    {
        public GetListByIdsSpec(IEnumerable<Guid> ids)
        {
            Query.Where(x => ids.Contains(x.Id));
        }
    }
}
