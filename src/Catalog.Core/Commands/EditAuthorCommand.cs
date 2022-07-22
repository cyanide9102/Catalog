using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record EditAuthorCommand(Guid Id, string Name) : IRequest<Author>;
}
