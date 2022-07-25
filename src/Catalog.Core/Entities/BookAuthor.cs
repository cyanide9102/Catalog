using System;

#nullable disable

namespace Catalog.Core.Entities
{
    public class BookAuthor
    {
        public BookAuthor(Guid bookId, Guid authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
            Order = 0;
        }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public byte Order { get; set; }
    }
}
