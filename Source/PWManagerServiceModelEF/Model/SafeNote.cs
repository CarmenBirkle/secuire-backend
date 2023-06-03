using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class SafeNote
    {
        [Key, ForeignKey(nameof(DataEntry))]
        public int DataEntryId { get; set; }
        public string Note { get; set; }

        public DataEntry DataEntry { get; set; }
    }
}
