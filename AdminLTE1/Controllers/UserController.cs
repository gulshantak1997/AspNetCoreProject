using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AdminLTE1.Areas.Identity.Pages.Account;
using AdminLTE1.Helper;
using AdminLTE1.Models;
using AdminLTE1.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLTE1.Controllers
{
    [AuthorizeActionFilter]
    public class UserController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;

        private readonly IHostingEnvironment _hostingEnvironment;
        public UserController(UserManager<ApplicationUser> userManager, AppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _context=context;
            _hostingEnvironment = hostingEnvironment;

        }
        public async Task<IActionResult> ListUser()
        {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
        }


        //public async Task<IActionResult> DeleteUser()
        //{
        //    var users = await _userManager.Users.ToListAsync();
        //    return View(users);
        //}

        //[HttpPost]
        //public async Task<IActionResult> DeleteUser(string id)
        //{
        //    var user = await _userManager.FindByIdAsync(id);

        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = $"Role with ID = {id} cannot be found";
        //        return View("NotFound");
        //    }
        //    else
        //    {
        //        var result = await _userManager.DeleteAsync(user);
        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("ListUser");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //        return View("ListUser");
        //    }
        //}


        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"User with ID = {id} cannot be found";
                return View("NotFound");
            }
            else
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListUser");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View("ListUser");
            }
        }



        [HttpGet]        public async Task<IActionResult> EditUser(string id)        {            var user = await _userManager.FindByIdAsync(id);            if (user == null)            {                ViewBag.ErrorMessage = $"User with Id = {id} cannot be found";                return View("NotFound");            }
            //var userClaims = await _userManager.GetClaimsAsync(user);
            //var userRoles = await _userManager.GetRolesAsync(user);            var model = new EditUserViewModel            {                Email = user.Email,
                //UserName = user.UserName,
                FirstName=user.FirstName,                LastName=user.LastName,                PhoneNumber=user.PhoneNumber,                Image=user.Image,                CountryId=user.CountryId,                StateId=user.StateId,                CityId=user.CityId,                            };            return View(model);        }        [HttpPost]        public async Task<IActionResult> EditUser(EditUserViewModel model)        {            var user = await _userManager.FindByIdAsync(model.Id);            if (user == null)            {                ViewBag.ErrorMessage = $"User with Id = {model.Id} cannot be found";                return View("NotFound");            }            else            {                user.Email = model.Email;
                //user.UserName = model.UserName;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.PhoneNumber = model.PhoneNumber;



                //var data = Encoding.ASCII.GetBytes(model.Password);
                //user.PasswordHash = Convert.ToString(data);


                user.CountryId = model.CountryId;
                user.StateId = model.StateId;
                user.CityId = model.CityId;





                if (model.ImageFile!=null)
                {

                    if (user.Image != null)
                    {

                        var paths = Path.Combine(_hostingEnvironment.WebRootPath, "Images", user.Image);

                        if (System.IO.File.Exists(paths))
                        {
                            // If file found, delete it    
                            System.IO.File.Delete(Path.Combine(paths));
                        }

                    }

                    string wwwRootPath = _hostingEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    string extension = Path.GetExtension(model.ImageFile.FileName);
                    user.Image = DateTime.Now.ToString("yymmssfff") + extension;


                    string path = Path.Combine(wwwRootPath, "Images", user.Image);
                    var fileStream = new FileStream(path, FileMode.Create);
                    model.ImageFile.CopyTo(fileStream);
                    await _userManager.UpdateAsync(user);


                }


                var result = await _userManager.UpdateAsync(user);                if (result.Succeeded)                {                    return RedirectToAction("ListUser");                }                foreach (var error in result.Errors)                {                    ModelState.AddModelError("", error.Description);                }                return View(model);            }        }



        public IActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(RegisterModel.InputModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    CountryId = model.CountryId,
                    StateId = model.StateId,
                    CityId = model.CityId
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {

                    return RedirectToAction("ListUser", "User");
                }
                return View(model);
            }
            else
                return View(model);
        }
    }

}





