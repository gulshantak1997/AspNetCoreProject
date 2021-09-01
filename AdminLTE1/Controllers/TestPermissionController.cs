﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE1.Entities;
using AdminLTE1.Helper;
using AdminLTE1.Models;
using AdminLTE1.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AdminLTE1.Controllers
{
    public class TestPermissionController : Controller
    {
        private readonly AppDbContext _context;
        public TestPermissionController(AppDbContext context)
        {
            this._context = context;
        }


        //public IActionResult Index()
        //{
        //    List<TreeViewNode> nodes = new List<TreeViewNode>();

        //    foreach (var type in _context./*State */MenuItems.Where(a => a.ParentId == 0))
        //    {
        //        nodes.Add(new TreeViewNode { id = type./*StateId*/Id.ToString(), parent = "#", text = type./*StateName*/ Name });
        //    }

        //    foreach (var subtype in _context./*City*/ MenuItems.Where(a => a.ParentId != 0))
        //    {
        //        nodes.Add(new TreeViewNode { id = subtype./*StateId*/ ParentId.ToString() + "-" + subtype./*CityId*/ Id.ToString(), parent = subtype./*StateId*/ ParentId.ToString(), text = subtype./*CityName*/ Name });
        //    }


        //    ViewBag.Json = JsonConvert.SerializeObject(nodes);

        //    return View();
        //}



        //[HttpPost]
        //public ActionResult Index(string selectedItems)
        //{
        //    List<TreeViewNode> items = JsonConvert.DeserializeObject<List<TreeViewNode>>(selectedItems);

        //    return RedirectToAction("Index");
        //}




        public IActionResult TreeViewFinal(Guid RoleId)

            var permission = from per in _context.MenuPermissions where (per.RoleId == RoleId) select per.MenuId;
            var per1 = string.Join(",", permission);

            ViewBag.Per = per1;



          

            // Loop and add the Parent Nodes.
            foreach (var type in result.Where(p => p.ParentId == 0))

            //Serialize to JSON string.

            var items = JsonConvert.SerializeObject(obj);


            return View(obj);

        [HttpPost]
        public JsonResult UpdatePermission([FromBody]UpdatePermissionViewModel[] model)
        {

            if (model != null)
            {

                int count = 0;
                foreach (var item in model)
                {
                    if (count == 0)
                    {
                        var existing = _context.MenuPermissions.Where(R => R.RoleId == item.RoleId);

                        foreach (var roles in existing)
                        {
                            _context.MenuPermissions.Remove(roles);
                        }
                        count++;
                    }
                    MenuPermission mnp = new MenuPermission();
                    mnp.MenuId = item.MenuId;
                    mnp.PermissionId = Guid.NewGuid();
                    mnp.RoleId = item.RoleId;
                    _context.MenuPermissions.Add(mnp);
                }

                _context.SaveChanges();

                return Json("Saved Successfully!");
            }
            return Json("NoData");
        }

    }
}