using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record CreateAuthorCommand(string Name) : IRequest<Author>;
}
