using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class PayPalAPITransaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string Intent { get; set; }
        public string TransactionState { get; set; }
        public string CartId { get; set; }
        public DateTime CreateTime { get; set; }



    }
}
