using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Extensions;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.WebUI.ViewModels;
using Catalog.WebUI.ViewModels.TagViewModels;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Catalog.WebUI.Controllers
{
    [Authorize]
    public class TagController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IReadRepository<Tag> _tagRepository;

        public TagController(IMediator mediator, IReadRepository<Tag> tagRepository)
        {
            _mediator = mediator;
            _tagRepository = tagRepository;
        }

        [HttpPost]
        public IActionResult GetTagList([FromForm] DtRequest dt)
        {
            try
            {
                var query = _tagRepository.Get();
                int recordsTotal = query.Count();

                if (!string.IsNullOrEmpty(dt.Search.Value))
                {
                    query = query.Where(dt.Search.Value, Core.Constants.StringCondition.Contains);
                }


                int recordsFiltered = query.Count();

                if (dt.Order.Length > 0)
                {
                    query = query.OrderBy(dt.Columns[dt.Order[0].Column].Name, dt.Order[0].Dir == "asc");

                }

                query = query.Skip(dt.Start).Take(dt.Length);
                query = query.Include(b => b.BookLinks)
                             .ThenInclude(l => l.Book);

                var tags = query.ToList();

                var result = new DtResponse<Tag>()
                {
                    Draw = dt.Draw,
                    RecordsTotal = recordsTotal,
                    RecordsFiltered = recordsFiltered,
                    Data = tags,
                    Error = ""
                };
                return Ok(result);
            }
            catch (Exception e)
            {
                return Problem(e.Message);
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
