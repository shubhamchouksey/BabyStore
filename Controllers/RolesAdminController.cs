using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BabyStore.Data;
using BabyStore.Models;
using BabyStore.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace BabyStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesAdminController : Controller
    {
        private readonly BabyStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesAdminController(BabyStoreContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        

        // GET: RolesAdmin    
        public ActionResult Index()
        {
            List<RoleViewModel> model = new List<RoleViewModel>();
            model = _roleManager.Roles.Select(r => new RoleViewModel
            {
                Name = r.Name,
                Id = r.Id
            }).ToList();
            return View(model);
        } 

        // GET: RolesAdmin/Details/5
        public async Task<ActionResult> Details(string id)
        {
           if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var role = await _roleManager.FindByIdAsync(id);
            // Get the list of Users in this Role   
            var users = new List<IdentityUser>();
            // Get the list of Users in this Role    
            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    users.Add(user);
                }
            }
            ViewBag.Users = users;
            ViewBag.UserCount = users.Count();
            RoleViewModel p = new RoleViewModel();
            return View(p); 

        }

        // GET: RolesAdmin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RolesAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(roleViewModel.Name);
                var roleresult = await _roleManager.CreateAsync(role);
                if (!roleresult.Succeeded)
                {
                    ModelState.AddModelError("", roleresult.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: RolesAdmin/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if(role == null)
            {
                return NotFound();
            }
            RoleViewModel roleModel = new RoleViewModel { Id = role.Id, Name = role.Name };
            return View(roleModel);

        }

        // POST: RolesAdmin/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(roleViewModel.Id);
                role.Name = roleViewModel.Name;
                await _roleManager.UpdateAsync(role);
                return RedirectToAction("Index");
            }
            return View();  
        }

        // GET: RolesAdmin/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            RoleViewModel roleViewModel = new RoleViewModel { Id = role.Id, Name  = role.Name};
            return View(roleViewModel);
        }

        // POST: RolesAdmin/Delete/5
        [HttpPost,ActionName("Delete")]     //The method has been renamed DeleteConfirmed because if the name was kept as Delete , it would have the same signature as the GET version of the Delete method 
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    return NotFound();
                }
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First().ToString());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}