using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class CreateAuthorHandler : IRequestHandler<CreateAuthorCommand, Author>
    {
        private readonly IRepository<Author> _repository;

        public CreateAuthorHandler(IRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<Author> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author(request.Name);
            author = await _repository.AddAsync(author, cancellationToken);
            return author;
        }
    }
}
