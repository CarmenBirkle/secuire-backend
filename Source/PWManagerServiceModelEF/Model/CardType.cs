using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PWManagerServiceModelEF.Model
{
    public class CardType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public ICollection<PaymentCard> PaymentCards { get; set; }
    }
}
