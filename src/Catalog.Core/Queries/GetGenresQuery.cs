using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetGenresQuery() : IRequest<IEnumerable<Genre>>;
}
