using System;
using Microsoft.AspNetCore.Identity;

namespace NetCoreBasicIdentity.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public DateTime CreatedDate { get; set; }
    }
}
