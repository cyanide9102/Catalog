using Ardalis.GuardClauses;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Queries;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class GetTagByIdHandler : IRequestHandler<GetTagByIdQuery, Tag>
    {
        private readonly IReadRepository<Tag> _repository;

        public GetTagByIdHandler(IReadRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task<Tag> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var specification = new GetTagSpec(request.Id);
            var tag = await _repository.FirstOrDefaultAsync(specification, cancellationToken);
            Guard.Against.Null(tag, nameof(tag), $"Publisher not found with given ID: {request.Id}");

            return tag;
        }
    }
}
