using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Pmat_PI.Models;
using System;
using Pmat_PI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Pmat_PI.Data;
using System.Linq;

namespace Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private UserManager<User> userManager;
        private IPasswordHasher<User> passwordHasher;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<User> usrMgr, IPasswordHasher<User> passwordHash, ApplicationDbContext context)

        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            _context = context;
           
        }

        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString,int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["CurrentFilter"] = searchString;

            var users = from u in _context.Users select u;

            if (!String.IsNullOrEmpty(searchString))
            {
                pageNumber = 1;
                users = users.Where(u => u.Name.Contains(searchString));
            }
            else
            {
                searchString = currentFilter;
            }

           
            var result = from u in _context.Users select u; 

            int pageSize = 50;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(userManager.Users);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await userManager.DeleteAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View("Index");
            }
        }

        public async Task<IActionResult> UpdateUser(string id)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
                return View(user);
            else
                return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(string id, string email,string username, string password, string name, string age)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user.Email = email;
                }
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(username))
                {
                    user.UserName = username;
                }
                else
                    ModelState.AddModelError("", "Username cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
            

                if (!string.IsNullOrEmpty(name))
                    user.Name = name;
                else
                    ModelState.AddModelError("", "Name cannot be empty");

                if (!string.IsNullOrEmpty(age))
                    user.Age = int.Parse(age);
                else
                    ModelState.AddModelError("", "Age cannot be empty");


                if (!string.IsNullOrEmpty(email))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("Index");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
        // removed for brevity
    }
}