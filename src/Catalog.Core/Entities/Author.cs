using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Author : EntityBase, IAggregateRoot
    {
        public Author(string name) : base()
        {
            Name = name;
            BookLinks = new HashSet<BookAuthor>();
        }

        public string Name { get; private set; }

        public ICollection<BookAuthor> BookLinks { get; private set; }

        public void Update(string name)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
