using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BabyStore.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System.Data.SqlClient;

namespace BabyStore.Controllers
{
    public class ProductImagesController : Controller
    {
        private readonly BabyStoreContext _context;

        public ProductImagesController(BabyStoreContext context)
        {
            _context = context;
        }

        private bool ValidateFile(IFormFile file)
        {
            string fileExtension = Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".bmp", ".png", ".jpeg", ".jpg" };
            if ((file.Length > 0 && file.Length < 2097152) && allowedFileTypes.Contains(fileExtension))     //One point to note is that rather than loop through the allowedFileTypes array, we used the LINQ contains operator to shorten the amount of code needed
            {
                return true;
            }
            return false;
        }

        
        private void SaveFileToDiskAsync(IFormFile file)
        {
                using (Image<Rgba32> image = SixLabors.ImageSharp.Image.Load(file.FileName))     //Rgba32 is our default PixelFormat
                {
                    if (image.Width > 190)
                    {
                        image.Mutate(x => x                             //try to change the form of an image
                        .Resize(190, image.Height)
                        .Grayscale());
                    }
                    image.Save(Constants.ProductImagePath + Path.GetFileName(file.FileName)); // Automatic encoder selected based on extension.

                    if (image.Width > 100)
                    {
                        image.Mutate(x => x
                        .Resize(100, image.Height)
                        .Grayscale());
                    }
                    image.Save(Constants.ProductThumbnailPath + Path.GetFileName(file.FileName)); // Automatic encoder selected based on extension.
              
            }
        }

        // GET: ProductImages
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductImage.ToListAsync());
        }

        // GET: ProductImages/Upload
        public IActionResult Upload()
        {
            return View();
        }

        // POST: ProductImages/Upload
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile[] files)
        {
            bool allValid = true;
            string inValidFiles = "";
            //check the user has entered a file    
            if (files.Length!=0)
            {
                //if the user has entered less than 10 files
                if (files.Length <= 10)
                {
                    foreach (var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles += ", " + Path.GetFileName(file.FileName);
                        }
                    }
                    //if they are valid then try to save them to disk
                    if (allValid)
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                SaveFileToDiskAsync(file);
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("FileName", "Sorry an error occurred saving the file to disk, please try again");
                            }
                        }
                    }
                    //else add an error listing out the invalid files
                    else
                    {
                        ModelState.AddModelError("FileName", "All files must be gif, png, jpeg or jpg and less than 2MB in size.The following files" + inValidFiles + " are not valid");
                    }
                }
                //the user has entered more than 10 files
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to ten files at a time");
                }
            }
            else
            {
                //if the user has not entered a file return an error message    
                ModelState.AddModelError("FileName", "Please choose a file");
            } 

            if (ModelState.IsValid)
            {
                bool duplicates = false;
                bool otherDbError = false;
                string duplicateFiles = "";

                 foreach (var file in files)
                 {
                    //try and save each file
                    var productToAdd = new ProductImage { FileName = Path.GetFileName(file.FileName) };
                    try
                    {
                        _context.Add(productToAdd);
                        await _context.SaveChangesAsync();
                    }
                    //if there is an exception check if it is caused by duplicate file
                    catch (DbUpdateException ex)
                    {
                        SqlException innerException = ex.InnerException as SqlException;
                        if (innerException!=null && innerException.Number == 2601)
                        {
                            duplicateFiles += "," + Path.GetFileName(file.FileName);
                            duplicates = true;
                            _context.Entry(productToAdd).State = EntityState.Detached;
                        }
                        else
                        {
                            otherDbError = true;
                        }
                    }
                }

                //add a list of duplicate files to the error message    
                if (duplicates)
                {
                    ModelState.AddModelError("FileName", "All files uploaded except the files" +duplicateFiles 
                        + ", which already exist in the system ." + " Please delete them and try again if you wish to re-add them");
                    return View();
                }
                else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Sorry an error has occurred saving to the database, please try again");
                    return View();
                }

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: ProductImages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productImage = await _context.ProductImage
                .FirstOrDefaultAsync(m => m.ID == id);
            if (productImage == null)
            {
                return NotFound();
            }

            return View(productImage);
        }

        // POST: ProductImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productImage = await _context.ProductImage.FindAsync(id);
            //find all the mappings for this image    
            var mappings = _context.ProductImageMapping.Where(pim => pim .ProductImageID == id);
            foreach (var mapping in mappings)
            {
                //find all mappings for any product containing this image  
                var mappingsToUpdate = _context.ProductImageMapping.Where(pim => pim .ProductID == mapping.ProductID);
                //for each image in each product change its imagenumber to one lower if it is higher         
                //than the current image   
                foreach (var mappingToUpdate in mappingsToUpdate)
                {
                    if (mappingToUpdate.ImageNumber > mapping.ImageNumber)
                    {
                        mappingToUpdate.ImageNumber--;
                    }
                }
            }
            System.IO.File.Delete(Constants.ProductImagePath+productImage.FileName);
            System.IO.File.Delete(Constants.ProductThumbnailPath+productImage.FileName);
            _context.ProductImage.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductImageExists(int id)
        {
            return _context.ProductImage.Any(e => e.ID == id);
        }
    }
}
    