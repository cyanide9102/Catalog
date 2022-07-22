using Catalog.Core.Commands;
using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using MediatR;

namespace Catalog.Core.Handlers
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, Tag>
    {
        private readonly IRepository<Tag> _repository;

        public CreateTagHandler(IRepository<Tag> repository)
        {
            _repository = repository;
        }

        public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = new Tag(request.Name);
            tag = await _repository.AddAsync(tag, cancellationToken);
            return tag;
        }
    }
}
