using Microsoft.AspNetCore.Mvc;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CommentsController(ApplicationDbContext context)
        {
            db = context;
        }

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
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();
            return Redirect("/Pins/Show/" + comment.PinId);
        }

        public IActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);
            ViewBag.Comment = comment;
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, Comment requestComment)
        {
            Comment comment = db.Comments.Find(id);
            if (ModelState.IsValid)
            {
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
