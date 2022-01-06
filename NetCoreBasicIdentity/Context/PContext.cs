using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetCoreBasicIdentity.Entities;

namespace NetCoreBasicIdentity.Context
{
    public class PContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public PContext(DbContextOptions<PContext> options) : base(options)
        {
        }
    }
}
