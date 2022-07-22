using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class CreateGenreHandler : IRequestHandler<CreateGenreCommand, Genre>
    {
        private readonly IRepository<Genre> _repository;

        public CreateGenreHandler(IRepository<Genre> repository)
        {
            _repository = repository;
        }

        public async Task<Genre> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
        {
            var genre = new Genre(request.Name);
            genre = await _repository.AddAsync(genre, cancellationToken);
            return genre;
        }
    }
}
