using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.BookViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<JsonResult> GetBookList([FromForm] DtRequest dt, [FromServices] IBookQueryService bookQueryService)
        {
            int recordsTotal = await bookQueryService.GetTotalBooksCountAsync();
            int recordsFiltered = await bookQueryService.GetFilteredBooksCountAsync(dt.Search.Value);
            var books = await bookQueryService.GetBooksPaginatedAsync(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir, dt.Start, dt.Length);

            var result = new DtResponse<Book>()
            {
                Draw = dt.Draw,
                RecordsTotal = recordsTotal,
                RecordsFiltered = recordsFiltered,
                Data = books,
                Error = ""
            };
            return Json(result);
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
            var authors = await _mediator.Send(new GetAuthorsByIdsQuery(viewModel.AuthorIds));
            var genres = await _mediator.Send(new GetGenresByIdsQuery(viewModel.GenreIds));
            var tags = await _mediator.Send(new GetTagsByIdsQuery(viewModel.TagIds));

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
            var authors = await _mediator.Send(new GetAuthorsByIdsQuery(viewModel.AuthorIds));
            var genres = await _mediator.Send(new GetGenresByIdsQuery(viewModel.GenreIds));
            var tags = await _mediator.Send(new GetTagsByIdsQuery(viewModel.TagIds));

            var book = await _mediator.Send(new EditBookCommand(viewModel.Id, viewModel.Title, viewModel.Description, viewModel.Price, viewModel.Pages, viewModel.PublishedOn, publisher, authors, genres, tags));
            return RedirectToAction(nameof(Info), new { book.Id });
        }
    }
}
