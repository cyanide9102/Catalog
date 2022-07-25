using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetGenresByIdsQuery(IEnumerable<Guid> Ids) : IRequest<IEnumerable<Genre>>;
}
