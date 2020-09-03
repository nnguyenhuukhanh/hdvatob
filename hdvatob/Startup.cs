using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using hdvatob.Data.Interfaces;
using hdvatob.Data.Model;
using hdvatob.Data.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace hdvatob
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

            services.AddDbContext<hdvatobDbContext>(option => option.UseSqlServer(Configuration.GetConnectionString("hdvatobConectionString")));
            //services.AddSession();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
            });
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddTransient<ILoginRepository, LoginRepository>();
            services.AddTransient<IHoadonRepository, HoadonRepository>();
            services.AddTransient<ICthdvatRepository, CthdvatRepository>();
            services.AddTransient<ICttachveRepository, CttachveRepository>();

            services.AddTransient<IDmhttcRepository, DmhttcRepository>();
            services.AddTransient<IDmtkRepository, DmtkRepository>();
            services.AddTransient<IBaocaoRepository, BaocaoRepository>();
            services.AddTransient<ISupplierRepository, SupplierRepository>();
            services.AddTransient<IDsdangkyhdRepository, DsdangkyhdRepository>();
            services.AddTransient<IDmChinhanhRepository, DmChinhanhRepository>();
            services.AddTransient<ITachveRepository,TachveRepository>();
            services.AddTransient<ICttachveRepository, CttachveRepository>();
            services.AddTransient<IHuyhdvatRepository, HuyhdvatRepository>();
            services.AddTransient<IHuycthdvatRepository, HuycthdvatRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<INguonhdRepository, NguonhdRepository>();
            services.AddTransient<IHoadondientuRepository, HoadondientuRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            var supportedCultures = new[] { new CultureInfo("en-AU") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-AU"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            // app.UseHttpsRedirection();
            app.UseSession();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
