using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.BookViewModels
{
    public class BookCreateViewModel
    {
        [Required]
        [StringLength(256)]
        public string Title { get; set; }

        [Required]
        [StringLength(1024)]
        public string Description { get; set; }

        public int? Pages { get; set; }

        public DateTime? PublishedOn { get; set; }

        public Guid? PublisherId { get; set; }

        public List<Guid> GenreIds { get; set; }
        public List<Guid> TagIds { get; set; }
        public List<Guid> AuthorIds { get; set; }
    }
}
