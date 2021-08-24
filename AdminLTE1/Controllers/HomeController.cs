using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminLTE1.Models;
using Newtonsoft.Json;
using AdminLTE1.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AdminLTE1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public HomeController(UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;

        }



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        public JsonResult Country()        {            var coun = _context.Country.                Select(x => new { value = x.CountryId, text = x.CountryName }).ToList();            return Json(coun);        }

        public JsonResult GetStatesByCountryId(int CId)
        {
            var sta = _context.State.Where(x => x.CountryId == CId).
                Select(x => new { value = x.StateId, text = x.StateName }).ToList();
            return Json(sta);
        }
        public JsonResult GetCitiesByStateId(int StateId)
        {
            var city = _context.City.Where(x => x.StateId == StateId).
                Select(x => new { value = x.CityId, text = x.CityName }).ToList();
            return Json(city);
        }




        public JsonResult PageAlreadyExist(string pageName/*, int? Id*/)        {            var validateName = _context.CMS.FirstOrDefault(x => x.PageName == pageName /*&& x.Id != Id*/);            if (validateName != null)            {                return Json(false, new JsonSerializerSettings());            }            else            {                return Json(true, new JsonSerializerSettings());            }        }


        public JsonResult URLAlreadyExist(string pageURL)        {            var validateName = _context.CMS.FirstOrDefault(x => x.PageURL == pageURL);            if (validateName != null)            {                return Json(false, new JsonSerializerSettings());            }            else            {                return Json(true, new JsonSerializerSettings());            }        }






        //public JsonResult EmailAlreadyExist(string Email)
        //{
        //    var validateName = _context.Users.FirstOrDefault(x => x.Email == Email);

        //    if (validateName != null)
        //    {
        //        return Json(false, new JsonSerializerSettings());
        //    }
        //    else
        //    {
        //        return Json(true, new JsonSerializerSettings());
        //    }
        //}





        [HttpGet]
        public IActionResult AddQuestions()
        {
            return View();
        }


        [HttpPost]

        public IActionResult AddQuestions(QuestionsViewModel model)
        {

            if (model != null)
            {
                var questionModel = new Questions();
                questionModel.Title = model.Title;
                questionModel.Type = model.Type;

                _context.Questions.Add(questionModel);
                _context.SaveChanges();

                return RedirectToAction("ViewQuestions");
            }
            return View();

        }




        public IActionResult ViewQuestions()
        {
            List<QuestionsViewModel> lstmodel = new List<QuestionsViewModel>();
            var models = _context.Questions;

            foreach (var item in models)
            {
                QuestionsViewModel qvmObj = new QuestionsViewModel();
                qvmObj.Id = item.Id;
                qvmObj.Title = item.Title;
                qvmObj.Type = item.Type;

                lstmodel.Add(qvmObj);
            }

            ViewBag.successMessage = TempData["successMessage"];
            return View(lstmodel);
        }

        [HttpGet]
        public IActionResult AddOptions(int Id)
        {
            AddOptionWithList addOption = new AddOptionWithList();
            var optionList = (from opt in _context.RelatedValues
                              join ques in _context.Questions
                              on opt.QuestionId equals Id
                              select new AddOption
                              {
                                  Id = opt.Id,
                                  Option = opt.Values,

                              }).GroupBy(n => new { n.Id, n.Option }).Select(g => g.FirstOrDefault()).ToList();

            addOption.lstAddOption = optionList;

            return View(addOption);

        }


        [HttpPost]
        public IActionResult AddOptions(AddOptionWithList model)
        {

            if (model != null)
            {
                RelatedValues obj = new RelatedValues();
                obj.QuestionId = model.Id;
                obj.Values = model.Option;

                _context.RelatedValues.Add(obj);
                _context.SaveChanges();

                return RedirectToAction("ViewQuestions");
            }

            return View();

        }



        public IActionResult Survey()
        {

            List<QuestionsRelatedValueViewModel> questionList = new List<QuestionsRelatedValueViewModel>();



                var lstquestion = (from t in _context.Questions
                                   select new Questions
                                   {
                                       Id = t.Id,
                                       Title = t.Title,
                                       Type = t.Type
                                   }).ToList();

                foreach (var item in lstquestion)
                {
                    QuestionsRelatedValueViewModel questions = new QuestionsRelatedValueViewModel();
                    questions.Id = item.Id;
                    questions.Title = item.Title;
                    questions.Type = item.Type;

                    if (item.Type == TitleType.RadioButton.ToString() || item.Type == TitleType.CheckBox.ToString() || item.Type == TitleType.DropDown.ToString())
                    {

                        questions.SelectList = (from que in _context.RelatedValues.Where(r => r.QuestionId == item.Id)
                                                select new SelectListItem
                                                {
                                                    Value = que.Id.ToString(),
                                                    Text = que.Values
                                                }).ToList();
                    }

                    questionList.Add(questions);

                }


                return View(questionList);

            

        }

        [HttpGet]
        public IActionResult SurveyPost()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SurveyPost([FromBody] FeedbackResultViewmodel[] survey)
        {

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            foreach (var item in survey)
            {

                FeedbackResult itemList = new FeedbackResult();

                itemList.TeamId = userId;
                itemList.QuestionId = item.QuestionId;
                if (item.Type == TitleType.TextBox.ToString()
                    || item.Type == TitleType.TextArea.ToString())
                {
                    itemList.Answer = item.Answer;
                }
                else
                {
                    itemList.OptionId = item.Answer;
                }

                _context.FeedbackResult.Add(itemList);
                _context.SaveChanges();

            }

            return Json("You have posted the feedback successfully");



        }


        public IActionResult SurveyNew()
        {

            List<QuestionsRelatedsurvayViewModel> questionList = new List<QuestionsRelatedsurvayViewModel>();

            var lstquestion = (from t in _context.Questions
                               select new Questions
                               {
                                   Id = t.Id,
                                   Title = t.Title,
                                   Type = t.Type
                               }).ToList();

            foreach (var item in lstquestion)
            {
                QuestionsRelatedsurvayViewModel questions = new QuestionsRelatedsurvayViewModel();
                questions.Id = item.Id;
                questions.Title = item.Title;
                questions.Type = item.Type;

                if (item.Type == TitleType.RadioButton.ToString() || item.Type == TitleType.CheckBox.ToString() || item.Type == TitleType.DropDown.ToString())
                {

                    questions.SelectList = (from que in _context.RelatedValues.Where(r => r.QuestionId == item.Id)
                                            select new SelectListItem
                                            {
                                                Value = que.Id.ToString(),
                                                Text = que.Values
                                            }).ToList();
                }
                questionList.Add(questions);
            }
            return View(questionList);
        }


        [HttpPost]
        public IActionResult SurveyNew([FromBody] QuestionsRelatedsurvayViewModel[] survey)
        {

            //foreach (var item in survey)
            //{
            //    var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //    item.TeamId = userId;
            //    //_context.FeedbackResult.Add(item);
            //    _context.SaveChanges();
            //}

            return Json("You have posted the feedback successfully");
        }

    }
}
