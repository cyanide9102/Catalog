using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.TagViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly IMediator _mediator;

        public TagController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<JsonResult> GetTagList([FromForm] DtRequest dt, [FromServices] ITagQueryService tagQueryService)
        {
            try
            {
                int recordsTotal = await tagQueryService.GetTotalTagsCountAsync();
                int recordsFiltered = await tagQueryService.GetFilteredTagsCountAsync(dt.Search.Value);
                var tags = await tagQueryService.GetTagsPaginatedAsync(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir, dt.Start, dt.Length);

                var result = new DtResponse<Tag>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = tags,
                    Error = ""
                };
                return Json(result);
            }
            catch (Exception e)
            {
                var result = new DtResponse<Tag>()
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
            var tags = await _mediator.Send(new GetTagsQuery());
            return View(tags);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var tag = await _mediator.Send(new GetTagByIdQuery(id));
            return View(tag);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] TagCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var tag = await _mediator.Send(new CreateTagCommand(viewModel.Name));
            return RedirectToAction(nameof(Info), new { tag.Id });
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tag = await _mediator.Send(new GetTagByIdQuery(id));
            var viewModel = new TagEditViewModel()
            {
                Id = tag.Id,
                Name = tag.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] TagEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var tag = await _mediator.Send(new EditTagCommand(viewModel.Id, viewModel.Name));
            return RedirectToAction(nameof(Info), new { tag.Id });
        }
    }
}
