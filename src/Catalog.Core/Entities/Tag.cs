using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Tag : EntityBase, IAggregateRoot
    {
        public Tag(string name) : base()
        {
            Name = name;
            Books = new HashSet<Book>();
        }

        public string Name { get; private set; }
        public ICollection<Book> Books { get; private set; }
    }
}
