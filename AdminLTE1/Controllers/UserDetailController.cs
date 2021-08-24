﻿using AdminLTE1.Models;

    public class UserDetailController : Controller




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





        [HttpPost]
            Encryption encryptObj = new Encryption();


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

    }