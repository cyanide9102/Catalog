using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.AuthorViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IMediator _mediator;

        public AuthorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<JsonResult> GetAuthorList([FromForm] DtRequest dt, [FromServices] IAuthorQueryService authorQueryService)
        {
            try
            {
                int recordsTotal = await authorQueryService.GetTotalAuthorsCountAsync();
                int recordsFiltered = await authorQueryService.GetFilteredAuthorsCountAsync(dt.Search.Value);
                var authors = await authorQueryService.GetAuthorsPaginatedAsync(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir, dt.Start, dt.Length);

                var result = new DtResponse<Author>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = authors,
                    Error = ""
                };
                return Json(result);
            }
            catch (Exception e)
            {
                var result = new DtResponse<Author>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Data = null,
                    Error = e.InnerException != null ? e.InnerException.Message : e.Message
                };
                return Json(result);
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
            var authors = await _mediator.Send(new GetAuthorsQuery());
            return View(authors);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            return View(author);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] AuthorCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = await _mediator.Send(new CreateAuthorCommand(viewModel.Name));
            return RedirectToAction(nameof(Info), new { author.Id });
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(id));
            var viewModel = new AuthorEditViewModel()
            {
                Id = author.Id,
                Name = author.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] AuthorEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var author = await _mediator.Send(new EditAuthorCommand(viewModel.Id, viewModel.Name));
            return RedirectToAction(nameof(Info), new { author.Id });
        }
    }
}
