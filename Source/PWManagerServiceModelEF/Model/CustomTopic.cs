using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF.Model
{
    public class CustomTopic
    {
        public int Id { get; set; }
        public int DataEntryId { get; set; }
        public string FieldContent { get; set; }
        public string FieldName { get; set; }

        public DataEntry DataEntry { get; set; }
    }
}
