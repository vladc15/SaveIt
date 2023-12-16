using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public PinsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        //[Authorize(Roles = "User,Admin")]
        public IActionResult Index()
        {
            //var pins = db.Pins.Include("PinTags.Tag");
            var pins = db.Pins.Include("User").Include(p => p.PinTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes);
            ViewBag.Pins = pins;
            
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }
            return View();
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Show(int id)
        {

            //Pin pin = db.Pins.Include("PinTags.Tag").Include("Comments").Where(p => p.Id == id).First();
            Pin pin = db.Pins.Include("User").Include(p => p.PinTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes).Include(p => p.Comments).Include("Comments.User").FirstOrDefault(p => p.Id == id);
            var usr = _userManager.GetUserId(User);
            var likes = db.Likes.Where(l => l.PinId == id && usr == l.UserId).ToList();
            if (likes.Count > 0)
            {
                ViewBag.Liked = true;
            }
            else
            {
                ViewBag.Liked = false;
            }
            SetAccessRights();
            return View(pin);
        }

        private void SetAccessRights()
        {
            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.UserCurent = _userManager.GetUserId(User);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Show([FromForm] Comment comment)
        {
            comment.Date = System.DateTime.Now;
            comment.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                db.Comments.Add(comment);
                db.SaveChanges();
                return Redirect("/Pins/Show/" + comment.PinId);
            }
            else
            {
                //Pin pin = db.Pins.Include("PinTags.Tag").Include("Comments").Where(p => p.Id == comment.PinId).First();
                Pin pin = db.Pins.Include(p => p.PinTags).ThenInclude(pt => pt.Tag).Include(p => p.Likes).Include(p => p.Comments).Include("Comments.User").FirstOrDefault(p => p.Id == comment.PinId);
                SetAccessRights();
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

        [Authorize(Roles = "User,Admin")]
        public IActionResult New()
        {
            Pin pin = new Pin();
            pin.Tags = GetAllTags();
            return View(pin);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult New(Pin pin)
        {
            pin.Date = DateTime.Now;
            pin.UserId = _userManager.GetUserId(User);
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
                pin.Tags = GetAllTags();
                return View(pin);
            }
        }

        [Authorize(Roles = "User,Admin")]
        public IActionResult Edit(int id)
        {
            //Pin pin = db.Pins.Include("PinTags.Tag").Where(art => art.Id == id).First();
            Pin pin = db.Pins.Include("User").Include(p => p.PinTags).ThenInclude(pt => pt.Tag).FirstOrDefault(p => p.Id == id);
            pin.Tags = GetAllTags();

            if (pin.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(pin);
            }
            else
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }

            return View(pin);
        }

        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Edit(int id, Pin requestPin)
        {
            if (ModelState.IsValid)
            {
                Pin pin = db.Pins.Include(p => p.PinTags).Where(p => p.Id == id).First();
                
                if (pin.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
                {
                    TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index");
                }

                pin.Title = requestPin.Title;
                pin.Content = requestPin.Content;
                pin.Date = DateTime.Now;
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
        [Authorize(Roles = "User,Admin")]
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pin = db.Pins.Include("Comments").Where(p => p.Id == id).First();
            if (pin.UserId != _userManager.GetUserId(User) && !User.IsInRole("Admin"))
            {
                TempData["message"] = "Nu aveti drepturi pentru aceasta actiune!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index");
            }
            db.Pins.Remove(pin);
            db.SaveChanges();
            TempData["message"] = "Pin-ul a fost sters!";
            return RedirectToAction("Index");
        }

    }
}
