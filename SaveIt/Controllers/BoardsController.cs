using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    public class BoardsController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public BoardsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize(Roles="User,Admin")]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

            SetAccessRights();

            if (User.IsInRole("User"))
            {
                var boards = db.Boards.Include("User").Where(b => b.UserId == _userManager.GetUserId(User));
                ViewBag.Boards = boards;
                return View();
            }
            else
                if (User.IsInRole("Admin"))
                {
                    var boards = db.Boards.Include("User");
                    ViewBag.Boards = boards;
                    return View();
                }
                else
                {
                    TempData["message"] = "Nu aveti drept de accesare!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Pins");
                }
        }

        [Authorize(Roles="User,Admin")]
        public IActionResult Show(int id)
        {
            SetAccessRights();

            if (User.IsInRole("User"))
            {
                var boards = db.Boards.Include("User")
                                     .Include("PinBoards.Pin.User")
                                     .Include("PinBoards.Pin.Likes")
                                     .Include("PinBoards.Pin.Comments")
                                     .Include(pb => pb.PinBoards)
                                     .ThenInclude(pb => pb.Pin)
                                     .ThenInclude(pb => pb.PinTags)
                                     .ThenInclude(pt => pt.Tag)
                                     .Where(b => b.UserId == _userManager.GetUserId(User)).FirstOrDefault(b => b.Id == id);
                if (boards != null)
                {
                    return View(boards);
                }
                else
                {
                    TempData["message"] = "Nu exista board-ul!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Pins");
                }
            }
            else
                if (User.IsInRole("Admin"))
                {
                    var boards = db.Boards.Include("User")
                                         .Include("PinBoards.Pin.User")
                                         .Include("PinBoards.Pin.Likes")
                                         .Include("PinBoards.Pin.Comments")
                                         .Include(pb => pb.PinBoards)
                                         .ThenInclude(pb => pb.Pin)
                                         .ThenInclude(pb => pb.PinTags)
                                         .ThenInclude(pt => pt.Tag)
                                         .FirstOrDefault(b => b.Id == id);
                    if (boards != null)
                    {
                        return View(boards);
                    }
                    else
                    {
                        TempData["message"] = "Nu exista board-ul!";
                        TempData["messageType"] = "alert-danger";
                        return RedirectToAction("Index", "Pins");
                    }
                }
                else
                {
                    TempData["message"] = "Nu aveti drept de accesare!";
                    TempData["messageType"] = "alert-danger";
                    return RedirectToAction("Index", "Pins");
                }
        }

        [Authorize(Roles="User,Admin")]
        public IActionResult New()
        {
            return View();
        }

        [Authorize(Roles="User,Admin")]
        [HttpPost]
        public IActionResult New(Board board)
        {
            board.UserId = _userManager.GetUserId(User);
            if (ModelState.IsValid)
            {
                db.Boards.Add(board);
                db.SaveChanges();
                TempData["message"] = "Board-ul a fost adaugat!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                return View(board);
            }
        }

        [Authorize(Roles="User,Admin")]
        public IActionResult Edit(int id)
        {
            var board = db.Boards.Find(id);
            if (board.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                return View(board);
            }
            else
            {
                TempData["message"] = "Nu aveti drept de accesare!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Pins");
            }
        }

        // TODO: edit
        
        public IActionResult Delete(int id)
        {
            var board = db.Boards.Find(id);
            if (board.UserId == _userManager.GetUserId(User) || User.IsInRole("Admin"))
            {
                db.Boards.Remove(board);
                db.SaveChanges();
                TempData["message"] = "Board-ul a fost sters!";
                TempData["messageType"] = "alert-success";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Nu aveti acest drept!";
                TempData["messageType"] = "alert-danger";
                return RedirectToAction("Index", "Pins");
            }
        }


        private void SetAccessRights()
        {
            ViewBag.EsteAdmin = User.IsInRole("Admin");
            ViewBag.UserCurent = _userManager.GetUserId(User);
        }
    }
}
