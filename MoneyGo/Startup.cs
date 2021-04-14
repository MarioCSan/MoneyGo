using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MoneyGo.Data;
using MoneyGo.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using MoneyGo.Models;
using Microsoft.AspNetCore.Identity;
using MoneyGo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MoneyGo
{
    public class Startup
    {
        IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            String urlapi = this.Configuration["urlapi"];

            #region services
            services.AddHttpContextAccessor();

            services.AddTransient<ServiceTransacciones>(x =>
            {
                var context = x.GetService<IHttpContextAccessor>();
                return new ServiceTransacciones(urlapi, context);
            });

            services.AddTransient<ServiceUsuario>(x =>
            {
                var context = x.GetService<IHttpContextAccessor>();
                return new ServiceUsuario(urlapi, context);
            });

            services.AddSingleton<ServiceSession>();

            #endregion

            #region autenticacion
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme =
                    CookieAuthenticationDefaults.AuthenticationScheme;
                }).AddCookie();

            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            #endregion

            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddSingleton<MailService>();
            services.AddSingleton<UploadService>();
            services.AddSingleton<PathProvider>();

            services.AddControllersWithViews(option => option.EnableEndpointRouting = false);
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseResponseCaching();
            app.UseSession();

            app.UseMvc(configureRoutes =>
            {
                configureRoutes.MapRoute(name: "default", template: "{controller=Landing}/{action=Index}/{id?}");
            });
        }
    }
}
