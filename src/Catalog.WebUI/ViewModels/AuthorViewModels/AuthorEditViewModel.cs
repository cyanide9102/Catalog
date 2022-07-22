using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.AuthorViewModels
{
    public class AuthorEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Author's Name")]
        public string Name { get; set; }
    }
}
