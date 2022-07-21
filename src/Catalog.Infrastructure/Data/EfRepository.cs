using Ardalis.Specification.EntityFrameworkCore;
using Catalog.Core.Interfaces;

namespace Catalog.Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T>, IReadRepository<T> where T : class, IAggregateRoot
    {
        public EfRepository(AppDbContext context) : base(context)
        {
        }
    }
}
