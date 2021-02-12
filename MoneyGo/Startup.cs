using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using MoneyGo.Data;
using MoneyGo.Helpers;
using MoneyGo.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MoneyGo
{
    public class Startup
    {
        IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                }
                ).AddCookie();

            services.AddSession(OptionsBuilderConfigurationExtensions =>
            {
                OptionsBuilderConfigurationExtensions.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            String database = configuration.GetConnectionString("database");

            services.AddSingleton<IConfiguration>(this.configuration);
            services.AddSingleton<MailService>();
            //services.AddSingleton<UploadService>();
            services.AddSingleton<PathProvider>();

            services.AddTransient<RepositoryTransacciones>();
            services.AddDbContext<TransaccionesContext>(options => options.UseSqlServer(database));

            services.AddControllersWithViews(option => option.EnableEndpointRouting = false);
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

            //Session
            app.UseSession();
            
            app.UseRouting();

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default"
                    , template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
