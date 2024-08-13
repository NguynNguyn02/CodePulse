using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "9f99dfbc-8c7b-4822-a4ac-54eacbdf00e6";
            var writerRoleId = "f8056a5a-a4b4-4262-9cd9-d8134520985d";

            //create Reader and Writer Role
            var roles = new List<IdentityRole>
                {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },
                new IdentityRole()
                {
                    Id=writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Write".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            //Seed the roles

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
