using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class CreatePublisherHandler : IRequestHandler<CreatePublisherCommand, Publisher>
    {
        private readonly IRepository<Publisher> _repository;

        public CreatePublisherHandler(IRepository<Publisher> repository)
        {
            _repository = repository;
        }

        public async Task<Publisher> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
        {
            var publisher = new Publisher(request.Name, request.Country);
            publisher = await _repository.AddAsync(publisher, cancellationToken);
            return publisher;
        }
    }
}
