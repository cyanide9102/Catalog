using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class EditGenreHandler : IRequestHandler<EditGenreCommand, Genre>
    {
        private readonly IMediator _mediator;
        private readonly IRepository<Genre> _repository;

        public EditGenreHandler(IMediator mediator, IRepository<Genre> repository)
        {
            _mediator = mediator;
            _repository = repository;
        }

        public async Task<Genre> Handle(EditGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = await _mediator.Send(new GetGenreByIdQuery(request.Id), cancellationToken);
            genre.Update(request.Name);
            await _repository.SaveChangesAsync(cancellationToken);
            return genre;
        }
    }
}
