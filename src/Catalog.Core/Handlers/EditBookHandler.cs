using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class EditBookHandler : IRequestHandler<EditBookCommand, Book>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Book> _repository;

        public EditBookHandler(IMediator mediator, IRepository<Book> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Book> Handle(EditBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(request.Id), cancellationToken);
            book.Update(request.Title, request.Description, request.Price, request.Pages, request.PublishedOn);
            book.UpdatePublisher(request.Publisher);

            if (request.Authors != null)
            {
                var authorLinksToDelete = book.AuthorLinks.Where(l => !request.Authors.Any(a => a.Id == l.AuthorId));
                foreach (var link in authorLinksToDelete)
                {
                    book.RemoveAuthorLink(link);
                }

                var authorsToAdd = request.Authors.Where(a => !book.AuthorLinks.Any(l => l.AuthorId == a.Id));
                foreach (var author in authorsToAdd)
                {
                    var authorLink = new BookAuthor(book.Id, author.Id);
                    book.AddAuthorLink(authorLink);
                }
            }
            else
            {
                foreach (var link in book.AuthorLinks)
                {
                    book.RemoveAuthorLink(link);
                }
            }

            if (request.Genres != null)
            {
                var genreLinksToDelete = book.GenreLinks.Where(l => !request.Genres.Any(a => a.Id == l.GenreId));
                foreach (var link in genreLinksToDelete)
                {
                    book.RemoveGenreLink(link);
                }

                var genresToAdd = request.Genres.Where(a => !book.GenreLinks.Any(l => l.GenreId == a.Id));
                foreach (var genre in genresToAdd)
                {
                    var genreLink = new BookGenre(book.Id, genre.Id);
                    book.AddGenreLink(genreLink);
                }
            }
            else
            {
                foreach (var link in book.GenreLinks)
                {
                    book.RemoveGenreLink(link);
                }
            }

            if (request.Tags != null)
            {
                var tagLinksToDelete = book.TagLinks.Where(l => !request.Tags.Any(a => a.Id == l.TagId));
                foreach (var link in tagLinksToDelete)
                {
                    book.RemoveTagLink(link);
                }

                var tagsToAdd = request.Tags.Where(a => !book.TagLinks.Any(l => l.TagId == a.Id));
                foreach (var tag in tagsToAdd)
                {
                    var tagLink = new BookTag(book.Id, tag.Id);
                    book.AddTagLink(tagLink);
                }
            }
            else
            {
                foreach (var link in book.TagLinks)
                {
                    book.RemoveTagLink(link);
                }
            }

            await _repository.SaveChangesAsync(cancellationToken);
            return book;
        }
    }
}
