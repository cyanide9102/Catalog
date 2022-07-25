using Catalog.Core.Entities;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.BookViewModels
{
    public class BookEditViewModel
    {
        private const string STRING_LENGHT_ERROR_MESSAGE = "{0} must have characters between {2} and {1}.";

        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 8, ErrorMessage = STRING_LENGHT_ERROR_MESSAGE)]
        public string Title { get; set; }

        [Required]
        [StringLength(1024, MinimumLength = 16, ErrorMessage = STRING_LENGHT_ERROR_MESSAGE)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public short? Pages { get; set; }

        public DateTime? PublishedOn { get; set; }

        public Guid PublisherId { get; set; }
        public List<Guid> AuthorIds { get; set; }
        public List<Guid> GenreIds { get; set; }
        public List<Guid> TagIds { get; set; }

        public IEnumerable<Author> Authors { get; set; }
        public IEnumerable<Publisher> Publishers { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}
