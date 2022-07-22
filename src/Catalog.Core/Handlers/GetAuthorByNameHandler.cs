using Ardalis.GuardClauses;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetAuthorByNameHandler : IRequestHandler<GetAuthorByNameQuery, Author?>
    {
        private readonly IReadRepository<Author> _repository;

        public GetAuthorByNameHandler(IReadRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Author?> Handle(GetAuthorByNameQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAuthorSpec(request.Name);
            var author = await _repository.FirstOrDefaultAsync(specification, cancellationToken);
            Guard.Against.Null(author, nameof(author), $"Author not found with given name: {request.Name}");

            return author;
        }
    }
}
