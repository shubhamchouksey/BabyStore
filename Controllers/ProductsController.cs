using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BabyStore.Models;
using BabyStore.ViewModel;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace BabyStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly BabyStoreContext _context;

        public ProductsController(BabyStoreContext context)
        {
            _context = context;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index(string category, string search, string sortBy, int? pageNumber)
        {
            //instantiate a new view model    
            ProductIndexViewModel viewModel = new ProductIndexViewModel();    //select the products    
            var products = _context.Product.Include(p => p.Category).Include(p => p.ProductImageMappings).AsQueryable();
            //perform the search and save the search string to the viewModel    
            if (!String.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.Contains(search) || p.Description.Contains(search) || p.Category.Name.Contains(search));
                viewModel.Search = search;                      //we have to save Search propery for next page
            }
            //group search results into categories and count how many items in each category    
            viewModel.CatsWithCount = from matchingProducts in products
                                      where
                                      matchingProducts.CategoryID != null
                                      group matchingProducts by
                                      matchingProducts.Category.Name into
                                      catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };
            if (!String.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category.Name == category);
                viewModel.Category = category;                      //we have to save Category propery for next page
            }
            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Price);
                    break;
            }
            int currentPage = pageNumber ?? 1;                    //if page=null then 1 else page value passed by argument
            viewModel.Products = products.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;                     //we have to save sortBy propery for next page
            viewModel.Sorts = new Dictionary<string, string>     //"key","value" pair
            {
                {"Price low to high" ,"price_lowest"},
                {"Price high to low", "price_highest" }
            };

            ViewData["P"] = _context.ProductImageMapping.Include(p => p.ProductImage).ToList();
            return View(viewModel);

        }
        // GET: Products/Details/5
            [AllowAnonymous]
            public async Task<IActionResult> Details(int? id)
            {
            if (id == null)
            {
                return NotFound();  
            }

            var product = await _context.Product
                .Include(p => p.Category).Include(p=>p.ProductImageMappings)
                .FirstOrDefaultAsync(m => m.ID == id);

            ViewData["P"] = _context.ProductImageMapping.Include(p => p.ProductImage).Where(p => p.ProductID == id).ToList();
            //ViewData["P"] = _context.ProductImageMapping.Include(p=>p.ProductImage).Where(p=>p.ProductID==id).Single().ProductImage.FileName;
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(_context.Set<Category>(), "ID", "Name");      //selectlist(System.Collections.IEnumerable items, string dataValueField, string dataTextField);
            viewModel.ImageLists = new List<SelectList>();
            for(int i=0;i<Constants.NumberOfProductImages;i++)
            {
                viewModel.ImageLists.Add(new SelectList(_context.Set<ProductImage>(), "ID", "FileName"));
            }
            return View(viewModel);
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel viewModel)
        {
            Product product = new Product();
            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Price = viewModel.Price;
            product.CategoryID = viewModel.CategoryID;
            product.ProductImageMappings = new List<ProductImageMapping>();
            //get a list of selected images without any blanks    
            string[] productImages = viewModel.ProductImages.Where(pi =>!string.IsNullOrEmpty(pi)).ToArray();
            for (int i = 0; i < productImages.Length; i++)
            {
                product.ProductImageMappings.Add(new ProductImageMapping
                {
                    ProductImage = _context.ProductImage.Find(int.Parse(productImages[i])),
                    ImageNumber = i
                });

            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            viewModel.CategoryList = new SelectList(_context.Set<Category>(), "ID", "Name", product.CategoryID);
            viewModel.ImageLists = new List<SelectList>();
            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(_context.Set<ProductImage>(), "ID", "FileName",viewModel.ProductImages[i]));
            }
            return View(viewModel);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel viewModel = new ProductViewModel();
            viewModel.CategoryList = new SelectList(_context.Set<Category>(), "ID", "Name",product.CategoryID);      //selectlist(System.Collections.IEnumerable items, string dataValueField, string dataTextField);
            viewModel.ImageLists = new List<SelectList>();
            foreach (var imageMapping in _context.ProductImageMapping.Include("ProductImage").Where(p=>p.ProductID==id).OrderBy(pim => pim.ImageNumber))
            {
                viewModel.ImageLists.Add(new SelectList(_context.Set<ProductImage>(), "ID", "FileName", imageMapping.ProductImageID));
            }
            for (int i = viewModel.ImageLists.Count; i < Constants.NumberOfProductImages; i++)
            {
                viewModel.ImageLists.Add(new SelectList(_context.Set<ProductImage>(), "ID", "FileName"));
            }
            viewModel.ID = product.ID;
            viewModel.Name = product.Name;
            viewModel.Price = product.Price;
            viewModel.Description = product.Description;
            return View(viewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel viewModel) //This viewModel is a model when you click the edit button on edit view and HttpPost form will be called and data to be captured by viewModel
        {
            var productToUpdate = _context.Product.Include(p => p.ProductImageMappings).Where(p => p.ID == viewModel.ID).Single();
            if (await TryUpdateModelAsync(productToUpdate, "",s=>s.Name,s=>s.Description,s=>s.Price,s=>s.CategoryID))
            {
                if(productToUpdate.ProductImageMappings == null)
                {
                    productToUpdate.ProductImageMappings = new List<ProductImageMapping>();
                }
                string[] productImages = viewModel.ProductImages.Where(pi => !string.IsNullOrEmpty(pi)).ToArray();
                for(int i=0;i<productImages.Length;i++)
                {
                    //get the image currently stored    
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim .ImageNumber == i).FirstOrDefault();
                    //find the new image    
                    var image = _context.ProductImage.Find(int.Parse(productImages[i]));
                    if (imageMappingToEdit == null)
                    {
                        //add image to the imagemappings    
                        productToUpdate.ProductImageMappings.Add(new ProductImageMapping
                        {
                            ImageNumber = i,
                            ProductImage = image,
                            ProductImageID = image.ID
                        });
                    }
                    //else it's not a new file so edit the current mapping    
                    else
                    {
                        if (imageMappingToEdit.ProductImageID != int.Parse(productImages[i])) //if the image is not same as previously in the ProductImageMapping database
                        {
                            //assign image property of the image mapping
                            imageMappingToEdit.ProductImage = image;
                        }
                    }
                }

                for (int i = productImages.Length; i < Constants.NumberOfProductImages; i++)
                {
                    var imageMappingToEdit = productToUpdate.ProductImageMappings.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                    //if there is something stored in the mapping     
                    if (imageMappingToEdit != null)
                    {
                        //delete the record from the mapping table directly. 
                        //just calling productToUpdate.ProductImageMappings.Remove(imageMappingToEdit)                  
                        //results in a FK error            
                        _context.ProductImageMapping.Remove(imageMappingToEdit);
                    }
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            var orderLines = _context.OrderLine.Where(ol => ol.ProductID == id);
            foreach (var ol in orderLines)
            {
                ol.ProductID = null;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ID == id);
        }
    }
}
