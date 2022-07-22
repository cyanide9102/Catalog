using Catalog.Core.Commands;
using Catalog.Core.Queries;
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
