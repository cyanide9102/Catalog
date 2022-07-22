using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebUI.Controllers
{
    public class BookController : Controller
    {
        public BookController()
        {
        }

        [HttpGet(Name = "List")]
        public IActionResult List()
        {
            return View();
        }
    }
}
