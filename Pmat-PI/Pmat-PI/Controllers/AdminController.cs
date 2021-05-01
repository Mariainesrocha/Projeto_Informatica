using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using Pmat_PI.Models;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<User> userManager;
        private IPasswordHasher<User> passwordHasher;

        public AdminController(UserManager<User> usrMgr, IPasswordHasher<User> passwordHash)

        {
            userManager = usrMgr;
            passwordHasher = passwordHash;
        }

        public IActionResult Index()
        {
            return View(userManager.Users);
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
        public async Task<IActionResult> UpdateUser(string id, string email, string password, string name, string age)
        {
            User user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(email))
                {
                    user.Email = email;
                    user.UserName = email;
                }
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(password))
                    user.PasswordHash = passwordHasher.HashPassword(user, password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(name))
                    user.Name = name;
                else
                    ModelState.AddModelError("", "Name cannot be empty");

                if (!string.IsNullOrEmpty(age))
                    user.Age = int.Parse(age);
                else
                    ModelState.AddModelError("", "Age cannot be empty");


                if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
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