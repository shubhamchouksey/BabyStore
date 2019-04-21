using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BabyStore.Data;
using BabyStore.Models;
using BabyStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BabyStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersAdminController : Controller
    {
        private readonly BabyStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UsersAdminController(BabyStoreContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: UsersAdmin
        public async Task<ActionResult> Index()
        {
            List<ApplicationUser> model = new List<ApplicationUser>();
            model = await _userManager.Users.Select(r => new ApplicationUser
            {
                UserName = r.UserName,
                Id = r.Id
            }).ToListAsync();
            return View(model);
        }

        // GET: UsersAdmin/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var user = await _context.Users.Include(a => a.Address).FirstOrDefaultAsync(m => m.Id == id);
            ViewBag.RoleNames = await _userManager.GetRolesAsync(user);
            return View(user);
        }

        // GET: UsersAdmin/Create
        public async Task<ActionResult> Create()
        {
            //Get the list of Roles    
            ViewBag.RoleId = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
            return View();
        }

        // POST: UsersAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    DateOfBirth = userViewModel.DateOfBirth,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    Address = userViewModel.Address
                };
                var adminresult = await _userManager.CreateAsync(user, userViewModel.Password);
                //Add User to the selected Roles        
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await _userManager.AddToRolesAsync(user, selectedRoles);
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First().ToString());
                            ViewBag.RoleId = new SelectList(await _roleManager.Roles.ToListAsync(), "Name", "Name");
                            return View();
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First().ToString());
                    ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name");
                    return View();
                }
                return RedirectToAction("Index");
            }
            ViewBag.RoleId = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        // GET: UsersAdmin/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _context.Users.Include(a => a.Address).FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                RolesList = _roleManager.Roles.ToList().Select(x => new SelectListItem()
                {
                    Selected = userRoles.Contains(x.Name),
                    Text = x.Name,
                    Value = x.Name
                })
            });
        }

        // POST: UsersAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel editUser, params string[] selectedRole)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return NotFound();
                }
                user.DateOfBirth = editUser.DateOfBirth;
                user.FirstName = editUser.FirstName;
                user.LastName = editUser.LastName;
                user.Address = editUser.Address;
                var userRoles = await _userManager.GetRolesAsync(user);
                //foreach (var item in editUser.RolesList)
                //{
                //    Debug.WriteLine(item + "=============================================================================");
                //}

                var result = await _userManager.AddToRolesAsync(user, selectedRole.Except(userRoles).ToArray<string>());
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                result = await _userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRole).ToArray<string>());
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

    }
}