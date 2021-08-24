using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class Questions
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Type { get; set; }



    }
}
