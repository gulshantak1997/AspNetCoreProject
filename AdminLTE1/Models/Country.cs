using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class Country
    {
        [Key]
        [Required(ErrorMessage = "Country is required.")]

        public int CountryId { get; set; }

        public string CountryName{ get; set; }

    }
}
