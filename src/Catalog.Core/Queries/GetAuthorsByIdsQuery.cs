using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetAuthorsByIdsQuery(IEnumerable<Guid> Ids) : IRequest<IEnumerable<Author>>;
}
