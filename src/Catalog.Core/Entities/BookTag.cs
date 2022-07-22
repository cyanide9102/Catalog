using System;

#nullable disable

namespace Catalog.Core.Entities
{
    public class BookTag
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
