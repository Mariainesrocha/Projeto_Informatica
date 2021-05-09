using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pmat_PI.Data;
using Pmat_PI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pmat_PI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // APP CONTEXTS x DATABASE
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<pmate2demoContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddDbContext<treinoContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDatabaseDeveloperPageExceptionFilter();

            // USE IDENTITY ROLES
            services.AddIdentity<Pmat_PI.Models.User, IdentityRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = false; //optional
                config.SignIn.RequireConfirmedEmail = true; //optional
            })

            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders()
            .AddRoles<IdentityRole>();

            //services.AddDefaultIdentity<Pmat_PI.Models.User>(options => options.SignIn.RequireConfirmedAccount = true)
            //.AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddScoped<IPasswordHasher<Pmat_PI.Models.User>, CPH<Pmat_PI.Models.User>>();
            services.AddRazorPages();

            // MAKES LOGIN PAGE THE START UP PAGE
            services.AddMvc().AddRazorPagesOptions(options =>
            {
                options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    //pattern: "{action=/Identity/Account/Login}/{id?}");
                     pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            
            CreateRoles(serviceProvider);
            //createUser(serviceProvider);
        }

        private void createUser(IServiceProvider serviceProvider) {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var user = new User();
            user.Name = "PhantomUser";
            user.Email = "phantomuser@ua.pt";
            user.UserName = "PhantomUser";
            user.EmailConfirmed = true;
            user.Age = 0;
            user.AccessFailedCount = 0;
            string userPWD = "DannyPhantom123";

            var exists = userManager.FindByEmailAsync("phantomuser@ua.pt");
            //exists.Wait();
            exists = null;
            if (exists == null) {
                Console.WriteLine("IT IS NULL");
                var created_user = userManager.CreateAsync(user, userPWD);
                //created_user.Wait();
            }
            

        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {

            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            Task<IdentityResult> roleResult;
            string[] roleNames = { "Admin", "Professor", "Aluno" };
            
            // Check if the roles exist. If not, create them.
            Task<bool> hasRole;
            foreach (var roleName in roleNames)
            {
                hasRole = roleManager.RoleExistsAsync(roleName);
                hasRole.Wait();

                if (!hasRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new IdentityRole(roleName));
                    roleResult.Wait();
                }
            }
        
        }





    }
}
