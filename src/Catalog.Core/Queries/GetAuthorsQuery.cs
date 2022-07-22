using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetAuthorsQuery() : IRequest<IEnumerable<Author>>;
}
