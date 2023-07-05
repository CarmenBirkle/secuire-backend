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
using System.Runtime.Serialization;

namespace PWManagerServiceModelEF
{
    public class DataContext : IdentityUserContext<IdentityUser>
    {
        //ToDo: in Konfig auslagern
        private string connectionString = "Server=isefpwmanagerdbserver.database.windows.net;Database=ISEFPWManagerDB;User Id=isefsa;Password=5^#YA8VdGobZKC92eAgVsxJXJf2ZZL8i%y@2r&s2^B%7x3sHC@bVDdPWDyrxF@85ryWEfXs48ABy*i^tgEx53F8ytU$#LZPu$svTjQ3@bB&qVAEofC9RpSzzD7tRMyMK;";
            //"Server=(LocalDB)\\MSSQLLocalDB;Database=ISEFPWManagerDB;Integrated Security=SSPI;";
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
            modelBuilder.Entity<Configuration>();
            modelBuilder.Entity<UserFailedLoginHistory>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<DataEntry> DataEntry { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<PaymentCard> PaymentCard { get; set; }
        public DbSet<SafeNote> SafeNote { get; set; }
        public DbSet<Configuration> Configuration { get; set; }
        public DbSet<UserFailedLoginHistory> UserFailedLoginHistories { get; set; }


        public void DeleteUserFailedLoginHistory(User user)
        {
            List<UserFailedLoginHistory> history = UserFailedLoginHistories.Where(u => u.UserId == user.IdentityUserId).ToList();
            UserFailedLoginHistories.RemoveRange(history);
        }

        public List<UserFailedLoginHistory> GetUserFailedLoginHistory(User user)
        {
            List<UserFailedLoginHistory> history = UserFailedLoginHistories.Where(h => h.UserId == user.IdentityUserId).OrderBy(h => h.TimeStamp).ToList();
            return history;
        }

        public Configuration? GetConfiguration(string key)
        {
            return Configuration.FirstOrDefault(x => x.Key == key);
        }

        public async Task<User?> GetUser(string email, UserManager<IdentityUser> userManager = null)
        {
            try
            {
                User user = User.Where(user => user.IdentityUser.Email == email).ToList().Single();
                if(userManager != null)
                    user.IdentityUser = await userManager.FindByEmailAsync(email);

                return user;
            }
            catch(InvalidOperationException)
            {
                return null;
            }
        }

        public DataEntry? GetDataEntry(int id)
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

        public DataEntry? GetDataEntry(int id, string userId)
        {
            DataEntry? entry = GetDataEntry(id);
            if (entry == null) return entry;

            return entry.UserId == userId ? entry : null;
        }

        public List<DataEntry> GetDataEntry(string userId)
        {
            List<DataEntry> entryList = new List<DataEntry>();
            entryList = this.DataEntry.Where(entry => entry.UserId == userId).ToList();
            return entryList;
        }


        public List<object> GetAllDataEntries(User user)
        {
            List<object> dataEntries = new List<object>();
            List<PaymentCard> paymentCards = this.GetPaymentCard().Where(x => x.DataEntry.UserId == user.IdentityUserId).ToList();
            paymentCards.ForEach(pc => pc.DataEntry.Category = "paymentcard");
            List<SafeNote> safeNotes = this.GetSafeNote().Where(x => x.DataEntry.UserId == user.IdentityUserId).ToList();
            safeNotes.ForEach(sn => sn.DataEntry.Category = "safenote");
            List<Login> logins = this.GetLogin().Where(x => x.DataEntry.UserId == user.IdentityUserId).ToList(); ;
            logins.ForEach(l => l.DataEntry.Category = "login");

            dataEntries.AddRange(paymentCards);
            dataEntries.AddRange(safeNotes);
            dataEntries.AddRange(logins);

            return dataEntries;
        }
      
        public (object?, DataEntry?) GetFullEntry(int id, string userId)
        {
            DataEntry? entry = GetDataEntry(id, userId);
            if (entry == null)
                return (null, null);

            Login? login = GetLogin(id);
            if (login != null) return (login, entry);
            PaymentCard? paymentCard = GetPaymentCard(id);
            if(paymentCard != null) return (paymentCard, entry);
            SafeNote? safeNote = GetSafeNote(id);
            if(safeNote != null) return (safeNote, entry);

            return (null, null);
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
        public List<PaymentCard> GetPaymentCard(User user)
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
