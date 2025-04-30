using MexicanRestaurant.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Threading.Tasks;

namespace MexicanRestaurant.Controllers
{
    [Authorize(Roles ="Admin,Developer")]
    [Area("Admin")]
    public class UserController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<ApplicationUser> userMngr,
            RoleManager<IdentityRole> roleMngr)
        {
            userManager = userMngr;
            roleManager = roleMngr;
        }

        public async Task<IActionResult> Index()
        {
            List<ApplicationUser> users = new List<ApplicationUser>();
            foreach (ApplicationUser user in userManager.Users)
            {
                user.RoleNames = await userManager.GetRolesAsync(user);
                users.Add(user);
            }

            UserViewModel model = new UserViewModel
            {
                Users = users,
                Roles = roleManager.Roles
            };

            return View(model);
        }

        [HttpPost]

        public async Task<IActionResult> Delete(string id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(id);
            {
                if (user != null)
                {
                    IdentityResult result = await userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        string errorMessage = "";
                        foreach (IdentityError error in result.Errors)
                        {
                            errorMessage += error.Description + " | ";
                        }
                        TempData["Message"] = errorMessage;
                    }
                }
                return RedirectToAction("Index");
            }
        }

        [HttpPost]

        public async Task<IActionResult> AddToRole(string userId, string roleName)
        {
            IdentityRole role = await roleManager.FindByNameAsync(roleName);
            if (role != null)
            {
                TempData["message"] = $"Role '{roleName}' doesn't exist";
            }
            else
            {
                ApplicationUser user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.AddToRoleAsync(user, roleName);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> RemoveFromRole(string userId, string roleName)
        {
           
            
                ApplicationUser user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    await userManager.RemoveFromRoleAsync(user, roleName);
                }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string rolename)
        {
            await roleManager.CreateAsync(new IdentityRole(rolename));

            return RedirectToAction("Index");
        }

        [HttpPost]

        public async Task<IActionResult> CreateAdminRole()
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            await roleManager.DeleteAsync(role);
            return RedirectToAction("Index");
        }

    }
}