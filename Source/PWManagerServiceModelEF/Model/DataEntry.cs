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
    [JsonDerivedType(typeof(LoginEntry))]
    [JsonDerivedType(typeof(SafeNoteEntry))]
    [JsonDerivedType(typeof(PaymentCardEntry))]
    public abstract class DataEntry
    {
        [JsonConstructor]
        public DataEntry() { }
        /// <summary>
        /// bestimmt den eigentlich Typen
        /// </summary>
        public abstract int? Id { get; set; }
        public string Subject { get; set; } = string.Empty;
        [NotMapped]
        public List<CustomTopic> CustomTopics { get; set; }
        public string Comment { get; set; } = string.Empty;
        public string Favourite { get; set; } = string.Empty;
    }
    public class CustomTopic
    {
        public string FieldName { get; set; } = string.Empty;
        public string FieldValue { get; set; } = string.Empty;
    }
}
