using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class Leave
    {
        public int Id{ get; set; }
        public string UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsHalfDay { get; set; }
        public string Reason { get; set; }
        public string HandOverPId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }


    }
}
