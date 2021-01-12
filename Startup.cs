using CoreCRUDwithORACLE.Interfaces;
using CoreCRUDwithORACLE.Models;
using CoreCRUDwithORACLE.Servicios;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreCRUDwithORACLE
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
            //services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddDbContext<ApplicationUser>(options => options.UseOracle(Configuration.GetConnectionString("OracleDBConnection")));
            services.AddControllersWithViews();
            services.AddTransient<IServicioJunta, ServicioJunta>();
            services.AddTransient<IServicioUsuario, ServicioUsuario>();
            services.AddTransient<IServicioActa, ServicioActa>();
            //services.AddControllersWithViews();
            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc().AddRazorPagesOptions(options => {
                //options.Conventions.AddPageRoute("/Junta/Index","");
                options.Conventions.AddPageRoute("/Account/Login", "");
            });
            services.AddDistributedMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(15);
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
            }

            app.UseSession();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllerRoute(
            //        name: "default",
            //        pattern: "{controller=Junta}/{action=Index}/{id?}");
            //});
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}");
            });
        }
    }
}
