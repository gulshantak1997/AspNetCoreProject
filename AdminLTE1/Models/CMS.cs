using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.Models
{
    public class CMS
    {
        public int Id { get; set; }

        [Required]
        [Remote("PageAlreadyExist", "Home",/* AdditionalFields = "Id",*/ ErrorMessage = "Page name already exists")]
        public string PageName { get; set; }

        [Required]
        [Remote("URLAlreadyExist", "Home", ErrorMessage = "Page URL already exists")]
        public string PageURL { get; set; }

        public string Description { get; set; }
        public string BannerImage { get; set; }

        [NotMapped]
        [Required]
        public IFormFile BannerImageFile { get; set; }

    }
}
