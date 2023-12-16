using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TagsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public TagsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
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
            Tag tag = db.Tags.Include(t => t.PinTags).ThenInclude(pt => pt.Pin).ThenInclude(p => p.Comments).FirstOrDefault(t => t.Id == id);

            foreach (var pinTag in tag.PinTags)
            {
                foreach (var comment in pinTag.Pin.Comments)
                {
                    db.Comments.Remove(comment);
                }
            }

            // vreau sa retin pin-urile care apar in tag.PinTags
            var pins = tag.PinTags.Select(pt => pt.Pin).ToList();

            // sterg pintag-urile care au tag-ul curent
            foreach (var pinTag in tag.PinTags)
            {
                db.PinTags.Remove(pinTag);
            }

            // sterg pin-urile care au fost retinute
            foreach (var pin in pins)
            {
                db.Pins.Remove(pin);
            }

            db.Tags.Remove(tag);

            db.SaveChanges();
            TempData["message"] = "Tag-ul a fost sters!";
            TempData["messageType"] = "alert-danger";
            return RedirectToAction("Index");
        }
    }
}
