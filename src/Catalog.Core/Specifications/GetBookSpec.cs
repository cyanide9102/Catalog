using Ardalis.Specification;
using Catalog.Core.Entities;

namespace Catalog.Core.Specifications
{
    public class GetBookSpec : Specification<Book>, ISingleResultSpecification
    {
        public GetBookSpec(Guid id)
        {
            Query.Where(b => b.Id == id)
                 .Include(b => b.Publisher)
                 .Include(b => b.AuthorLinks)
                 .ThenInclude(l => l.Author)
                 .Include(b => b.GenreLinks)
                 .ThenInclude(l => l.Genre)
                 .Include(b => b.TagLinks)
                 .ThenInclude(l => l.Tag);
        }

        public GetBookSpec(string title)
        {
            Query.Where(b => b.Title == title)
                 .Include(b => b.Publisher)
                 .Include(b => b.AuthorLinks)
                 .ThenInclude(l => l.Author)
                 .Include(b => b.GenreLinks)
                 .ThenInclude(l => l.Genre)
                 .Include(b => b.TagLinks)
                 .ThenInclude(l => l.Tag);
        }
    }
}
