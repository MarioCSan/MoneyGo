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
        public void ConfigureServices(IServiceCollection services, IHttpContextAccessor accessor)
        {
            String urlapi = this.Configuration["urlapi"];

            
            services.AddSingleton(x => new ServiceTransacciones(urlapi, accessor));
            services.AddHttpContextAccessor();
            services.AddSingleton(x => new ServiceUsuario(urlapi));
            services.AddSingleton<ServiceSession>();

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

            services.AddSession(OptionsBuilderConfigurationExtensions =>
            {
                OptionsBuilderConfigurationExtensions.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            String database = Configuration.GetConnectionString("database");

            services.AddSingleton<IConfiguration>(this.Configuration);
            services.AddSingleton<MailService>();
            //services.AddSingleton<UploadService>();
            services.AddSingleton<PathProvider>();

            services.AddDbContext<TransaccionesContext>(options => options.UseSqlServer(database));

            services.AddControllersWithViews(option => option.EnableEndpointRouting = false);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env )
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();

            

            app.UseRouting();

            app.UseStaticFiles();
            app.UseAuthentication();
            //Session
            app.UseSession();
            app.UseMvc(configureRoutes =>
            {
                configureRoutes.MapRoute(name: "default", template: "{controller=Landing}/{action=Index}/{id?}");
            });
        }
    }
}
