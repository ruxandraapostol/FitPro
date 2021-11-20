using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using FitPro.BusinessLogic;
using FitPro.Common.DTOs;
using System;
using System.Linq;
using FitPro.Entities;

namespace FitPro.WebApp
{
    public static class ServiceCollectionExtensionMethods
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddScoped<ControllerDependencies>();

            return services;
        }

        public static IServiceCollection AddFitProBusinessLogic(this IServiceCollection services)
        {
            services.AddSingleton(s => { return new DropDowns();});
            services.AddScoped<ServiceDependencies>();
            services.AddScoped<AdminService>();
            services.AddScoped<TrainerService>();
            services.AddScoped<RegularUserAccountService>();
            services.AddScoped<UserService>();
            services.AddScoped<FitProProgramService>();
            services.AddScoped<NutritionistService>();
            services.AddScoped<NutritionTrackService>();

            return services;
        }

        public static IServiceCollection AddFitProCurrentUser(this IServiceCollection services)
        {
            services.AddScoped(s =>
            {
                var accessor = s.GetService<IHttpContextAccessor>();
                var httpContext = accessor.HttpContext;
                var claims = httpContext.User.Claims;

                var userIdClaim = claims?.FirstOrDefault(c => c.Type == "Id")?.Value;
                var userFirstName = claims?.FirstOrDefault(c => c.Type == "FirstName")?.Value;
                var userLastName = claims?.FirstOrDefault(c => c.Type == "LastName")?.Value;
                var userUserName = claims?.FirstOrDefault(c => c.Type == "UserName")?.Value;
                var userEmail = claims?.FirstOrDefault(c => c.Type == "Email")?.Value;
                var userRole = claims?.FirstOrDefault(c => c.Type == "Role")?.Value;
                var userStreak = claims?.FirstOrDefault(c => c.Type == "Streak")?.Value ?? "0";
                var isParsingSuccessful = Guid.TryParse(userIdClaim, out Guid id);

                return new CurrentUserDto
                {
                    Id = id,
                    IsAuthenticated = httpContext.User.Identity.IsAuthenticated,
                    FirstName = userFirstName,
                    UserName = userUserName,
                    LastName = userLastName,
                    Streak = Int32.Parse(userStreak),
                    Role = userRole,
                };
            });

            return services;
        }
    }
}
