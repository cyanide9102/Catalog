using System;

#nullable disable

namespace Catalog.Core.Entities
{
    public class BookTag
    {
        public BookTag(Guid bookId, Guid tagId)
        {
            BookId = bookId;
            TagId = tagId;
        }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
