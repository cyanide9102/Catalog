using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetBookByIdQuery(Guid Id) : IRequest<Book>;
}
