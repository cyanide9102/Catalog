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

        public string Name { get; set; }
        public string? Country { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
