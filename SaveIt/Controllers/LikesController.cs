using Microsoft.AspNetCore.Mvc;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext db;


        public LikesController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult New(Like like)
        {
            if (ModelState.IsValid)
            {
                db.Likes.Add(like);
                db.SaveChanges();
                return Redirect("/Pins/Show/" + like.PinId);
            }
            else
            {
                return Redirect("/Pins/Show/" + like.PinId);
            }
        }
    }
}
