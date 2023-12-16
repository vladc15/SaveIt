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
        [HttpPost]
        public IActionResult AddLike(int id)
        {
            db.Add(new Like { PinId = id });
            db.SaveChanges();
            return Redirect("/Pins/Show/" + id);
        }

        [HttpPost]
        public IActionResult DeleteLike(int id)
        {
            var likesToDelete = db.Likes.Where(l => l.PinId == id).ToList();

            foreach (var like in likesToDelete)
            {
                db.Likes.Remove(like);
            }

            db.SaveChanges();

            return Redirect("/Pins/Show/" + id);
        }
    }
}
