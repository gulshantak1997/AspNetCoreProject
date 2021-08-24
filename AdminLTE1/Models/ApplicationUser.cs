using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class ApplicationUser:IdentityUser
    {  
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Image { get; set; }


        
    }
}
