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
            //var pins = db.Pins.Include("PinTags.Tag");
            var pins = db.Pins.Include(p => p.PinTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes);
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

            //Pin pin = db.Pins.Include("PinTags.Tag").Include("Comments").Where(p => p.Id == id).First();
            Pin pin = db.Pins.Include(p => p.PinTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes).Include(p => p.Comments).FirstOrDefault(p => p.Id == id);
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
                //Pin pin = db.Pins.Include("PinTags.Tag").Include("Comments").Where(p => p.Id == comment.PinId).First();
                Pin pin = db.Pins.Include(p => p.PinTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes).Include(p => p.Comments).FirstOrDefault(p => p.Id == comment.PinId);
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
            pin.Tags = GetAllTags();
            return View(pin);
        }

        [HttpPost]
        public IActionResult New(Pin pin)
        {
            pin.Date = DateTime.Now;
            pin.Tags = GetAllTags();
            //pin.UserId = _userManager.GetUserId();
            if (ModelState.IsValid)
            {
                db.Pins.Add(pin);
                db.SaveChanges();
                foreach (var tagId in pin.TagIds)
                {
                    PinTag pinTag = new PinTag
                    {
                        PinId = pin.Id,
                        TagId = tagId
                    };
                    db.PinTags.Add(pinTag);
                }
                db.SaveChanges();
                TempData["message"] = "Pin-ul a fost adaugat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(pin);
            }
        }
        
        public IActionResult Edit(int id)
        {
            //Pin pin = db.Pins.Include("PinTags.Tag").Where(art => art.Id == id).First();
            Pin pin = db.Pins.Include(p => p.PinTags).ThenInclude(pt => pt.Tag).FirstOrDefault(p => p.Id == id);
            pin.Tags = GetAllTags();

            return View(pin);
        }

        [HttpPost]
        public IActionResult Edit(int id, Pin requestPin)
        {
            if (ModelState.IsValid)
            {
                Pin pin = db.Pins.Include(p => p.PinTags).Where(p => p.Id == id).First();
                pin.Title = requestPin.Title;
                pin.Content = requestPin.Content;
                pin.Date = DateTime.Now;
                // trebuie pus si tag-ul
                //pin.Tags = GetAllTags();
                // vreau ca lista de TagIds sa fie copiata de la requestPin
                //pin.TagIds = requestPin.TagIds;
                //pin.TagId = requestPin.TagId;
                //pin.Tags = GetAllTags();
                //pin.PinTags = requestPin.PinTags;
                //db.Pins.Update(pin);
                //db.SaveChanges();
                pin.PinTags.Clear();
                foreach (var tagId in requestPin.TagIds)
                {
                    PinTag pinTag = new PinTag
                    {
                        PinId = pin.Id,
                        TagId = tagId
                    };
                    db.PinTags.Add(pinTag);
                }
                db.SaveChanges();
                TempData["message"] = "Pin-ul a fost modificat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                requestPin.Tags = GetAllTags();
                return View();
            }
        }
       

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pin = db.Pins.Find(id);
            db.Pins.Remove(pin);
            db.SaveChanges();
            TempData["message"] = "Pin-ul a fost sters!";
            return RedirectToAction("Index");
        }

    }
}
