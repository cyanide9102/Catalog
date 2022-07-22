using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetAuthorsHandler : IRequestHandler<GetAuthorsQuery, IEnumerable<Author>>
    {
        private readonly IReadRepository<Author> _repository;

        public GetAuthorsHandler(IReadRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Author>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _repository.ListAsync(cancellationToken);
            return authors;
        }
    }
}
