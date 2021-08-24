using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE1.Data;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE1.Controllers
{
    public class CMSController : Controller
    {
        private readonly AppDbContext _context;        private readonly IHostingEnvironment _hostEnvironment;

        public CMSController(AppDbContext context, IHostingEnvironment hostEnvironment)        {            _context = context;            _hostEnvironment = hostEnvironment;        }

        public IActionResult Index()        {

            List<CMS> lstmodel = new List<CMS>();            var models = _context.CMS;            foreach (var item in models)            {                CMS mvm = new CMS();                mvm.Id = item.Id;                mvm.PageName = item.PageName;                mvm.PageURL = item.PageURL;                mvm.Description = item.Description;                mvm.BannerImage = item.BannerImage;                lstmodel.Add(mvm);            }

            return View(lstmodel);        }        [HttpGet]        public IActionResult CreatePage()        {            return View();        }        [HttpPost]        public IActionResult CreatePage(CMS model)        {            if (model != null)            {                var cmsModel = new CMS();                cmsModel.PageName = model.PageName;                cmsModel.PageURL = model.PageURL;                cmsModel.Description = model.Description;




                if (model.BannerImageFile!= null)                {                    string wwwRootPath = _hostEnvironment.WebRootPath;                    string fileName = Path.GetFileNameWithoutExtension(model.BannerImageFile.FileName);                    string extension = Path.GetExtension(model.BannerImageFile.FileName);                    cmsModel.BannerImage = DateTime.Now.ToString("yymmssfff") + extension;                    string path = Path.Combine(wwwRootPath, "CMSImages", cmsModel.BannerImage);
                    var fileStream = new FileStream(path, FileMode.Create);                    model.BannerImageFile.CopyTo(fileStream);                }
                _context.CMS.Add(cmsModel);                _context.SaveChanges();                return RedirectToAction("Index");            }                       return View();        }        [HttpGet]        public IActionResult EditPage(int id)        {            CMS mvm = new CMS();            var page = _context.CMS.FirstOrDefault(m => m.Id == id);            if (page != null)            {                mvm.PageName = page.PageName;                mvm.PageURL = page.PageURL;                mvm.Description = page.Description;                mvm.BannerImage = page.BannerImage;            }            return View(mvm);        }        [HttpPost]        public IActionResult EditPage(CMS model)        {            if (model != null)            {                var PageItem = _context.CMS.FirstOrDefault(m => m.Id == model.Id);                if (PageItem != null)                {                    PageItem.PageName = model.PageName;                    PageItem.PageURL = model.PageURL;                    PageItem.Description = model.Description;                    var BannerImage = PageItem.BannerImage;                    if (model.BannerImageFile != null)                    {                        if (BannerImage != model.BannerImage)                        {                            string wwwRootPath = _hostEnvironment.WebRootPath;                            string fileName = Path.GetFileNameWithoutExtension(model.BannerImageFile.FileName);                            string extension = Path.GetExtension(model.BannerImageFile.FileName);                            PageItem.BannerImage = DateTime.Now.ToString("yymmssfff") + extension;                            string path = Path.Combine(wwwRootPath, "CMSImages", PageItem.BannerImage);
                            //var mem = new MemoryStream();
                            //user.ProfilePicture
                            var fileStream = new FileStream(path, FileMode.Create);                            model.BannerImageFile.CopyTo(fileStream);                        }                        else if (BannerImage == null)                        {                            string wwwRootPath = _hostEnvironment.WebRootPath;                            string fileName = Path.GetFileNameWithoutExtension(model.BannerImageFile.FileName);                            string extension = Path.GetExtension(model.BannerImageFile.FileName);                            PageItem.BannerImage = DateTime.Now.ToString("yymmssfff") + extension;                            string path = Path.Combine(wwwRootPath, "CMSImages", PageItem.BannerImage);
                            //var mem = new MemoryStream();
                            //user.ProfilePicture
                            var fileStream = new FileStream(path, FileMode.Create);                            model.BannerImageFile.CopyTo(fileStream);                        }                    }                    _context.Entry(PageItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;                    _context.SaveChanges();                    return RedirectToAction("Index");                }                return View();            }            return View();        }                public IActionResult DeletePage(int id)        {            var page = _context.CMS.FirstOrDefault(m => m.Id == id);            _context.CMS.Remove(page);            _context.SaveChanges();            TempData["successMessage"] = "Page Deleted Successfully.";            TempData.Keep();            return RedirectToAction("Index");        }

    }
}

