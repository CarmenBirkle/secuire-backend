using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string PasswordHint { get; set; }
        public DateTime AgbAcceptedAt { get; set; }
        public int FailedLogins { get; set; } 
        public bool LockedLogin { get; set; }
        public string Salt { get; set; }

        public ICollection<DataEntry> DataEntries { get; set; }
    }
}
