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

        public async Task<IActionResult> Index(string searchString, string filterType, string escola, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            ViewData["CurrentType"] = filterType;
            ViewData["EscolaFilter"] = escola;
            var users = userManager.Users;

            if (!String.IsNullOrEmpty(escola))
            {
                users = from u in _context.AspNetUsers
                        join ue in _context.UserEscolas
                        .Where(e => e.IdEscola.ToString().Equals(escola) || e.IdEscolaNavigation.NomeEscola.ToLower().Contains(escola.ToLower()))
                        on u.Id equals ue.IdUser
                        select new User { UserName = u.UserName, Id = u.Id, Email = u.Email, Age = u.Age, Name = u.Name, PasswordHash = u.PasswordHash };
            }

            if (!String.IsNullOrEmpty(searchString) && !String.IsNullOrEmpty(filterType))
            {
                switch (filterType)
                {
                    case "nome":
                        users = users.Where(u => u.Name.ToLower().StartsWith(searchString.ToLower()));
                        break;
                    case "email":
                        users = users.Where(u => u.Email.ToLower().Contains(searchString.ToLower()));
                        break;
                    case "id":
                        users = users.Where(u => u.Id.ToLower().Equals(searchString.ToLower()));
                        break;
                    default:
                        break;
                }
            }

            int pageSize = 30;
            return View(await PaginatedList<User>.CreateAsync(users.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Utilizador com Id = {id} não encontrado";
                Response.StatusCode = 404;
                return View(nameof(NotFound));
            }
            else
            {
                string deleted_tag = "DELETED_" + user.Id;
                if (deleted_tag.Length > 256)
                {
                    deleted_tag = deleted_tag.Substring(0, 255);
                }
                user.Name = "[Deleted]";
                user.UserName = deleted_tag;
                user.NormalizedUserName = deleted_tag;
                user.Email = deleted_tag;
                user.PasswordHash = deleted_tag;
                user.Age = 0;

                var result = await userManager.UpdateAsync(user);

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

                var userEscola = await _context.UserEscolas.Include(u => u.IdEscolaNavigation).FirstOrDefaultAsync(u => u.IdUser == id);
                if (userEscola != null)
                {
                    ViewData["EscolaID"] = userEscola.IdEscola;
                    ViewData["EscolaNome"] = userEscola.IdEscolaNavigation.NomeEscola;
                }
                else {
                    ViewData["EscolaID"] = "N/A";
                    ViewData["EscolaNome"] = "N/A";
                }

                
                
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

        public async Task<IActionResult> RemoveFromSchool(string idUser, int idEscola)
        {
            User user = await userManager.FindByIdAsync(idUser);
            if (user != null) {
                var userEscola = _context.UserEscolas.Where(u => u.IdUser.Equals(idUser) && u.IdEscola == idEscola).First();
                if (userEscola != null) {
                    _context.UserEscolas.Remove(userEscola);
                    await _context.SaveChangesAsync();
                    //_context.UserEscolaHistoricos.Add(userEscola);
                    return RedirectToAction("UpdateUser", new { id = user.Id });
                }
            }
            return View(nameof(NotFound));

        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }
    }
}