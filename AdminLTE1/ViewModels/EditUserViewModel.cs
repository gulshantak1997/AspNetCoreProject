using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdminLTE1.ViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {

        }

        public string Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string RoleId { get; set; }


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please select Country")]
        [Display(Name = "Country")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Please select State")]
        [Display(Name = "State")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "Please select City")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Photo")]
        public string Image { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }
}
