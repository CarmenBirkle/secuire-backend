using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF
{
    public class PaymentCardEntry : DataEntry
    {
        public override int? Id { get; set; }
        public string Owner { get; set; } = string.Empty;
        public string Cardnumber { get; set; } = string.Empty;
        public string CardType { get; set; } = string.Empty;
        public string ExpirationDate { get; set; } = string.Empty;
        public string Pin { get; set; } = string.Empty;
        public string Cvv { get; set; } = string.Empty;
    }
}
