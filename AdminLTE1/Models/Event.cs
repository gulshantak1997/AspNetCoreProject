﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public DateTime? StartDate{ get; set; }
        public DateTime? EndDate{ get; set; }
        public string Description{ get; set; }
        public bool IsAllDayEvent{ get; set; }

    }
}
