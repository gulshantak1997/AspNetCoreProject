using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE1.Controllers
{
    public class PageController : Controller
    {
        private readonly AppDbContext _context;
        public PageController(AppDbContext context)
        {
            _context = context;

        }

        public IActionResult Index(string Id)
        {
            CMS cvm = new CMS();
            var page = _context.CMS.Where(p => p.PageURL == Id).ToList();
            foreach (var item in page)
            {
                cvm.Description = item.Description;
                cvm.BannerImage = item.BannerImage;
            }

            //var objEditor=CKEDITOR.instances["Description"].getData();
            return View(cvm);
        }
    }
}
