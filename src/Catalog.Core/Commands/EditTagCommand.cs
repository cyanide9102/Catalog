using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record EditTagCommand(Guid Id, string Name) : IRequest<Tag>;
}
