using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmat_PI.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message, IFormFile file);
    }
}
