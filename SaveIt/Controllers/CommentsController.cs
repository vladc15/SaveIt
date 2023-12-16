using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public CommentsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /*
        [HttpPost]
        public IActionResult New(Comment comment)
        {
            comment.Date = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Pins/Show/" + comment.PinId);
            }
            else
            {
                return Redirect("/Pins/Show/" + comment.PinId);
            }
        }*/

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                TempData["message"] = "Nu aveti dreptul sa stergeti acest comentariu!";
                TempData["messageType"] = "alert-danger";
                return Redirect("/Pins/Show/" + comment.PinId);
            }
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Redirect("/Pins/Show/" + comment.PinId);
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            if (comment.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                TempData["message"] = "Nu aveti dreptul sa editati acest comentariu!";
                TempData["messageType"] = "alert-danger";
                return Redirect("/Pins/Show/" + comment.PinId);
            }
            ViewBag.Comment = comment;
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Comment requestComment)
        {
            Comment comment = db.Comments.Find(id);
            if (ModelState.IsValid)
            {
                if (comment.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
                {
                    TempData["message"] = "Nu aveti dreptul sa editati acest comentariu!";
                    TempData["messageType"] = "alert-danger";
                    return Redirect("/Pins/Show/" + comment.PinId);
                }
                comment.Content = requestComment.Content;
                db.SaveChanges();
                return Redirect("/Pins/Show/" + comment.PinId);
            }
            else
            {
                return Redirect("/Pins/Show/" + comment.PinId);
            }
        }

    }
}
