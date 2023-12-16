using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class LikesController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public LikesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult AddLike(int id)
        {
            var usr = _userManager.GetUserId(User);
            var userLikes = db.Likes.Where(l => l.PinId == id && l.UserId == usr).ToList();
            if (userLikes.Count == 0)
                db.Add(new Like { PinId = id, UserId = usr});
                db.SaveChanges();
            return Redirect("/Pins/Show/" + id);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult DeleteLike(int id)
        {
            var usr = _userManager.GetUserId(User);
            var likesToDelete = db.Likes.Where(l => l.PinId == id && l.UserId == usr).ToList();

            foreach (var like in likesToDelete)
            {
                db.Likes.Remove(like);
            }

            db.SaveChanges();

            return Redirect("/Pins/Show/" + id);
        }
    }
}
