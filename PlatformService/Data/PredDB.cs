using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PlatformService.Data
{
    public static class PrepDB
    {
        public static void PrepPopulateion(IApplicationBuilder app, bool isProduction)
        {
            using(var serviceScope = app.ApplicationServices.CreateScope())
            {
                SeedData(serviceScope.ServiceProvider.GetService<AppDBContext>(), isProduction);
            }
        }

        private static void SeedData(AppDBContext context, bool isProduction)
        {
            if(isProduction)
            {
                Console.WriteLine("--> Attemting to apply database migrations");
                
                try
                {
                    context.Database.Migrate();
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"--> Failed to apply migrations: {ex.Message}");
                }
            }

            if(!context.Platforms.Any())
            {
                Console.WriteLine("--> Seeding Data...");

                context.Platforms.AddRange(
                    new Models.Platform(){Name = "Don Net", Publisher = "Microsoft", Cost = "Free"},
                    new Models.Platform(){Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free"},
                    new Models.Platform(){Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free"}
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}