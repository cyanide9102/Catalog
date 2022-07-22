using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetBooksHandler : IRequestHandler<GetBooksQuery, IEnumerable<Book>>
    {
        private readonly IReadRepository<Book> _repository;

        public GetBooksHandler(IReadRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Book>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            var books = await _repository.ListAsync(cancellationToken);
            return books;
        }
    }
}
