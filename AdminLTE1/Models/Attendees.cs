using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class Attendees
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Guid AttendeeId{ get; set; }
    }
}
