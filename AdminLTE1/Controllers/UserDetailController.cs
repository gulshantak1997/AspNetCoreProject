using AdminLTE1.Models;using AdminLTE1.Security;using AdminLTE1.ViewModels;using Microsoft.AspNetCore.Mvc;using Microsoft.AspNetCore.Mvc.Rendering;using Microsoft.Extensions.Configuration;using System;using System.Linq;namespace AdminLTE1.Controllers{

    public class UserDetailController : Controller    {        private readonly AppDbContext _context;        private readonly IConfiguration _configuration;        public UserDetailController(AppDbContext context, IConfiguration configuration)        {            _context = context;            _configuration = configuration;        }




        [HttpGet]
        public IActionResult GetUser(Guid UserId)
        {

            SurveyURLViewModel viewModel = new SurveyURLViewModel();
            var user = _context.Users.Where(x => x.Id == UserId.ToString()).FirstOrDefault();
            if (user != null)
            {
                viewModel.Id = user.Id;
                viewModel.UserName = user.UserName;
                viewModel.FirstName = user.FirstName;
                viewModel.LastName = user.LastName;
                viewModel.Email = user.Email;
                viewModel.Address1 = user.Address1;
            }


            var UserList = (from users in _context.Users
                            select new SelectListItem
                            {
                                Value = users.Id,
                                Text = users.UserName
                            }).ToList();
            ViewBag.UserList = UserList;

            return View(viewModel);

        }





        [HttpPost]        public JsonResult GetUser(SurveyURLViewModel model)        {            var cryptURL = model.UserIdNew +"^"+ model.code;
            Encryption encryptObj = new Encryption();            string uniqueKey = _configuration["Encryption:UniqueKey"];            var encriptURL= encryptObj.EncryptString(cryptURL, uniqueKey);            SurveyUrl survObj = new SurveyUrl();            survObj.URL = cryptURL;            survObj.EncriptURL = encriptURL;            survObj.StartDate = model.StartDate;            survObj.EndDate = model.Enddate;            _context.SurveyURL.Add(survObj);            _context.SaveChanges();            return Json("Saved Successfully");        }


        public IActionResult Decrypt()
        {
            var encriptedURL = _context.SurveyURL.OrderByDescending(x => x.Id).FirstOrDefault().EncriptURL;
            Encryption encryptObj = new Encryption();
            string uniqueKey = _configuration["Encryption:UniqueKey"];
            var decryptURL = encryptObj.DecryptString(encriptedURL, uniqueKey);

            var userId = decryptURL.Split('^')[0];
            if (_context.Users.Where(a => a.Id == userId).Count() > 0)
            {
                return Json("Valid user");
            }
            else
            {
                return Json("Unvalid user");
            }
        }

    }}