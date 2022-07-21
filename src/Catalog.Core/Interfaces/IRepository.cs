using Ardalis.Specification;

namespace Catalog.Core.Interfaces
{
    public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
    {

    }
}
