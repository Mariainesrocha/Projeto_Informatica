using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Pmat_PI.Models;
using System;
using Pmat_PI;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Pmat_PI.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContextAlmostFinal _context;
        private RoleManager<IdentityRole> roleManager;
        private UserManager<User> userManager;
        private IPasswordHasher<User> passwordHasher;
        private readonly ApplicationDbContext _identitycontext;


        public UsersController(UserManager<User> usrMgr, IPasswordHasher<User> passwordHash, ApplicationDbContextAlmostFinal context, RoleManager<IdentityRole> role, ApplicationDbContext identitycontext)

        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
            _context = context;
            roleManager = role;
            _identitycontext = identitycontext;
        }

        public async Task<IActionResult> Index(string sortOrder,string currentFilter,string searchString, string filterType, string currentType, int? pageNumber)
        {
            if (searchString != null && filterType != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
                filterType = currentType;
            }

            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentType"] = filterType;
            var users = userManager.Users;
            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "escola":
                        users = from u in _context.AspNetUsers join ue in _context.UserEscolas.Where(e => e.IdEscolaNavigation.NomeEscola.ToLower().Equals(searchString.ToLower())) on u.Id equals ue.IdUser select new User { UserName = u.UserName, Id = u.Id, Email = u.Email, Age = u.Age, Name = u.Name, PasswordHash = u.PasswordHash};
                        break;
                    case "nome":
                        users = userManager.Users.Where(u => u.Name.ToLower().StartsWith(searchString.ToLower()));
                        break;
                    case "email":
                        users = userManager.Users.Where(u => u.Email.ToLower().Contains(searchString.ToLower()));
                        break;
                    case "role":
                        users = (IQueryable<User>)await userManager.GetUsersInRoleAsync("ADMIN");//TODO: N FUNCIONA
                        break;
                    default:
                        break;
                }
            }

            int pageSize = 50;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Utilizador com Id = {id} não encontrado";
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
            if (user != null) {
                var allRoles = roleManager.Roles;
                var userRoles = userManager.GetRolesAsync(user).Result;
                ViewData["AllRoles"] = new SelectList(allRoles, "Name", "Name");
                ViewBag.Roles = new SelectList(userRoles);
                ViewBag.UserRoles = userRoles;
                return View(user);
            }
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
                    ModelState.AddModelError("", "O email não pode estar vazio.");

                if (!string.IsNullOrEmpty(username))
                {
                    user.UserName = username;
                }
                else
                    ModelState.AddModelError("", "O username não pode estar vazio.");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
            

                if (!string.IsNullOrEmpty(name))
                    user.Name = name;
                else
                    ModelState.AddModelError("", "O nome não pode estar vazio.");

                if (!string.IsNullOrEmpty(age))
                    user.Age = int.Parse(age);
                else
                    ModelState.AddModelError("", "A idade não pode estar vazia.");


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
                ModelState.AddModelError("", "Utilizador não encontrado");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}