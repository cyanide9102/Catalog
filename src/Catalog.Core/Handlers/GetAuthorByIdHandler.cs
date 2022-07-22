using Ardalis.GuardClauses;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdQuery, Author>
    {
        private readonly IReadRepository<Author> _repository;

        public GetAuthorByIdHandler(IReadRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Author> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetAuthorSpec(request.Id);
            var author = await _repository.FirstOrDefaultAsync(specification, cancellationToken);
            Guard.Against.Null(author, nameof(author), $"Author not found with given ID: {request.Id}");

            return author;
        }
    }
}
