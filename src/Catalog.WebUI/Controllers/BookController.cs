using Catalog.Core.Commands;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
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
        public async Task<JsonResult> GetBookList([FromServices] IBookQueryService bookQueryService)
        {
            var draw = Request.Form["draw"].FirstOrDefault();
            var searchTerm = Request.Form["search[value]"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "-1");

            var data = await bookQueryService.GetBookList(Convert.ToInt32(draw), searchTerm, sortColumn, sortColumnDirection, skip, pageSize);
            return Json(data);
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
            var viewModel = new BookEditViewModel()
            {
                Id = id,
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

            // TODO: Implement
            //var author = await _mediator.Send(new EditAuthorCommand(viewModel.Id, viewModel.Name));
            //return RedirectToAction(nameof(Info), new { author.Id });
            return View(viewModel);
        }
    }
}
