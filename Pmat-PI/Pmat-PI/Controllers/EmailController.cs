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

namespace Pmat_PI.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;
        public EmailController(IEmailSender emailSender, IHostingEnvironment env)
        {
            _emailSender = emailSender;
        }
        public IActionResult EnviaEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EnviaEmail(EmailModel2 email)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //TesteEnvioEmail(email.Destino, email.Assunto, email.Mensagem).GetAwaiter();
                    TesteEnvioEmail2(email.Destino,email.Assunto,email.Mensagem).GetAwaiter();
                    return RedirectToAction("EmailEnviado");
                }
                catch (Exception)
                {
                    return RedirectToAction("EmailFalhou");
                }
            }
            return View(email);
        }

      

        public async Task TesteEnvioEmail2(string email, string assunto, string mensagem)
        {
            try
            {
                Console.WriteLine(" Destino,assunto,mensagem=" + email + "," + assunto + "," + mensagem);

                // GENERATE MESSAGE TO SEND
                MimeMessage message = new MimeMessage();
                MailboxAddress from = new MailboxAddress("Admin","pmate.backoffice.test@gmail.com");
                MailboxAddress to = new MailboxAddress("User",email);

                message.From.Add(from);
                message.To.Add(to);
                message.Subject = assunto;

                Console.WriteLine("Before authentication!");
                // SEND MESSAGE 
                SmtpClient client = new SmtpClient();

                //client.Connect("smtp.gmail.com", 25, true);
                //client.Authenticate("pmate.backoffice.test@gmail.com", "backoffice123");

                //client.Connect("smtp.gmail.com", 465, false);
                //client.Authenticate("pmate.backoffice.test@gmail.com", "backoffice123");
                

                Console.WriteLine("Authentication status:" + client.IsAuthenticated);
                Console.WriteLine("Now it is authenticated!");
                client.Send(message);

                Console.WriteLine("If it reached here, it should have been sent successfully");

                // CLOSE SMTP
                client.Disconnect(true);
                client.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task TesteEnvioEmail(string email, string assunto, string mensagem)
        {
            try
            {
                Console.WriteLine("Right before sending email! Destino,assunto,mensagem=" +  email + "," + assunto + "," + mensagem );
                //email destino, assunto do email, mensagem a enviar
                await _emailSender.SendEmailAsync(email, assunto, mensagem);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult EmailEnviado()
        {
            return View();
        }
        public ActionResult EmailFalhou()
        {
            return View();
        }
    }
}
