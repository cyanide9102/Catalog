using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record EditPublisherCommand(Guid Id, string Name, string? Country = null) : IRequest<Publisher>;
}
