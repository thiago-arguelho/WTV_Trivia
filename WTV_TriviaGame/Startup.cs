using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WTV_TriviaGame.Identity;
using WTV_TriviaGame.Identity.Stores;
using WTV_TriviaGame.Models;

namespace WTV_TriviaGame
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
            services.AddRazorPages();
            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddTransient<MongoBuilder>();
            //Add User Policies for permission verification on pages
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("Administrator", policyBuilder => policyBuilder.RequireRole("ADMIN"));
                opt.AddPolicy("User", policyBuilder => policyBuilder.RequireRole("USER"));
            });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, opt =>
            {
                opt.Cookie.Name = "CookiAuth";
            });
            //Identity Options and Required Fields
            //MongoUser as User StoreType (With ApplicationUser as a TUser type and Mongo ObjectId as TKey type)
            //MongoRole  as Role StoreType (With ApplicationRole as TRole type and Mongo ObjectId as TKey type)
            services.AddIdentity<ApplicationUser, ApplicationRole>(opt =>
            {
                opt.Password.RequireDigit = false;
                opt.Password.RequiredLength = 6;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Password.RequireNonAlphanumeric = false;

                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);

                opt.SignIn.RequireConfirmedAccount = false;
            })
                .AddDefaultUI()
                .AddDefaultTokenProviders()
                .AddUserStore<MongoUser<ApplicationUser, ObjectId>>()
                .AddRoleStore<MongoRole<ApplicationRole, ObjectId>>();

            //services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AdditionalUserClaimsPrincipalFactory>();
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
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
