using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Commands
{
    public record CreateBookCommand(string Title, string Description, decimal Price, short? Pages = null, DateTime? PublishedOn = null, Publisher? Publisher = null, IEnumerable<Author>? Authors = null, IEnumerable<Genre>? Genres = null, IEnumerable<Tag>? Tags = null) : IRequest<Book>;
}
