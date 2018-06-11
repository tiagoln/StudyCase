using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DataSeed
    {
        public static async Task<IWebHost> SeedData(this IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetService<RoleManager<Role>>();

                await SeedRoles(roleManager);
            }

            return host;
        }

        private static async Task SeedRoles(RoleManager<Role> roleManager)
        {
//            if (!roleManager.Roles.Any())
//            {
//                var roles = new List<Role>
//                {
//                    new Role {Name = "Administrator",},
//                    new Role {Name = "Customer",},
//                    new Role {Name = "Employee",}
//                };
//
//                foreach (var role in roles)
//                {
//                    await roleManager.CreateAsync(role);
//                }
//            }
        }
    }
}