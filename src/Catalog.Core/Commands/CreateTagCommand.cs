using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record CreateTagCommand(string Name) : IRequest<Tag>;
}
