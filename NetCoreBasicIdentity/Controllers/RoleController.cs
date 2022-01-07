using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreBasicIdentity.Entities;
using NetCoreBasicIdentity.Models;

namespace NetCoreBasicIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleController(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new RoleAddModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(RoleAddModel model)
        {
            if (ModelState.IsValid)
            {
               var result = await _roleManager.CreateAsync(new AppRole
                {
                    Name = model.Name,
                    CreatedDate = DateTime.Now
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }
            return View(model);
        }
    }
}