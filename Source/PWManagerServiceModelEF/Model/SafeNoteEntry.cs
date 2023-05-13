using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class SafeNoteEntry : DataEntry
    {
        public SafeNoteEntry() : base() { }
        public int Id { get; set; }
        public string SafeNote { get; set; } = string.Empty;
    }
}
