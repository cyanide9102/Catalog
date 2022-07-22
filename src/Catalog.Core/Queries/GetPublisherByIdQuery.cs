using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetPublisherByIdQuery(Guid Id) : IRequest<Publisher>;
}
