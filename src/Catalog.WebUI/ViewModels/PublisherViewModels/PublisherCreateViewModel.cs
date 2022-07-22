using System.ComponentModel.DataAnnotations;

#nullable disable

namespace Catalog.WebUI.ViewModels.PublisherViewModels
{
    public class PublisherCreateViewModel
    {
        [Required]
        [StringLength(256)]
        [Display(Name = "Publisher's Name")]
        public string Name { get; set; }

        [StringLength(128)]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}
