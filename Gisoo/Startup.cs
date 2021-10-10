using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Gisoo.Service;
using Gisoo.Service.Interface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Gisoo
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region Authentication

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

            }).AddCookie(options =>
            {
                options.LoginPath = "/User/Login";
                options.LogoutPath = "/Logout";
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
                options.Cookie.Expiration = TimeSpan.FromDays(30);
                
            });

            #endregion
            services.AddMvc();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            #region DataBaseContext
            services.AddDbContext<Gisoo.DAL.Context>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("GisooConnection"));
            }

            );
            #endregion

            #region IOc
            services.AddTransient<INotice, noticeService>();
            services.AddTransient<ISlider, sliderService>();
            services.AddTransient<IAllPrice, allPriceService>();
            services.AddTransient<IUser, userService>();
            services.AddTransient<IFactor, factorService>();
            services.AddTransient<ICity, cityService>();
            services.AddTransient<IInformation, informationService>();
            services.AddTransient<IAdvertisment, advertismentService>();
            services.AddTransient<IBanner, bannerService>();
            services.AddTransient<IArticle, articleService>();
            services.AddTransient<IRole, roleService>();
            services.AddTransient<ILineType, lineTypeService>();
            services.AddTransient<ILine, lineService>();
            services.AddTransient<IClassRoom, classRoomService>();
            services.AddTransient<IClassRoomType, classRoomTypeService>();
            services.AddTransient<IProduct, productService>();
            services.AddTransient<IVisit, visitService>();
            services.AddTransient<IContactUs, contactUsService>();
            services.AddTransient<IAboutUs, aboutUsService>();
            services.AddTransient<IRule, ruleService>();
            services.AddTransient<IReserve, reserveService>();
            services.AddTransient<ILineWeekDate, lineWeekDateService>();
            #endregion

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.IsEssential = true;
                
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var provider = new FileExtensionContentTypeProvider();

            provider.Mappings[".apk"] = "application/vnd.android.package-archive";
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
                ContentTypeProvider = provider
            });
            app.UseHttpsRedirection();
            //app.UseStaticFiles();
            app.UseAuthentication();
            //app.UseMvcWithDefaultRoute();
            app.UseSession();

            app.UseMvc(routes =>
           {
               //routes.MapRoute(
               //    name: "areas",
               //    template: "{area:exists}/{controller=User}/{action=Login}/{id?}"

               //);
               //routes.MapRoute("Default", "{controller=User}/{action=Login}/{id?}");
               routes.MapRoute("Default", "{controller=CustomerHome}/{action=Index}");
               //routes.MapRoute( "default2", "{controller=CustomerHome}/{action=NoticeDetail}/{id}/{title}");
               routes.MapRoute("ActionApi", "api/{controller}/{name?}");
           });

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

    }
}
