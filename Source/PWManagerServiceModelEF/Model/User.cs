using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class User : IdentityUser
    {
        //public int Id { get; set; }
        //public string Email { get; set; }
        //public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PasswordHint { get; set; } = string.Empty;
        public DateTime AgbAcceptedAt { get; set; }
        public int FailedLogins { get; set; }
        public bool LockedLogin { get; set; }
        public string Salt { get; set; } = string.Empty;

        public ICollection<DataEntry> DataEntries { get; set; } = new List<DataEntry>();
    }
}
