using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    /// <summary>
    /// Einträge, welche User abspeichert - alles verschlüsselt -> daher überall Datentyp string
    /// </summary>
    public abstract class DataEntry
    {
        public DataEntry() { }
        //public int Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        [NotMapped]
        public Dictionary<string, string> CustomTopics { get; set; } = new Dictionary<string, string>();
        public string Comment { get; set; } = string.Empty;
        public string Favourite { get; set; } = string.Empty;
    }

}
