using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaveIt.Data;
using SaveIt.Models;

namespace SaveIt.Controllers
{
    
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext db;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            db = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            if (TempData.ContainsKey("message"))
            {
                ViewBag.Message = TempData["message"];
                ViewBag.Alert = TempData["messageType"];
            }

            var users = db.Users;
            ViewBag.UsersList = users;
            return View();
        }
        [Authorize(Roles = "User,Admin")]
        public async Task<ActionResult> Show(string id)
        {
            /*ApplicationUser user = db.Users
                                     .Include(u => u.Boards)
                                        .ThenInclude(b => b.PinBoards)
                                        .ThenInclude(pb => pb.Pin)
                                        .ThenInclude(p => p.PinTags)
                                        .ThenInclude(pt => pt.Tag)
                                     .Include(u => u.Boards)
                                        .ThenInclude(b => b.PinBoards)
                                        .ThenInclude(pb => pb.Pin)
                                        .ThenInclude(p => p.Comments)
                                      .Include(u => u.Boards)
                                        .ThenInclude(b => b.PinBoards)
                                        .ThenInclude(pb => pb.Pin)
                                        .ThenInclude(p => p.Likes)
                                     .Where(u => u.Id == id).FirstOrDefault();*/
            ApplicationUser user = db.Users.Include(u => u.Boards).Where(u => u.Id == id).FirstOrDefault();
            // vreau toate pinurile din toate boardurile userului
            ViewBag.SavedPins = db.Pins
                                .Include("Likes")
                                .Include("Comments")
                                .Include(p => p.PinTags)
                                    .ThenInclude(pt => pt.Tag)
                                .Include(p => p.PinBoards)
                                    .ThenInclude(pb => pb.Board)
                                  .Where(p => p.PinBoards.Any(pb => pb.Board.UserId == id))
                                    .Distinct().ToList();

            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.Roles = roles;
            return View(user);
        }
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();

            var roleNames = await _userManager.GetRolesAsync(user);
            var currentUserRole = _roleManager.Roles.Where(r => roleNames.Contains(r.Name)).Select(r => r.Id).FirstOrDefault();

            ViewBag.UserRole = currentUserRole;

            return View(user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id, ApplicationUser newData, [FromForm] string newRole)
        {
            ApplicationUser user = db.Users.Find(id);
            user.AllRoles = GetAllRoles();
            if (ModelState.IsValid)
            {
                user.UserName = newData.UserName;
                user.Email = newData.Email;
                user.FirstName = newData.FirstName;
                user.LastName = newData.LastName;

                var roles = db.Roles.ToList();
                foreach (var role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                var roleName = await _roleManager.FindByIdAsync(newRole);
                await _userManager.AddToRoleAsync(user, roleName.ToString());
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string id)
        {
            var user = db.Users.Include("Pins").Include("Boards").Include("Comments").FirstOrDefault(u => u.Id == id);
            
            if (user.Comments.Count() > 0)
            {
                foreach (var comment in user.Comments)
                {
                    db.Comments.Remove(comment);
                }
            }

            if (user.Boards.Count() > 0)
            {
                foreach (var board in user.Boards)
                {
                    db.Boards.Remove(board);
                }
            }

            if (user.Pins.Count() > 0)
            {
                foreach (var pin in user.Pins)
                {
                    db.Pins.Remove(pin);
                }
            }
            
            db.ApplicationUsers.Remove(user);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllRoles()
        {
            var selectList = new List<SelectListItem>();
            var roles = db.Roles;
            foreach (var role in roles)
            {
                selectList.Add(new SelectListItem
                {
                    Value = role.Id.ToString(),
                    Text = role.Name.ToString()
                });
            }
            return selectList;
        }

    }
}
