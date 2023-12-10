using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class PinsController : Controller
    {
        private readonly ApplicationDbContext db;

        public PinsController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var pins = from pin in db.Pins
                       orderby pin.Title
                       select pin;//Include("Tag");
            ViewBag.Pins = pins;
            
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }
            return View();
        }

        public IActionResult Show(int id)
        {
            Pin pin = db.Pins.Include("Comments").Where(p => p.Id == id).First();
            return View(pin);
        }

        [HttpPost]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = System.DateTime.Now;
            //comment.UserId = _userManager.GetUserId();
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Pins/Show/" + comment.PinId);
            }
            else
            {
                Pin pin = db.Pins.Include("Comments").Where(p => p.Id == comment.PinId).First();
                return View(pin);
            }
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllTags()
        {
            var selectList = new List<SelectListItem>();
            var tags = from t in db.Tags select t;

            foreach (var tag in tags)
            {
                selectList.Add(new SelectListItem
                {
                    Value = tag.Id.ToString(),
                    Text = tag.TagName.ToString()
                });
            }
            return selectList;
        }

        public IActionResult New()
        {
            Pin pin = new Pin();
            //pin.Tags = GetAllTags();
            return View(pin);
        }

        [HttpPost]
        public IActionResult New(Pin pin)
        {
            pin.Date = System.DateTime.Now;
            //pin.UserId = _userManager.GetUserId();
            if (ModelState.IsValid)
            {
                db.Pins.Add(pin);
                db.SaveChanges();
                TempData["message"] = "Pin-ul a fost adaugat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                //pin.Tags = GetAllTags();
                return View(pin);
            }
        }
        
        public IActionResult Edit(int id)
        {
            Pin pin = db.Pins.Where(art => art.Id == id).First();
            //pin.Tags = GetAllTags();

            return View(pin);
        }

        [HttpPost]
        public IActionResult Edit(int id, Pin requestPin)
        {
            if (ModelState.IsValid)
            {
                Pin pin = db.Pins.Find(id);
                pin.Title = requestPin.Title;
                pin.Content = requestPin.Content;
                db.SaveChanges();
                TempData["message"] = "Pin-ul a fost modificat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                //requestPin.Tags = GetAllTags();
                return View();
            }
        }

        //[HttpPost]
        public IActionResult Delete(int id)
        {
            var pin = db.Pins.Find(id);
            db.Pins.Remove(pin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
