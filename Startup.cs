using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BabyStore.Models;
using Microsoft.Extensions.FileProviders;
using System.IO;
using BabyStore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace BabyStore
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
            services.AddRouting();
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<BabyStoreContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("BabyStoreContext")));
            services.AddDefaultIdentity<ApplicationUser>().AddRoles<IdentityRole>()
            .AddDefaultUI(UIFramework.Bootstrap4)
            .AddEntityFrameworkStores<BabyStoreContext>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddDistributedMemoryCache(); //Adds a default in memory implementation of IDistributedCache
            services.AddSession();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                    app.UseDeveloperExceptionPage();
                    app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSession();
            app.UseHttpsRedirection();
            app.UseAuthentication();    //set for authentication
            app.UseStaticFiles();       //for www root
            app.UseStaticFiles(new StaticFileOptions               //for Content folder
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = "/StaticFiles"
            });
            app.UseCookiePolicy();

            //We won't use MapRoute for /Products/Create because Create is an action and automaticatically routes to /Products/Create and even
            //it does not map with /Products/{category} so when ex: /Products/create Request then create is not treat as a parameter for category
            //it acts as a Action Method Create()


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "ProductsCreate", 
                    template: "Products/Create", 
                    defaults: new { controller = "Products", action = "Create" }
                    );
                routes.MapRoute(
                    name: "ProductsByCategoryByPage",
                    template: "Products/{category}/Page{pageNumber}",
                    defaults: new { controller = "Products", action = "Index" }
                    );
                routes.MapRoute(
                   name: "ProductsByPage",
                   template: "Products/Page{pageNumber}",
                   defaults: new { controller = "Products", action = "Index" }
                   );
                routes.MapRoute(
                    name: "ProductsbyCategory",
                    template: "Products/{category}",
                    defaults: new { controller = "Products", action = "Index" }
                    );
                routes.MapRoute(
                    name: "ProductsIndex",
                    template: "Products",
                    defaults: new { controller = "Products", action = "Index" }
                    );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
