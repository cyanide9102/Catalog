using System;

#nullable disable

namespace Catalog.Core.Entities
{
    public class BookGenre
    {
        public BookGenre(Guid bookId, Guid genreId)
        {
            BookId = bookId;
            GenreId = genreId;
        }

        public Guid BookId { get; set; }
        public Book Book { get; set; }
        public Guid GenreId { get; set; }
        public Genre Genre { get; set; }
    }
}
