using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BabyStore.Models;
using BabyStore.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BabyStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly BabyStoreContext _context;

        public HomeController(BabyStoreContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            var orderLines = _context.OrderLine.Include(p => p.Product).AsQueryable();
            //foreach (var item in orderLines)
            //{
            //    Debug.WriteLine(item.Product.Name + "HHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHHUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUUU");
            //}
            var topSellers = (from topProducts in _context.OrderLine
                              where (topProducts.ProductID != null)
                              group topProducts by topProducts.Product into topGroup
                              select new BestSellersViewModel
                              {
                                  Product = topGroup.Key,
                                  SalesCount = topGroup.Sum(q=>Convert.ToInt32(q.Quantity)),
                                  ProductImage = _context.ProductImageMapping.Include(p=>p.ProductImage).Where(p=>p.Product==topGroup.Key).OrderBy(pim => pim.ImageNumber).FirstOrDefault().ProductImage.FileName
                              }).OrderByDescending(tg => tg.SalesCount).Take(4);


            
            return View(await topSellers.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult Privacy(string name)
        //{
        //    ViewData["Message"] = "Hello " + name;
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
