using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record CreateGenreCommand(string Name) : IRequest<Genre>;
}
