using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _002_WebApiAutores
{
    public class BanckingSecurityContext : IdentityDbContext
    {
        private readonly DbContextOptions<BanckingSecurityContext> options;

        //private readonly DbContextOptions options;

        public BanckingSecurityContext(DbContextOptions<BanckingSecurityContext> options) : base(options) 
        {
            this.options = options;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);      //this is a must i you configure something in this area
        }
    }
}
