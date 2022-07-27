using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record DeleteBookCommand(Guid Id) : IRequest;
}
