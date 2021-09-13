using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AdminLTE1.Data;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace AdminLTE1.Controllers
{
    public class BaseController : Controller
    { 
        public BaseController()        {
            //System.Globalization.CultureInfo customCulture = new System.Globalization.CultureInfo("en-US", true);
            //customCulture.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy h:mm tt";
            //string shortUsDateFormatString = customCulture.DateTimeFormat.ShortDatePattern;
            //string shortUsTimeFormatString = customCulture.DateTimeFormat.ShortTimePattern;

            //Thread.CurrentThread.CurrentCulture = customCulture;


            CultureInfo cultureInfo = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            cultureInfo.DateTimeFormat.ShortDatePattern = "MM/dd/yyyy";
            //cultureInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cultureInfo.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = cultureInfo;
        }
       
    }
}

