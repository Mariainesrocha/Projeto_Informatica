using Microsoft.AspNetCore.Mvc;
using Pmat_PI.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Pmat_PI.Models;
using Microsoft.AspNetCore.Identity;

namespace Pmat_PI.Controllers
{
    public class EmailController : Controller
    {
        private UserManager<User> userManager;
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender, IWebHostEnvironment env, UserManager<User> usrMgr)
        {
            _emailSender = emailSender;
            userManager = usrMgr;
        }


        public IActionResult SendEmail()
        {
            return View();
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Failure()
        {
            return View();
        }




        [HttpPost]
        public IActionResult SendEmail(EmailModel2 email)
        {
            if (ModelState.IsValid)
            {
                try
                {   
                    ProcessEmailSending(email.Destino,email.Assunto,email.Mensagem).GetAwaiter();
                    return RedirectToAction("Success");
                }
                catch (Exception)
                {
                    return RedirectToAction("Failure");
                }
            }
            return View(email);
        }

      


        public async Task ProcessEmailSending(string grupo_destino, string assunto, string mensagem)
        {
            try
            {
                var users =  userManager.GetUsersInRoleAsync(grupo_destino).Result;
              
                foreach (User u in users)
                {
                    if(u.Email!=null && IsValidEmail(u.Email))
                    {
                        Console.WriteLine("Right before sending email! destino,assunto,mensagem=" + u.Email + "," + assunto + "," + mensagem);
                        await _emailSender.SendEmailAsync(u.Email, assunto, mensagem);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
