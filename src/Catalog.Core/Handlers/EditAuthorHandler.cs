using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class EditAuthorHandler : IRequestHandler<EditAuthorCommand, Author>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Author> _repository;

        public EditAuthorHandler(IMediator mediator, IRepository<Author> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Author> Handle(EditAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _mediator.Send(new GetAuthorByIdQuery(request.Id), cancellationToken);
            author.Update(request.Name);
            await _repository.SaveChangesAsync(cancellationToken);
            return author;
        }
    }
}
