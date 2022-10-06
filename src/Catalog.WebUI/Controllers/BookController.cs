using Catalog.Core.Commands;
using Catalog.Core.Constants;
using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.BookViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IMediator _mediator;
        private IReadRepository<Book> _bookRepository;

        public BookController(IMediator mediator, IReadRepository<Book> bookRepository)
        {
            _mediator = mediator;
            _bookRepository = bookRepository;
        }

        [HttpPost]
        public IActionResult GetBookList([FromForm] DtRequest dt)
        {
            try
            {
                var query = _bookRepository.Get();
                query = query.Include(b => b.Publisher)
                             .Include(b => b.AuthorLinks)
                             .ThenInclude(l => l.Author)
                             .Include(b => b.GenreLinks)
                             .ThenInclude(l => l.Genre)
                             .Include(b => b.TagLinks)
                             .ThenInclude(l => l.Tag);

                int recordsTotal = query.Count();

                foreach (var column in dt.Columns)
                {
                    if (!string.IsNullOrWhiteSpace(column.Search.Value))
                    {
                        bool valueIsNumeric = decimal.TryParse(column.Search.Value, out decimal numericValue);
                        if (valueIsNumeric)
                        {
                            query = query.Where(column.Name, numericValue, NumberCondition.Equal);
                        }
                        else
                        {
                            query = query.Where(column.Name, column.Search.Value, StringCondition.Contains);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(dt.Search.Value))
                {
                    query = query.Where(dt.Search.Value, StringCondition.Contains);
                }

                int recordsFiltered = query.Count();

                if (dt.Order.Length > 0)
                {
                    query = query.OrderBy(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir == "asc");
                }

                query = query.Skip(dt.Start).Take(dt.Length);

                var books = query.ToList();
                var result = new DtResponse<Book>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = books,
                    Error = ""
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.InnerException != null ? e.InnerException.Message : e.Message);
            }
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var books = await _mediator.Send(new GetBooksQuery());
            return View(books);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));
            return View(book);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var viewModel = new BookCreateViewModel()
            {
                Authors = await _mediator.Send(new GetAuthorsQuery()),
                Publishers = await _mediator.Send(new GetPublishersQuery()),
                Genres = await _mediator.Send(new GetGenresQuery()),
                Tags = await _mediator.Send(new GetTagsQuery())
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] BookCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Authors = await _mediator.Send(new GetAuthorsQuery());
                viewModel.Publishers = await _mediator.Send(new GetPublishersQuery());
                viewModel.Genres = await _mediator.Send(new GetGenresQuery());
                viewModel.Tags = await _mediator.Send(new GetTagsQuery());
                return View(viewModel);
            }

            var publisher = viewModel.PublisherId != Guid.Empty ? await _mediator.Send(new GetPublisherByIdQuery(viewModel.PublisherId)) : null;
            var authors = viewModel.AuthorIds != null && viewModel.AuthorIds.Any() ? await _mediator.Send(new GetAuthorsByIdsQuery(viewModel.AuthorIds)) : null;
            var genres = viewModel.GenreIds != null && viewModel.GenreIds.Any() ? await _mediator.Send(new GetGenresByIdsQuery(viewModel.GenreIds)) : null;
            var tags = viewModel.TagIds != null && viewModel.TagIds.Any() ? await _mediator.Send(new GetTagsByIdsQuery(viewModel.TagIds)) : null;

            var book = await _mediator.Send(new CreateBookCommand(viewModel.Title, viewModel.Description, viewModel.Price, viewModel.Pages, viewModel.PublishedOn, publisher, authors, genres, tags));
            return RedirectToAction(nameof(Info), new { book.Id });
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(id));

            var authorIds = new List<Guid>();
            foreach (var link in book.AuthorLinks)
            {
                authorIds.Add(link.AuthorId);
            }

            var genreIds = new List<Guid>();
            foreach (var link in book.GenreLinks)
            {
                genreIds.Add(link.GenreId);
            }

            var tagIds = new List<Guid>();
            foreach (var link in book.TagLinks)
            {
                tagIds.Add(link.TagId);
            }
            var viewModel = new BookEditViewModel()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                Price = book.Price,
                Pages = book.Pages,
                PublishedOn = book.PublishedOn,
                PublisherId = book.PublisherId ?? Guid.Empty,
                AuthorIds = authorIds,
                GenreIds = genreIds,
                TagIds = tagIds,
                Authors = await _mediator.Send(new GetAuthorsQuery()),
                Publishers = await _mediator.Send(new GetPublishersQuery()),
                Genres = await _mediator.Send(new GetGenresQuery()),
                Tags = await _mediator.Send(new GetTagsQuery())
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] BookEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Authors = await _mediator.Send(new GetAuthorsQuery());
                viewModel.Publishers = await _mediator.Send(new GetPublishersQuery());
                viewModel.Genres = await _mediator.Send(new GetGenresQuery());
                viewModel.Tags = await _mediator.Send(new GetTagsQuery());
                return View(viewModel);
            }

            var publisher = viewModel.PublisherId != Guid.Empty ? await _mediator.Send(new GetPublisherByIdQuery(viewModel.PublisherId)) : null;
            var authors = viewModel.AuthorIds != null && viewModel.AuthorIds.Any() ? await _mediator.Send(new GetAuthorsByIdsQuery(viewModel.AuthorIds)) : null;
            var genres = viewModel.GenreIds != null && viewModel.GenreIds.Any() ? await _mediator.Send(new GetGenresByIdsQuery(viewModel.GenreIds)) : null;
            var tags = viewModel.TagIds != null && viewModel.TagIds.Any() ? await _mediator.Send(new GetTagsByIdsQuery(viewModel.TagIds)) : null;

            var book = await _mediator.Send(new EditBookCommand(viewModel.Id, viewModel.Title, viewModel.Description, viewModel.Price, viewModel.Pages, viewModel.PublishedOn, publisher, authors, genres, tags));
            return RedirectToAction(nameof(Info), new { book.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromForm] Guid id)
        {
            await _mediator.Send(new DeleteBookCommand(id));
            return RedirectToAction(nameof(Index));
        }
    }
}
