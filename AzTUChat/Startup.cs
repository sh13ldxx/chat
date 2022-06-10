using AzTUChat.DAL;
using AzTUChat.Hubs;
using AzTUChat.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzTUChat
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
            services.AddControllersWithViews();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddHttpContextAccessor();
            services.AddIdentity<AppUser, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(opt =>
            {
                opt.User.RequireUniqueEmail = false;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                opt.Lockout.MaxFailedAccessAttempts = 30;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequiredLength = 1;
                opt.Password.RequireDigit = true;
                opt.Password.RequireUppercase = true;
                opt.Password.RequireLowercase = true; 


            });
            services.AddSignalR();  
            services.ConfigureApplicationCookie(opt =>
            {
                opt.LoginPath = "/Auth/SignIn";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chathub");
            });

        }
    }
}
