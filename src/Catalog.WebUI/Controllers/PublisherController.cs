using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.PublisherViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class PublisherController : Controller
    {
        private readonly IMediator _mediator;

        public PublisherController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<JsonResult> GetPublisherList([FromForm] DtRequest dt, [FromServices] IPublisherQueryService publisherQueryService)
        {
            try
            {
                int recordsTotal = await publisherQueryService.GetTotalPublishersCountAsync();
                int recordsFiltered = await publisherQueryService.GetFilteredPublishersCountAsync(dt.Search.Value);
                var publishers = await publisherQueryService.GetPublishersPaginatedAsync(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir, dt.Start, dt.Length);

                var response = new DtResponse<Publisher>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = publishers,
                    Error = ""
                };
                return Json(response);
            }
            catch (Exception e)
            {
                var response = new DtResponse<Publisher>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = 0,
                    RecordsFiltered = 0,
                    Data = null,
                    Error = e.InnerException != null ? e.InnerException.Message : e.Message
                };
                return Json(response);
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
            var publishers = await _mediator.Send(new GetPublishersQuery());
            return View(publishers);
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Info(Guid id)
        {
            var publisher = await _mediator.Send(new GetPublisherByIdQuery(id));
            return View(publisher);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PublisherCreateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var publisher = await _mediator.Send(new CreatePublisherCommand(viewModel.Name, viewModel.Country));
            return RedirectToAction(nameof(Info), new { publisher.Id });
        }

        [HttpGet("[controller]/[action]/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            var publisher = await _mediator.Send(new GetPublisherByIdQuery(id));
            var viewModel = new PublisherEditViewModel()
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Country = publisher.Country
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromForm] PublisherEditViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var publisher = await _mediator.Send(new EditPublisherCommand(viewModel.Id, viewModel.Name, viewModel.Country));
            return RedirectToAction(nameof(Info), new { publisher.Id });
        }
    }
}
