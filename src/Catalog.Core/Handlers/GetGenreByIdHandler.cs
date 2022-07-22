using Ardalis.GuardClauses;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetGenreByIdHandler : IRequestHandler<GetGenreByIdQuery, Genre>
    {
        private readonly IReadRepository<Genre> _repository;

        public GetGenreByIdHandler(IReadRepository<Genre> repository)
        {
            _repository = repository;
        }

        public async Task<Genre> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetGenreSpec(request.Id);
            var genre = await _repository.FirstOrDefaultAsync(specification, cancellationToken);
            Guard.Against.Null(genre, nameof(genre), $"Publisher not found with given ID: {request.Id}");

            return genre;
        }
    }
}
