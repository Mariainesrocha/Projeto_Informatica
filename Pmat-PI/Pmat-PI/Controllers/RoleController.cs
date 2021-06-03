using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pmat_PI.Models;

namespace Pmat_PI.Identity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private UserManager<User> userManager;
        private RoleManager<IdentityRole> roleManager;
        public RoleController(RoleManager<IdentityRole> roleMgr, UserManager<User> usrMgr)
        {
            roleManager = roleMgr;
            userManager = usrMgr;
        }

        public ViewResult Index() => View(roleManager.Roles);

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Create() => View();

        // CREATE ROLE go to localhost(....)/Role/Create.
        [HttpPost]
        public async Task<IActionResult> Create([Required] string name)
        {
            Console.WriteLine("INSIDE CREATE ROLE!");
            if (ModelState.IsValid)
            {
                IdentityResult result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            return View(name);
        }

        // DELETE ROLE 
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            Console.WriteLine("Inside Delete Role function!");
            IdentityRole role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                Console.WriteLine("Role Exists !");
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "Função não encontrada");
            return View("Index", roleManager.Roles);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRole(string addRole, string removeRole, string id)
        {
            var msg = "";
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var allRoles = roleManager.Roles;
                var userRoles = userManager.GetRolesAsync(user).Result;

                if (addRole != null)
                {
                    if (!userRoles.Contains(addRole))
                    {
                        await userManager.AddToRoleAsync(user, addRole);
                        msg += " A função " + addRole.ToUpper() + " foi atribuída a este utilizador!"; 
                    }                   
                }

                if (removeRole != null)
                {
                    await userManager.RemoveFromRoleAsync(user, removeRole);
                    msg += " A função " + removeRole.ToUpper() + " foi removida deste utilizador!";
                }

                ViewData["AllRoles"] = new SelectList(allRoles, "Name", "Name");
                ViewBag.Roles = new SelectList(userRoles);

                if (msg != "")
                    TempData["msg"] = msg;
                else
                    TempData["msg"] = " Erro ao tentar alterar funções, tente novamente!";
                return RedirectToAction("updateUser", "Admin", new { id = user.Id });
            }
            else
            {
                TempData["msg"] = " Erro ao tentar encontrar este utilizador";
                return null;
            }
        }
    }
}
