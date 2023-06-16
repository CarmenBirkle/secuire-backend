using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PWManagerServiceModelEF
{
    public class DataContext : IdentityUserContext<IdentityUser>
    {
        //ToDo: in Konfig auslagern
        private string connectionString = "Server=(LocalDB)\\MSSQLLocalDB;Database=ISEFPWManagerDB;Integrated Security=SSPI;";
        /*= "Server=isefpwmanagerdbserver.database.windows.net;Database=ISEFPWManagerDB;User Id=isefsa;Password=5^#YA8VdGobZKC92eAgVsxJXJf2ZZL8i%y@2r&s2^B%7x3sHC@bVDdPWDyrxF@85ryWEfXs48ABy*i^tgEx53F8ytU$#LZPu$svTjQ3@bB&qVAEofC9RpSzzD7tRMyMK;";*/
        public DataContext() { }
        public DataContext(string connectionString) 
        { 
            this.connectionString = connectionString; 
        }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {


        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Login>();
            modelBuilder.Entity<DataEntry>();
            modelBuilder.Entity<SafeNote>();
            modelBuilder.Entity<User>();
            modelBuilder.Entity<PaymentCard>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<DataEntry> DataEntry { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<PaymentCard> PaymentCard { get; set; }
        public DbSet<SafeNote> SafeNote { get; set; }


        public DataEntry GetDataEntry(int id)
        {
            try
            {
                DataEntry dataEntry = DataEntry
                .Where(d => d.Id == id)
                .ToList()
                .Single();

                return dataEntry;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
           
        }
        public List<PaymentCard> GetPaymentCard()
        {
            try
            {
                List<PaymentCard> paymentCards = PaymentCard
                .Include(d => d.DataEntry)
                .ToList();

                return paymentCards;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

        }
        public PaymentCard GetPaymentCard(int id)
        {
            try
            {
                PaymentCard paymentCard = PaymentCard
                                .Where(p => p.DataEntryId == id)
                                .Include(d => d.DataEntry)
                                .ToList()
                                .Single();

                return paymentCard;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public List<Login> GetLogin()
        {
            try
            {
                List<Login> logins = Login
                .Include(d => d.DataEntry)
                .ToList();

                return logins;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

        }

        public Login GetLogin(int id)
        {
            try
            {
                Login login = Login
                .Where(l => l.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

                return login;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        public List<SafeNote> GetSafeNote()
        {
            try
            {
                List<SafeNote> safeNotes = SafeNote
                .Include(d => d.DataEntry)
                .ToList();

                return safeNotes;
            }
            catch (InvalidOperationException)
            {

                return null;
            }

        }

        public SafeNote GetSafeNote(int id)
        {
            try
            {
                SafeNote safeNote = SafeNote
                .Where(s => s.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();

                return safeNote;
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }


    }
}
