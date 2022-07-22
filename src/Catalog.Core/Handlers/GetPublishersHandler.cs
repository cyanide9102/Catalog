using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetPublishersHandler : IRequestHandler<GetPublishersQuery, IEnumerable<Publisher>>
    {
        private readonly IReadRepository<Publisher> _repository;

        public GetPublishersHandler(IReadRepository<Publisher> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Publisher>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
        {
            var publishers = await _repository.ListAsync(cancellationToken);
            return publishers;
        }
    }
}
