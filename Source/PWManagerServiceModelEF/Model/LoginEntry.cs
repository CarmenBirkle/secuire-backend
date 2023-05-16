using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class LoginEntry : DataEntry
    {
        public LoginEntry() { }
        public override int? Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
