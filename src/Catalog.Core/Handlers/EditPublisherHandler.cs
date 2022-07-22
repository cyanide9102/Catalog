using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class EditPublisherHandler : IRequestHandler<EditPublisherCommand, Publisher>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Publisher> _repository;

        public EditPublisherHandler(IMediator mediator, IRepository<Publisher> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Publisher> Handle(EditPublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = await _mediator.Send(new GetPublisherByIdQuery(request.Id), cancellationToken);
            publisher.Update(request.Name, request.Country);
            await _repository.SaveChangesAsync(cancellationToken);
            return publisher;
        }
    }
}
