using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetAuthorByIdQuery(Guid Id) : IRequest<Author>;
}
