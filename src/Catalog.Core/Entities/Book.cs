using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Book : EntityBase, IAggregateRoot
    {
        public Book(string title, string description, decimal price, short? pages = null, DateTime? publishedOn = null) : base()
        {
            Title = title;
            Description = description;
            Price = price;
            Pages = pages;
            PublishedOn = publishedOn;
            PublisherId = null;
            Publisher = null;
            AuthorLinks = new HashSet<BookAuthor>();
            GenreLinks = new HashSet<BookGenre>();
            TagLinks = new HashSet<BookTag>();
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; set; }
        public short? Pages { get; private set; }
        public DateTime? PublishedOn { get; private set; }

        public Guid? PublisherId { get; private set; }
        public Publisher? Publisher { get; private set; }

        public ICollection<BookAuthor> AuthorLinks { get; private set; }
        public ICollection<BookGenre> GenreLinks { get; private set; }
        public ICollection<BookTag> TagLinks { get; private set; }

        public void Update(string title, string description, short? pages = null, DateTime? publishedOn = null)
        {
            Title = title;
            Description = description;
            Pages = pages;
            PublishedOn = publishedOn;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePublisher(Publisher? publisher)
        {
            PublisherId = publisher?.Id;
            Publisher = publisher;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
