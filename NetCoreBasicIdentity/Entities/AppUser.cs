using Microsoft.AspNetCore.Identity;

namespace NetCoreBasicIdentity.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public string ImagePath { get; set; }
        public string Gender { get; set; }
    }
}
