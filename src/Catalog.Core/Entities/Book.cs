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
            Authors = new HashSet<Author>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public int? Pages { get; set; }
        public DateTime? PublishedOn { get; set; }

        public Guid? PublisherId { get; set; }
        public Publisher? Publisher { get; set; }

        public ICollection<Genre> Genres { get; set; }
        public ICollection<Author> Authors { get; set; }
    }
}
