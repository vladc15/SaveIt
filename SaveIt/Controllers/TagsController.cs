using Microsoft.AspNetCore.Mvc;

namespace SaveIt.Controllers
{
    public class TagsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
