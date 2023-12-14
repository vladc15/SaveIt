using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext db;

        public TagsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"].ToString();
                ViewBag.Alert = TempData["messageType"].ToString();
            }
            var tags = from tag in db.Tags
                       orderby tag.TagName
                       select tag;
            ViewBag.Tags = tags;
            return View();
        }

        public IActionResult Show(int id)
        {
            Tag tag = db.Tags.Find(id);
            return View(tag);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Tag tag)
        {
            if (ModelState.IsValid)
            {
                db.Tags.Add(tag);
                db.SaveChanges();
                TempData["message"] = "Tag-ul a fost adaugat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(tag);
            }
        }

        public IActionResult Edit(int id)
        {
            Tag tag = db.Tags.Find(id);
            return View(tag);
        }

        [HttpPost]
        public IActionResult Edit(int id, Tag requestTag)
        {
            if (ModelState.IsValid)
            {                
                Tag tag = db.Tags.Find(id);
                tag.TagName = requestTag.TagName;
                db.SaveChanges();
                TempData["message"] = "Tagul a fost modificat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(requestTag);
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            Tag tag = db.Tags.Find(id);
            db.Tags.Remove(tag);
            
            db.SaveChanges();
            TempData["message"] = "Tag-ul a fost sters!";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");
        }
    }
}
