using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class EditTagHandler : IRequestHandler<EditTagCommand, Tag>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Tag> _repository;

        public EditTagHandler(IMediator mediator, IRepository<Tag> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Tag> Handle(EditTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _mediator.Send(new GetTagByIdQuery(request.Id), cancellationToken);
            tag.Update(request.Name);
            await _repository.SaveChangesAsync(cancellationToken);
            return tag;
        }
    }
}
