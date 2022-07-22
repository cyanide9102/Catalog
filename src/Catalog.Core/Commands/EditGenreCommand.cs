using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record EditGenreCommand(Guid Id, string Name) : IRequest<Genre>;
}
