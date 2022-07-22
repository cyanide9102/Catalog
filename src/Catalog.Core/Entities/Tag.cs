using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Tag : EntityBase, IAggregateRoot
    {
        public Tag(string name) : base()
        {
            Name = name;
            BookLinks = new HashSet<BookTag>();
        }

        public string Name { get; private set; }

        public ICollection<BookTag> BookLinks { get; private set; }
        public void Update(string name)
        {
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
