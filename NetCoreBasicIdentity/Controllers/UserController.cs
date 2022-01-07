using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetCoreBasicIdentity.Context;
using NetCoreBasicIdentity.Entities;
using NetCoreBasicIdentity.Models;

namespace NetCoreBasicIdentity.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly PContext _context;

        public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, PContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {

            List<AppUser> filterUsers = new List<AppUser>();
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains("Admin"))
                {
                    filterUsers.Add(user);
                }

                return View(filterUsers);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return View(new UserAdminAddModel());
        }

        [HttpPost]
        public async Task<IActionResult> Add(UserAdminAddModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    Gender = model.Gender
                };
                var result = await _userManager.CreateAsync(user, model.UserName + "1!");
                if (result.Succeeded)
                {
                    var memberRole = await _roleManager.FindByNameAsync("Member");
                    if (memberRole == null)
                    {
                        await _roleManager.CreateAsync(new()
                        {
                            Name = "Member",
                            CreatedDate = DateTime.Now
                        });
                    }


                    await _userManager.AddToRoleAsync(user, "Member");
                    return RedirectToAction("Index");
                }

                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("", err.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AssignRole(int id)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == id);
            var userRoles = await _userManager.GetRolesAsync(user);
            var roles = _roleManager.Roles.ToList();

            RoleAssignSendModel model = new RoleAssignSendModel();
            List<RoleAssignListModel> listModel = new List<RoleAssignListModel>();
            foreach (var role in roles)
            {
                listModel.Add(new()
                {
                    Name = role.Name,
                    RoleId = role.Id,
                    Exist = userRoles.Contains(role.Name)
                });
            }

            model.Roles = listModel;
            model.UserId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole(RoleAssignSendModel model)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.Id == model.UserId);
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (role.Exist)
                {
                    if (!userRoles.Contains((role.Name)))
                    {
                        await _userManager.AddToRoleAsync(user, role.Name);
                    }
                }
                else
                {
                    if (userRoles.Contains(role.Name))
                    {
                        await _userManager.RemoveFromRoleAsync(user, role.Name);
                    }
                }
            }
            return RedirectToAction("Index");
        }
    }
}