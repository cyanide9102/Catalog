using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Genre : EntityBase, IAggregateRoot
    {
        public Genre(string name) : base()
        {
            Name = name;
            BookLinks = new HashSet<BookGenre>();
        }

        public string Name { get; private set; }

        public ICollection<BookGenre> BookLinks { get; private set; }

        public void Update(string name)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
