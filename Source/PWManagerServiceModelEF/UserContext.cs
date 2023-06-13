using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    //public class UserContext : IdentityUserContext<IdentityUser>
    //{
    //    //ToDo: in Konfig auslagern
    //    private string connectionString = "Server=isefpwmanagerdbserver.database.windows.net;Database=ISEFPWManagerDB;User Id=isefsa;Password=5^#YA8VdGobZKC92eAgVsxJXJf2ZZL8i%y@2r&s2^B%7x3sHC@bVDdPWDyrxF@85ryWEfXs48ABy*i^tgEx53F8ytU$#LZPu$svTjQ3@bB&qVAEofC9RpSzzD7tRMyMK;";

    //    public UserContext(DbContextOptions<UserContext> options) : base(options)
    //    {

    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder options)
    //    {
    //        // It would be a good idea to move the connection string to user secrets
    //        base.OnConfiguring(options);
    //        options.UseSqlServer(connectionString);
    //    }

    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {
    //        base.OnModelCreating(modelBuilder);
    //    }
    //}
}
