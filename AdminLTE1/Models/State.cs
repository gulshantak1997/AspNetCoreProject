using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string StateName { get; set; }


    }
}
