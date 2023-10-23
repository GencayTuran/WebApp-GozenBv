using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApp_GozenBv.Data;
using WebApp_GozenBv.DataHandlers;
using WebApp_GozenBv.DataHandlers.Interfaces;
using WebApp_GozenBv.Managers;
using WebApp_GozenBv.Managers.Interfaces;
using WebApp_GozenBv.Services;

namespace WebApp_GozenBv
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRazorPages().AddRazorRuntimeCompilation();

            services.AddDbContext<DataDbContext>(opts =>
            {
                //opts.UseSqlServer(Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING"));
                opts.UseSqlServer(Configuration.GetConnectionString("DbConnection"));
            });

            services.AddTransient<IUserLogService, UserLogService>();
            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ICarParkManager, CarParkManager>();
            services.AddTransient<ICarParkDataHandler, CarParkDataHandler>();
            services.AddTransient<ICarMaintenanceDataHandler, CarMaintenanceDataHandler>();

            services.AddTransient<IEmployeeManager, EmployeeManager>();
            services.AddTransient<IEmployeeDataHandler, EmployeeDataHandler>();

            services.AddTransient<IStockManager, StockManager>();
            services.AddTransient<IStockDataHandler, StockDataHandler>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.EnsurePopulated(app);
        }
    }
}
