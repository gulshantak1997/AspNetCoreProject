using System;
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




        public IActionResult TreeViewFinal(Guid RoleId)        {            List<PermissionViewModel> lstPermission = new List<PermissionViewModel>();            List<Final> obj = new List<Final>();            var RolesList = (from roles in _context.Roles                             select new SelectListItem                             {                                 Value = roles.Id,                                 Text = roles.Name                             }).ToList();            ViewBag.RolesList = RolesList;

            var permission = from per in _context.MenuPermissions where (per.RoleId == RoleId) select per.MenuId;
            var per1 = string.Join(",", permission);

            ViewBag.Per = per1;
            var result = (from Menus in _context.MenuItems                          join Permissions in _context.MenuPermissions.Where(r => r.RoleId == RoleId)                          on Menus.Id equals Permissions.MenuId into menuPerm                          from perm in menuPerm.DefaultIfEmpty()                          select new PermissionViewModel                          {                              Id = Menus.Id,                              Path = Menus.Path,                              Name = Menus.Name,                              ParentId = Menus.ParentId,                              MenuLevel = Menus.MenuLevel,                              HasAccess = perm == null ? false : !(string.IsNullOrEmpty(Convert.ToString(perm.RoleId)))                          }).ToList();


          

            // Loop and add the Parent Nodes.
            foreach (var type in result.Where(p => p.ParentId == 0))            {                List<Child> ch = new List<Child>();                foreach (var subChild in result.Where(x => x.ParentId == type.Id))                {                    ch.Add(new Child { id = subChild.Id, title = "'" + subChild.Name + "'", Checked=subChild.HasAccess });                }                obj.Add(new Final { id = type.Id, title = "'" + type.Name + "'", pId = type.ParentId, subs = ch, Checked=type.HasAccess });            }

            //Serialize to JSON string.

            var items = JsonConvert.SerializeObject(obj);            ViewBag.Json = items.Replace('"', ' ');


            return View(obj);        }

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