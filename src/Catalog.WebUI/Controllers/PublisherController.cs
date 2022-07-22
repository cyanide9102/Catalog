using Catalog.Core.Commands;
using Catalog.Core.Queries;
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
