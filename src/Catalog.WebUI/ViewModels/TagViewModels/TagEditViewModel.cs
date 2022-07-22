using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.TagViewModels
{
    public class TagEditViewModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [StringLength(256)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
