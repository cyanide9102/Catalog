using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetGenreByIdQuery(Guid Id) : IRequest<Genre>;
}
