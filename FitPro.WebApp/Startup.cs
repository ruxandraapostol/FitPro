using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FitPro.DataAccess;
using FitPro.BusinessLogic;

namespace FitPro.WebApp
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
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilterAttribute));
            });


            services.AddAutoMapper(options =>
            {
                options.AddMaps(typeof(Startup), typeof(BaseService));
            });

            services.AddDbContext<FitProContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });


            services.AddScoped<UnitOfWork>();

            services.AddFitProCurrentUser();

            services.AddPresentation();
            services.AddFitProBusinessLogic();

            services.AddAuthentication("FitProCookies")
                   .AddCookie("FitProCookies", options =>
                   {
                       options.LoginPath = new PathString("/Account/Login");
                   });

            services.AddAuthorization(option =>
            {
                option.AddPolicy("AdminOnly", policy => policy.RequireClaim("Role", "Admin"));
                option.AddPolicy("NutritionistOnly", policy => policy.RequireClaim("Role", "Nutritionist"));
                option.AddPolicy("TrainerOnly", policy => policy.RequireClaim("Role", "Trainer"));
                option.AddPolicy("RegularOnly", policy => policy.RequireClaim("Role", "Regular"));
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
                app.UseHsts();
            }

            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/NotFound";
                    await next();
                } 
                else if (context.Response.StatusCode == 505)
                {
                    context.Request.Path = "/InternalProblem";
                    await next();
                }
                else if (context.Response.StatusCode == 303)
                {
                    context.Request.Path = "/Unauthorized";
                    await next();
                }
            });


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
            });
        }
    }
}
