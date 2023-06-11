using Microsoft.EntityFrameworkCore;
using PWManagerServiceModelEF.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PWManagerServiceModelEF
{
    public class DataContext : DbContext
    {
        private string connectionString = "Server=isefpwmanagerdbserver.database.windows.net;Database=ISEFPWManagerDB;User Id=isefsa;Password=5^#YA8VdGobZKC92eAgVsxJXJf2ZZL8i%y@2r&s2^B%7x3sHC@bVDdPWDyrxF@85ryWEfXs48ABy*i^tgEx53F8ytU$#LZPu$svTjQ3@bB&qVAEofC9RpSzzD7tRMyMK;";
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
            modelBuilder.Entity<CardType>();
            modelBuilder.Entity<CustomTopic>();

            //modelBuilder.Entity<SafeNoteEntry>();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> User { get; set; }
        public DbSet<DataEntry> DataEntry { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<PaymentCard> PaymentCard { get; set; }
        public DbSet<SafeNote> SafeNote { get; set; }
        public DbSet<CustomTopic> CustomTopic { get; set; }

        public DbSet<CardType> CardType { get; set; }

        public DataEntry GetDataEntry(int id)
        {
            return DataEntry
                .Where(d => d.Id == id)
                .ToList()
                .Single();
        }
        public List<PaymentCard> GetPaymentCard()
        {
            return PaymentCard
                .Include(d => d.DataEntry)
                .ToList();
        }
        public PaymentCard GetPaymentCard(int id)
        {
            return PaymentCard
                .Where(p => p.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();
        }

        public List<Login> GetLogin()
        {
            return Login
                .Include(d => d.DataEntry)
                .ToList();
        }

        public Login GetLogin(int id)
        {
            return Login
                .Where(l => l.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();
        }

        public List<SafeNote> GetSafeNote()
        {
            return SafeNote
                .Include(d => d.DataEntry)
                .ToList();
        }

        public SafeNote GetSafeNote(int id)
        {
            return SafeNote
                .Where(s => s.DataEntryId == id)
                .Include(d => d.DataEntry)
                .ToList()
                .Single();
        }

        public void SeedData()
        {
            if (!User.Any())
            {
                var users = new[]
                {
                    new User
                    {
                        Username = "Dominik",
                        Password = "mein_passwort9",
                        PasswordHint = "Passworthinweis",
                        AgbAcceptedAt = DateTime.Now.AddDays(-1),
                        FailedLogins = 0,
                        LockedLogin = false,
                        Salt = "salzig"
                    },
                    new User
                    {
                        Username = "Stephan",
                        Password = "mein_passwort11",
                        PasswordHint = "Passworthinweis2",
                        AgbAcceptedAt = DateTime.Now.AddDays(-11),
                        FailedLogins = 0,
                        LockedLogin = false,
                        Salt = "sehr_salzig"
                    },
                };

                User.AddRange(users);
                SaveChanges();
            }

            if (!CardType.Any())
            {
                var cardTypes = new[]
                {
                    new CardType {Type = "VISA"},
                    new CardType {Type = "EC"},
                    new CardType {Type = "MasterCard"},
                };

                CardType.AddRange(cardTypes);
                SaveChanges();
            }

            if (!DataEntry.Any())
            {
                var dataEntries = new[]
                {
                new DataEntry {UserId = 1, Subject = "DataEntry 1-1", Favourite = false, Comment= "Comment 1-1"  },
                new DataEntry {UserId = 1, Subject = "DataEntry 1-2", Favourite = false, Comment= "Comment 1-2"  },
                new DataEntry {UserId = 1, Subject = "DataEntry 1-3", Favourite = false, Comment= "Comment 1-3"  },
                new DataEntry {UserId = 2, Subject = "DataEntry 2-1", Favourite = false, Comment= "Comment 2-1"  },
                new DataEntry {UserId = 2, Subject = "DataEntry 2-2", Favourite = false, Comment= "Comment 2-2"  },
                new DataEntry {UserId = 2, Subject = "DataEntry 2-3", Favourite = false, Comment= "Comment 2-3"  },
                };

                DataEntry.AddRange(dataEntries);
                SaveChanges();

                var logins = new[]
                {
                    new Login
                    {
                        DataEntryId = dataEntries[0].Id,
                        Username = "dominik",
                        Password = "passwort",
                        Url = "https://eineseite.de"
                    },
                    new Login
                    {
                        DataEntryId = dataEntries[3].Id,
                        Username = "stephan",
                        Password = "passwort2",
                        Url = "https://zweiteseite.de"
                    }
                };

                Login.AddRange(logins);
                SaveChanges();

                var safeNote = new[]
                {
                    new SafeNote
                    {
                        DataEntryId = dataEntries[1].Id,
                        Note = "Eine sichere Notiz mit Top Secret Inhalt."
                    },
                    new SafeNote
                    {
                        DataEntryId = dataEntries[4].Id,
                        Note = "Eine zweite sichere Notiz mit Top top Secret Inhalt."
                    }
                };

                var paymentCards = new[]
                {
                    new PaymentCard
                    {
                        DataEntryId = dataEntries[2].Id,
                        Owner = "Karten Inhaber",
                        Number = "1234567890",
                        CardTypeId = CardType.OrderBy(o => o.Id).FirstOrDefault().Id,
                        ExpirationDate = DateTime.Now.AddDays(365),
                        Pin = "0277",
                        Cvv = "069"
                    },
                    new PaymentCard
                    {
                        DataEntryId = dataEntries[5].Id,
                        Owner = "Karten Inhaber2",
                        Number = "09876543210",
                        CardTypeId = CardType.OrderBy(o => o.Id).LastOrDefault().Id,
                        ExpirationDate = DateTime.Now.AddDays(50),
                        Pin = "0277",
                        Cvv = "069"
                    }
                };

                PaymentCard.AddRange(paymentCards);
                SaveChanges();
            }
        }
    }
}
