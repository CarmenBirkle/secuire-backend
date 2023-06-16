using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class User
    {
        //public int Id { get; set; }
        //public string Email { get; set; }
        //public string UserName { get; set; } = string.Empty;
        //public string Password { get; set; } = string.Empty;
        [Key, ForeignKey(nameof(IdentityUser))]
        public string IdentityUserId { get; set; }


        public string PasswordHint { get; set; } = string.Empty;
        public DateTime AgbAcceptedAt { get; set; }
        public int FailedLogins { get; set; } 
        public bool LockedLogin { get; set; }
        public string Salt { get; set; } = string.Empty;

        public IdentityUser IdentityUser { get; set; }
        public ICollection<DataEntry> DataEntries { get; set; } = new List<DataEntry>();
    }
}
