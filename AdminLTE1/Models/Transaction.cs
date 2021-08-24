using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionId { get; set; }
        public string Status { get; set; }
        public string CurrencyCode{ get; set; }
        public string Amount { get; set; }
        public string PayerName { get; set; }
        public string MerchantId{ get; set; }
        public string PayerEmail { get; set; }
        public string PayerId { get; set; }
        public string PostalCode { get; set; }
        public string Address { get; set; }




    }
}
