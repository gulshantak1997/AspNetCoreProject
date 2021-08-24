﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AdminLTE1.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        private readonly IHostingEnvironment _hostingEnvironment;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,

            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _hostingEnvironment = hostingEnvironment;
        }

        public string Username { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }


            public string Address1 { get; set; }
            public string Address2 { get; set; }


            [Display(Name = "Profile Picture")]
            public string Image { get; set; }

            [Display(Name = "Profile Picture")]
            public IFormFile ImageFile { get; set; }





        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var userName = await _userManager.GetUserNameAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            Username = userName;

            var firstName = user.FirstName;
            var lastName = user.LastName;
            var profilePicture = user.Image;


            var address1 = user.Address1;
            var address2 = user.Address2;

            Input = new InputModel
            {
                Email = email,
                PhoneNumber = phoneNumber,

                Image = profilePicture,
                FirstName = firstName,
                LastName = lastName,
                Address1 = address1,
                Address2 = address2
            };

            IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var email = await _userManager.GetEmailAsync(user);
            if (Input.Email != email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
                if (!setEmailResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
                }
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);


                    throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
                }
            }





            var firstName = user.FirstName;
            var lastName = user.LastName;
            if (Input.FirstName != firstName)
            {
                user.FirstName = Input.FirstName;
                await _userManager.UpdateAsync(user);
            }
            if (Input.LastName != lastName)
            {
                user.LastName = Input.LastName;
                await _userManager.UpdateAsync(user);
            }

            var profilePicture = user.Image;


            if (Input.Image != profilePicture)
            {

                var paths = Path.Combine(_hostingEnvironment.WebRootPath, "Images", user.Image);

                if (System.IO.File.Exists(paths))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(Path.Combine(paths));
                }

                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(Input.ImageFile.FileName);
                string extension = Path.GetExtension(Input.ImageFile.FileName);
                user.Image = DateTime.Now.ToString("yymmssfff") + extension;


                string path = Path.Combine(wwwRootPath, "Images", user.Image);
                var fileStream = new FileStream(path, FileMode.Create);
                Input.ImageFile.CopyTo(fileStream);
                await _userManager.UpdateAsync(user);

            }
            else if (profilePicture == null)
            {
                string wwwRootPath = _hostingEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(Input.ImageFile.FileName);
                string extension = Path.GetExtension(Input.ImageFile.FileName);
                user.Image = DateTime.Now.ToString("yymmssfff") + extension;


                string path = Path.Combine(wwwRootPath, "Images", user.Image);
                var fileStream = new FileStream(path, FileMode.Create);
                Input.ImageFile.CopyTo(fileStream);
                await _userManager.UpdateAsync(user);
            }







            var address1 = user.Address1;
            var address2 = user.Address2;
            if (Input.Address1 != address1)
            {
                user.Address1 = Input.Address1;
                await _userManager.UpdateAsync(user);
            }
            if (Input.Address2 != address2)
            {
                user.Address2 = Input.Address2;
                await _userManager.UpdateAsync(user);
            }




            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostSendVerificationEmailAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            var userId = await _userManager.GetUserIdAsync(user);
            var email = await _userManager.GetEmailAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId = userId, code = code },
                protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(
                email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Verification email sent. Please check your email.";
            return RedirectToPage();
        }
    }
}
