using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class Login
    {
        [Key, ForeignKey(nameof(DataEntry))]
        public int DataEntryId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Url { get; set; }

        public DataEntry DataEntry { get; set; }
    }
}
