using PWManagerServiceModelEF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    /// <summary>
    /// Einträge, welche User abspeichert - alles verschlüsselt -> daher überall Datentyp string
    /// </summary>
    public class DataEntry
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public string Subject { get; set; }
        public string Favourite { get; set; }
        public string Comment { get; set; }
        public string SelectedIcon { get; set; }
        public string CustomTopics { get; set; }
    }
}
