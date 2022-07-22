using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetGenresHandler : IRequestHandler<GetGenresQuery, IEnumerable<Genre>>
    {
        private readonly IReadRepository<Genre> _repository;

        public GetGenresHandler(IReadRepository<Genre> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Genre>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
        {
            var genres = await _repository.ListAsync(cancellationToken);
            return genres;
        }
    }
}
