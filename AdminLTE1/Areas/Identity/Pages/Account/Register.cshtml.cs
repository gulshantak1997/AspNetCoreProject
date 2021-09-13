using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using System;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace AdminLTE1.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        private readonly AppDbContext _context;



        private readonly IHostingEnvironment _hostingEnvironment;



        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            AppDbContext context,

            IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;

            _hostingEnvironment = hostingEnvironment;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }





        public class InputModel
        {

            [Required]
            [Display(Name = "FirstName")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "LastName")]
            public string LastName { get; set; }

            [Required]
            [Display(Name = "UserName")]
            public string UserName { get; set; }

            [Required(ErrorMessage = "Please select Country")]
            [Display(Name = "Country")]
            public int CountryId { get; set; }

            [Required(ErrorMessage = "Please select State")]
            [Display(Name = "State")]
            public int StateId { get; set; }

            [Required(ErrorMessage = "Please select City")]
            [Display(Name = "City")]
            public int CityId { get; set; }

            [Required]
            [Display(Name = "Address1")]
            public string Address1 { get; set; }

            [Required]
            [Display(Name = "Address2")]
            public string Address2 { get; set; }



            [Display(Name = "Image")]
            public IFormFile ImageFile { get; set; }

            [Display(Name = "Photo")]
            public string Image { get; set; }



            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            //[Remote("EmailAlreadyExist", "Home", ErrorMessage = "Email already exists")]
            public string Email { get; set; }

            [Required]
            [RegularExpression("^((?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])|(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[^a-zA-Z0-9])|(?=.*?[A-Z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])|(?=.*?[a-z])(?=.*?[0-9])(?=.*?[^a-zA-Z0-9])).{8,}$", ErrorMessage = "Passwords must be at least 8 characters and contain at 3 of 4 of the following: upper case (A-Z), lower case (a-z), number (0-9) and special character (e.g. !@#$%^&*)")]
            //[StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }





            //           [AllowAnonymous]
            //public async Task<JsonResult> UserAlreadyExistsAsync(string email)
            //{
            //    var result = 
            //        await _userManager.FindByNameAsync(email) ?? 
            //        await userManager.FindByEmailAsync(email);
            //    return Json(result == null, JsonRequestBehavior.AllowGet);
            //}

        }






        public void OnGet(string returnUrl = null)
        {

            ReturnUrl = returnUrl;

        }



        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    Email = Input.Email,
                    UserName = Input.UserName,
                    Image = Input.Image,
                    CountryId = Input.CountryId,
                    StateId = Input.StateId,
                    CityId = Input.CityId,
                    Address1 = Input.Address1,
                    Address2 = Input.Address2

                };




                if (Input.ImageFile != null)
                {
                    string wwwRootPath = _hostingEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(Input.ImageFile.FileName);
                    string extension = Path.GetExtension(Input.ImageFile.FileName);
                    user.Image = DateTime.Now.ToString("yymmssfff") + extension;


                    string path = Path.Combine(wwwRootPath, "Images", user.Image);

                    var fileStream = new FileStream(path, FileMode.Create);
                    Input.ImageFile.CopyTo(fileStream);
                }


                //{
                //    var file = user.ImageUpload;

                ////Get the path of the folder Images in the wwwroot file
                //var uploads = Path.Combine(_hostingEnvironment.WebRootPath, "Images");
                //var filePathName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                //var fileExtention = Path.GetExtension(filePathName);
                //var fileName = Guid.NewGuid().ToString("N").Substring(0, 6) + fileExtention;
                //var path = Path.Combine(uploads, fileName);



                //var stream = new MemoryStream();

                //var stringBytes = Encoding.UTF8.GetBytes(path);
                //stream.Write(stringBytes, 0, stringBytes.Length);

                //await file.CopyToAsync(stream);




                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    var roleName = "User";
                    await _userManager.AddToRoleAsync(user ,roleName);

                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code = code },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }


                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}

