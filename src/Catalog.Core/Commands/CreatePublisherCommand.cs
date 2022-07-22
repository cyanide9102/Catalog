using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record CreatePublisherCommand(string Name, string? Country = null) : IRequest<Publisher>;
}
