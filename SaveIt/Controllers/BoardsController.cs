using Microsoft.AspNetCore.Mvc;
using SaveIt.Data;

namespace SaveIt.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ApplicationDbContext db;

        public BoardsController(ApplicationDbContext context)
        {
            db = context;
        }

        /*public IActionResult Index()
        {

        }*/


    }
}
