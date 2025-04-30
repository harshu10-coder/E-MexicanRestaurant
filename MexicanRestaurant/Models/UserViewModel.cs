using Microsoft.AspNetCore.Identity;

namespace MexicanRestaurant.Models
{
    public class UserViewModel
    {
        public IEnumerable<ApplicationUser> Users { get; set; } = null;

        public IEnumerable<IdentityRole> Roles { get; set; } = null;
    }
}
