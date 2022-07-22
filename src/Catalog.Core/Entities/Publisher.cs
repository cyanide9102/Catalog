using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Publisher : EntityBase, IAggregateRoot
    {
        public Publisher(string name, string? country = null) : base()
        {
            Name = name;
            Country = country;
            Books = new HashSet<Book>();
        }

        public string Name { get; private set; }
        public string? Country { get; private set; }

        public ICollection<Book> Books { get; private set; }
    }
}
