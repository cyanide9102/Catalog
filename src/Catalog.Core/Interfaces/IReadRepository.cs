using Ardalis.Specification;

namespace Catalog.Core.Interfaces
{
    public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
    {
        IQueryable<T> Get();
    }
}
