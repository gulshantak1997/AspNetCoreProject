﻿using System;
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
        private readonly AppDbContext _context;

        public CMSController(AppDbContext context, IHostingEnvironment hostEnvironment)

        public IActionResult Index()

            List<CMS> lstmodel = new List<CMS>();

            return View(lstmodel);




                if (model.BannerImageFile!= null)
                    var fileStream = new FileStream(path, FileMode.Create);
                _context.CMS.Add(cmsModel);
                            //var mem = new MemoryStream();
                            //user.ProfilePicture
                            var fileStream = new FileStream(path, FileMode.Create);
                            //var mem = new MemoryStream();
                            //user.ProfilePicture
                            var fileStream = new FileStream(path, FileMode.Create);

    }
}
