using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BabyStore.Models;
using BabyStore.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BabyStore.Controllers
{ 
    public class BasketController : Controller
    {
        private readonly BabyStoreContext _context;
        public BasketController(BabyStoreContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Basket basket = Basket.GetBasket(_context);
            BasketViewModel viewModel = new BasketViewModel
            {
                BasketLines = basket.GetBasketLines(),
                TotalCost = basket.GetTotalCost()
            };
            foreach (var item in basket.GetBasketLines())
            {
                ViewData[item.Product.Name] = _context.ProductImageMapping.Include(p => p.ProductImage).Where(p=>p.ProductID==item.ProductId).ToList();
            }
            
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddToBasket(int id, int quantity)
        {
            Basket b = new Basket(_context);
            Basket basket = Basket.GetBasket(_context);
            basket.AddToBasket(id, quantity);
            return RedirectToAction("Index");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateBasket(BasketViewModel viewModel)
        {
            Basket basket = Basket.GetBasket(_context);
            basket.UpdateBasket(viewModel.BasketLines);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult RemoveLine(int id)
        {
            Basket basket = Basket.GetBasket(_context);
            basket.RemoveLine(id);
            return RedirectToAction("Index");
        }

        public PartialViewResult Summary()
        {
            Basket basket = Basket.GetBasket(_context);
            BasketSummaryViewModel viewModel = new BasketSummaryViewModel
            {
                NumberOfItems = basket.GetNumberOfItems(),
                TotalCost = basket.GetTotalCost()
            };
            Debug.WriteLine(basket.GetTotalCost() + "RRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRRrrrr");
            return PartialView(viewModel);
        }
    }
}