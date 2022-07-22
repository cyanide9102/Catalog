using Ardalis.GuardClauses;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetPublisherByIdHandler : IRequestHandler<GetPublisherByIdQuery, Publisher>
    {
        private readonly IReadRepository<Publisher> _repository;

        public GetPublisherByIdHandler(IReadRepository<Publisher> repository)
        {
            _repository = repository;
        }

        public async Task<Publisher> Handle(GetPublisherByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetPublisherSpec(request.Id);
            var publisher = await _repository.FirstOrDefaultAsync(specification, cancellationToken);
            Guard.Against.Null(publisher, nameof(publisher), $"Publisher not found with given ID: {request.Id}");

            return publisher;
        }
    }
}
