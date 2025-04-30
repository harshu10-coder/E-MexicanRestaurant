using Microsoft.AspNetCore.Identity;

namespace MexicanRestaurant.Models
{
    public class IdentityConfig
    {
        public static async Task CreateAdminUserAsync(IServiceProvider provider)
        {
            var roleManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

            string username = "Harsh@123.com";
            string password = "Harsh@123";
            string roleName = "Admin";

            // if role doesnot exist,create it
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }


            if (await roleManager.FindByNameAsync(username) == null)
            {
                ApplicationUser user = new ApplicationUser { UserName = username, Email = "Harsh@123.com", EmailConfirmed = true };
                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
        }
    }
}
