using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Genre : EntityBase, IAggregateRoot
    {
        public Genre(string name) : base()
        {
            Name = name;
            Books = new List<Book>();
        }

        public string Name { get; private set; }

        public ICollection<Book> Books { get; private set; }
    }
}
