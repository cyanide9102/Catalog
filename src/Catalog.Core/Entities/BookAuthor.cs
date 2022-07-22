using System;

#nullable disable

namespace Catalog.Core.Entities
{
    public class BookAuthor
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        public byte Order { get; set; }
    }
}
