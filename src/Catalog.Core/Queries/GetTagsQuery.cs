using Catalog.Core.Entities;
using MediatR;

namespace Catalog.Core.Queries
{
    public record GetTagsQuery() : IRequest<IEnumerable<Tag>>;
}
