using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetAuthorsByIdsHandler : IRequestHandler<GetAuthorsByIdsQuery, IEnumerable<Author>>
    {
        private readonly IReadRepository<Author> _repository;

        public GetAuthorsByIdsHandler(IReadRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Author>> Handle(GetAuthorsByIdsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetListByIdsSpec<Author>(request.Ids);
            var authors = await _repository.ListAsync(specification, cancellationToken);
            return authors;
        }
    }
}
