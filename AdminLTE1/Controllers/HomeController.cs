using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdminLTE1.Models;

namespace AdminLTE1.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        public HomeController(AppDbContext context)
        {
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


    }
}
