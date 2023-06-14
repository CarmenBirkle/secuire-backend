using PWManagerServiceModelEF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class PaymentCard
    {
        [Key, ForeignKey(nameof(DataEntry))]
        public int DataEntryId { get; set; }
        public string Owner { get; set; }
        public string Number { get; set; }
        public string ExpirationDate { get; set; }
        public string Pin { get; set; }
        public string Cvv { get; set; }
        public DataEntry DataEntry { get; set; }
        public string CardType { get; set; }
    }
}
