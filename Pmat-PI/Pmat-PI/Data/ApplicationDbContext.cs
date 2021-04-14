using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pmat_PI.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pmat_PI.Data
{
    public class ApplicationDbContext : IdentityDbContext<Pmat_PI.Models.User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {}
        //public DbSet<User> Users { get; set; }
    }
}
