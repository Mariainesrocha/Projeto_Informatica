using Microsoft.AspNetCore.Mvc;
using Pmat_PI.Services;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Threading.Tasks;
using Pmat_PI.Models;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Pmat_PI.Controllers
{
    [Authorize(Roles = "Admin")]
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
        public IActionResult SendEmail(EmailModel2 email, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                try {
                    ProcessEmailSending(email.Destino,email.Assunto,email.Mensagem,file).GetAwaiter();
                    return RedirectToAction("Success");
                }
                catch (Exception)
                {
                    return RedirectToAction("Failure");
                }
            }
            return View(email);
        }

      


        public async Task ProcessEmailSending(string destino, string assunto, string mensagem,IFormFile file)
        {
            try
            {
                // DESTINO UNICO
                if (IsValidEmail(destino))
                {
                    Console.WriteLine("Destino unico");
                    await _emailSender.SendEmailAsync(destino, assunto, mensagem, file);
                }

                // DESTINO GRUPO
                else
                {
                    IEnumerable<User> users = userManager.GetUsersInRoleAsync(destino).Result;

                    Console.WriteLine("Users length:" + users.ToList().Count);
                    foreach (User u in users)
                    {
                        if (u.Email != null && IsValidEmail(u.Email))
                        {
                            Console.WriteLine("Right before sending email! destino,assunto,mensagem=" + u.Email + "," + assunto + "," + mensagem);
                            await _emailSender.SendEmailAsync(u.Email, assunto, mensagem, file);
                        }
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
