using Microsoft.AspNetCore.Mvc;

namespace SaveIt.Controllers
{
    public class CommentsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
