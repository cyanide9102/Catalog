using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetAuthorByNameQuery(string Name) : IRequest<Author?>;
}
