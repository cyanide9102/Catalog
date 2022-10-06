using Ardalis.Specification.EntityFrameworkCore;
using Catalog.Core.Interfaces;

namespace Catalog.Infrastructure.Data
{
    public class EfRepository<T> : RepositoryBase<T>, IRepository<T>, IReadRepository<T> where T : class, IAggregateRoot
    {
        private readonly AppDbContext _context;
        public EfRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public IQueryable<T> Get()
        {
            return _context.Set<T>();
        }
    }
}
