using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.GenreViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class GenreController : Controller
    {
        private readonly IMediator _mediator;

        public GenreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<JsonResult> GetGenreList([FromForm] DtRequest dt, [FromServices] IGenreQueryService genreQueryService)
        {
            try
            {
                int recordsTotal = await genreQueryService.GetTotalGenresCountAsync();
                int recordsFiltered = await genreQueryService.GetFilteredGenresCountAsync(dt.Search.Value);
                var genres = await genreQueryService.GetGenresPaginatedAsync(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir, dt.Start, dt.Length);

                var result = new DtResponse<Genre>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = genres,
                    Error = ""
                };
                return Json(result);
            }
            catch (Exception e)
            {
                var result = new DtResponse<Genre>()
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
            var genres = await _mediator.Send(new GetGenresQuery());
            return View(genres);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var genre = await _mediator.Send(new GetGenreByIdQuery(id));
            return View(genre);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] GenreCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var genre = await _mediator.Send(new CreateGenreCommand(viewModel.Name));
            return RedirectToAction(nameof(Info), new { genre.Id });
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var genre = await _mediator.Send(new GetGenreByIdQuery(id));
            var viewModel = new GenreEditViewModel()
            {
                Id = genre.Id,
                Name = genre.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] GenreEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var genre = await _mediator.Send(new EditGenreCommand(viewModel.Id, viewModel.Name));
            return RedirectToAction(nameof(Info), new { genre.Id });
        }
    }
}
