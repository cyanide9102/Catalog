using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetTagsByIdsQuery(IEnumerable<Guid> Ids) : IRequest<IEnumerable<Tag>>;
}
