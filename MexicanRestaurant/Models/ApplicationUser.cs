using Microsoft.AspNetCore.Identity;
<<<<<<< HEAD
using System.ComponentModel.DataAnnotations.Schema;
=======
>>>>>>> c1a8722ef73c4dbd4fec6dbf10f1525714f22f4d

namespace MexicanRestaurant.Models
{
    public class ApplicationUser:IdentityUser
    {
        public ICollection<Order>?Orders { get; set; }
<<<<<<< HEAD

        [NotMapped]
        public IList<string> RoleNames { get; set; } = null;
=======
>>>>>>> c1a8722ef73c4dbd4fec6dbf10f1525714f22f4d
    }
}
