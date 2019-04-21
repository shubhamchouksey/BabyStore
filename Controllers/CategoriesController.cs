using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BabyStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace BabyStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly BabyStoreContext _context;

        public CategoriesController(BabyStoreContext context)
        {
            _context = context;
        }

        // GET: Categories
        [AllowAnonymous]                                   //So that anyone can view the list of categories without logging in
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.OrderBy(c=>c.Name).ToListAsync());
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.ID == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id,byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Name", "RowVersion" };
            if (id == null)
            {
                return NotFound();
            }
            var categoryToUpdate = await _context.Category.FindAsync(id);
            if(categoryToUpdate==null)
            {
                Category deletedCategory = new Category();
                await TryUpdateModelAsync(deletedCategory,"",s=>s.ID,s=>s.Name,s=>s.RowVersion);
                ModelState.AddModelError(string.Empty, "Unable to save your changes because the " +
                    "category has been deleted by another user.");
                return View(deletedCategory);
            }
            if (await TryUpdateModelAsync(categoryToUpdate,"",s=>s.ID,s=>s.Name,s=>s.RowVersion))
            {
                try
                {
                    _context.Entry(categoryToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exEntry = ex.Entries.Single();
                    var currentUIValues = (Category)exEntry.Entity;
                    var databaseCategory = exEntry.GetDatabaseValues();
                    if (databaseCategory == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save your changes because " +
                            "the category has been deleted by another user.");
                    }
                    else
                    {
                        var databaseCategoryValues = (Category)databaseCategory.ToObject();
                        if (databaseCategoryValues.Name != currentUIValues.Name)
                        {
                            ModelState.AddModelError("Name", "Current value in database: " + databaseCategoryValues.Name);
                        }
                        ModelState.AddModelError(string.Empty, "The record has been modified by  " +
                            "another user after you loaded the screen. Your changes have not yet been saved. " + 
                            "The new values in the database are shown below. If you want to overwrite these values" +
                            " with your changes then click save otherwise go back to the categories page.");
                        categoryToUpdate.RowVersion = (byte[])databaseCategoryValues.RowVersion;
                        ModelState.Remove("RowVersion");   //This is must because ModelState has the old RowVersion value. In the Razor Page the ModelState value for a field takes precedence over the model property values when bot are present
                    }
                }
            }
            return View(categoryToUpdate);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id,bool? deletionError)
        {
            if (id == null)
            {
                return NotFound();
            }

            string query = "Select * from Category where ID=@p0";
            var category = await _context.Category.FromSql(query, id).SingleOrDefaultAsync();
                
            if (category == null)
            {
                if (deletionError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return NotFound();
            }
            if(deletionError.GetValueOrDefault())
            {
                ModelState.AddModelError(string.Empty, "The category you attempted to delete has been " +
                    "modified by another user after you loaded it. " + "The delete has not been" +
                    " performed. The current values in the database are shown above. " + "If you still " +
                    "want to delete this record click the Delete button again, otherwise go back to the categories page.");
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Category category)
        {
            
            try
            {
                _context.Entry(category).State = EntityState.Deleted;
                    var products = await _context.Product
                 .Where(d => d.CategoryID == category.ID)
                .ToListAsync();
                products.ForEach(d => d.CategoryID = null);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction("Delete", new { deletionError = true, id = category.ID });
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.ID == id);
        }
    }
}
