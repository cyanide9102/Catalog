using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetTagsByIdsHandler : IRequestHandler<GetTagsByIdsQuery, IEnumerable<Tag>>
    {
        private readonly IReadRepository<Tag> _repository;

        public GetTagsByIdsHandler(IReadRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Tag>> Handle(GetTagsByIdsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetListByIdsSpec<Tag>(request.Ids);
            var tags = await _repository.ListAsync(specification, cancellationToken);
            return tags;
        }
    }
}
