using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetGenresByIdsHandler : IRequestHandler<GetGenresByIdsQuery, IEnumerable<Genre>>
    {
        private readonly IReadRepository<Genre> _repository;

        public GetGenresByIdsHandler(IReadRepository<Genre> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Genre>> Handle(GetGenresByIdsQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetListByIdsSpec<Genre>(request.Ids);
            var genres = await _repository.ListAsync(specification, cancellationToken);
            return genres;
        }
    }
}
