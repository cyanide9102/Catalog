using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class DeleteBookHandler : IRequestHandler<DeleteBookCommand>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Book> _repository;

        public DeleteBookHandler(IMediator mediator, IRepository<Book> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var book = await _mediator.Send(new GetBookByIdQuery(request.Id), cancellationToken);

            await _repository.DeleteAsync(book, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
