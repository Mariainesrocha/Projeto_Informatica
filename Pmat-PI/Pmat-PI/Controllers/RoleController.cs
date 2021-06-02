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

        private async Task ChangeRoleAsync(string addRole, string removeRole, string id)
        {
            Console.WriteLine("aaaa111111111111111111111111aaaaaa");

            User user = await userManager.FindByIdAsync(id);

            if (user != null)
            {
                if (addRole != null)
                {
                    await userManager.AddToRoleAsync(user, addRole);
                    Console.WriteLine("aazzz^^^^^^^^^^^^^^^^^^^^^^^^^^zz");
                }

                if (removeRole != null)
                {
                    Console.WriteLine("aaaaaaaaaaaaaWWWWWWWWWWWWWWWWWWWaaaaaaaaaaaaaaaaa");

                    await userManager.RemoveFromRoleAsync(user, removeRole);
                }
                var allRoles = roleManager.Roles;
                var userRoles = await userManager.GetRolesAsync(user);
                ViewData["AllRoles"] = new SelectList(allRoles, "Id", "Name");
                ViewBag.Roles = new SelectList(userRoles.ToList(), "Id", "Name");
            }
            else
            {
                Console.WriteLine("ERROOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOR");
            }
        }
    }
}
