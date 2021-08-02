using System;
using System.Linq;
using System.Threading.Tasks;
using AdminLTE1.Helper;
using AdminLTE1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminLTE1.Controllers
{
    [AuthorizeActionFilter]
    public class RoleController : Controller
    {
        private readonly AppDbContext _context;

        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager, AppDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        [HttpPost]        public async Task<IActionResult> DeleteRole(string id)        {            var role = await _roleManager.FindByIdAsync(id);            if (role == null)            {                ViewBag.ErrorMessage = $"Role with Id = {id} cannot be found";                return View("NotFound");            }            else            {                try                {                    var permission = _context.MenuPermissions.Where(m => m.RoleId == new Guid(id));                    foreach (var perm in permission)                    {                        _context.MenuPermissions.Remove(perm);                    }                    var perms = _context.MenuPermissions.FirstOrDefault(m => m.RoleId == new Guid(id));                    _context.MenuPermissions.Remove(perms);                    _context.SaveChanges();

                    var result = await _roleManager.DeleteAsync(role);                    if (result.Succeeded)                    {                        return RedirectToAction("Index");                    }                    foreach (var error in result.Errors)                    {                        ModelState.AddModelError("", error.Description);                    }                    return View("Index");                }                catch (DbUpdateException ex)                {
                    //logger.LogError($"Error deleting role {ex}");
                    ViewBag.ErrorTitle = $"{role.Name} role is in use";                    ViewBag.ErrorMessage = $"{role.Name} role cannot be deleted as there are users " +                    $"in this role. If you want to delete this role, please remove users from " +                    $"the role and then try to delete.";                    return View("Error");                }            }        }
    }
}