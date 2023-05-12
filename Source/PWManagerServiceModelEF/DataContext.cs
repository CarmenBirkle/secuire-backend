using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class DataContext : DbContext
    {
        public DataContext() { }
        public DataContext(DbContextOptions<DataContext> options) : base(options) 
        {
            
        
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server=isefpwmanagerdbserver.database.windows.net;Database=ISEFPWManagerDB;User Id=isefsa;Password=5^#YA8VdGobZKC92eAgVsxJXJf2ZZL8i%y@2r&s2^B%7x3sHC@bVDdPWDyrxF@85ryWEfXs48ABy*i^tgEx53F8ytU$#LZPu$svTjQ3@bB&qVAEofC9RpSzzD7tRMyMK;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LoginEntry>();
            modelBuilder.Entity<SafeNoteEntry>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DataEntry> DataEntry { get; set; }

    }
}
