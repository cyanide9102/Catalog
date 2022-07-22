using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.GenreViewModels
{
    public class GenreCreateViewModel
    {
        [Required]
        [StringLength(256)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }
}
