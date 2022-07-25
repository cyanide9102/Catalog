using Ardalis.GuardClauses;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetBookByIdHandler : IRequestHandler<GetBookByIdQuery, Book>
    {
        private readonly IReadRepository<Book> _repository;

        public GetBookByIdHandler(IReadRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<Book> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetBookSpec(request.Id);
            var book = await _repository.FirstOrDefaultAsync(specification, cancellationToken);
            Guard.Against.Null(book, nameof(book), $"Book not found with given ID: {request.Id}");

            return book;
        }
    }
}
