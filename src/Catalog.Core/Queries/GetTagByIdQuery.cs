using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetTagByIdQuery(Guid Id) : IRequest<Tag>;
}
