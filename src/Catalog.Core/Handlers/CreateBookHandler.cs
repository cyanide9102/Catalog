using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class CreateBookHandler : IRequestHandler<CreateBookCommand, Book>
    {
        private readonly IRepository<Book> _repository;

        public CreateBookHandler(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<Book> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Book(request.Title, request.Description, request.Price, request.Pages, request.PublishedOn);
            book.UpdatePublisher(request.Publisher);

            var authorLinks = new HashSet<BookAuthor>();
            if (request.Authors != null)
            {
                foreach (var author in request.Authors)
                {
                    var authorLink = new BookAuthor(book.Id, author.Id);
                    authorLinks.Add(authorLink);
                }
            }
            book.UpdateAuthorLinks(authorLinks);

            var genreLinks = new HashSet<BookGenre>();
            if (request.Genres != null)
            {
                foreach (var genre in request.Genres)
                {
                    var genreLink = new BookGenre(book.Id, genre.Id);
                    genreLinks.Add(genreLink);
                }
            }
            book.UpdateGenreLinks(genreLinks);

            var tagLinks = new HashSet<BookTag>();
            if (request.Tags != null)
            {
                foreach (var tag in request.Tags)
                {
                    var tagLink = new BookTag(book.Id, tag.Id);
                    tagLinks.Add(tagLink);
                }
            }
            book.UpdateTagLinks(tagLinks);

            book = await _repository.AddAsync(book, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return book;
        }
    }
}
