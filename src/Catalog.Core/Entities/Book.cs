using Catalog.Core.Interfaces;

namespace Catalog.Core.Entities
{
    public class Book : EntityBase, IAggregateRoot
    {
        public Book(string title, string description, int? pages = null, DateTime? publishedOn = null, Guid? publisherId = null) : base()
        {
            Title = title;
            Description = description;
            Pages = pages;
            PublishedOn = publishedOn;
            PublisherId = publisherId;
            Genres = new HashSet<Genre>();
            Tags = new HashSet<Tag>();
            Authors = new HashSet<Author>();
        }

        public string Title { get; private set; }
        public string Description { get; private set; }
        public int? Pages { get; private set; }
        public DateTime? PublishedOn { get; private set; }

        public Guid? PublisherId { get; private set; }
        public Publisher? Publisher { get; private set; }

        public ICollection<Genre> Genres { get; private set; }
        public ICollection<Tag> Tags { get; private set; }
        public ICollection<Author> Authors { get; private set; }
    }
}
