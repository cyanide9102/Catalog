using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetTagsHandler : IRequestHandler<GetTagsQuery, IEnumerable<Tag>>
    {
        private readonly IReadRepository<Tag> _repository;

        public GetTagsHandler(IReadRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tag>> Handle(GetTagsQuery request, CancellationToken cancellationToken)
        {
            var tags = await _repository.ListAsync(cancellationToken);
            return tags;
        }
    }
}
