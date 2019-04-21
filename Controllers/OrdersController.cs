using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BabyStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using BabyStore.Data;
using System.Diagnostics;
using BabyStore.Utilities;

namespace BabyStore.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly BabyStoreContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OrdersController(BabyStoreContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Orders
        public async Task<IActionResult> Index(string orderSearch, string startDate, string endDate,string orderSortOrder,int? pageNumber)
        {
            var orders =_context.Order.OrderBy(o => o.DateCreated).Include(o => o.OrderLines).Include(da=>da.DeliveryAddress).AsQueryable();
            if (!User.IsInRole("Admin"))
            {
                orders = orders.Where(o => o.UserID == User.Identity.Name);
            }
            if (!String.IsNullOrEmpty(orderSearch))
            {
                orders = orders.Where(o => o.OrderID.ToString().Equals(orderSearch) || 
                o.UserID.Contains(orderSearch) || 
                o.DeliveryName.Contains(orderSearch) || 
                o.DeliveryAddress.AddressLine1.Contains(orderSearch) ||
                o.DeliveryAddress.AddressLine2.Contains(orderSearch) ||
                o.DeliveryAddress.Town.Contains(orderSearch) ||
                o.DeliveryAddress.County.Contains(orderSearch) || 
                o.DeliveryAddress.Postcode.Contains(orderSearch) || 
                o.TotalPrice.ToString().Equals(orderSearch) ||
                o.OrderLines.Any(ol => ol.ProductName.Contains(orderSearch)));
            }
            DateTime parsedStartDate;
            if (DateTime.TryParse(startDate, out parsedStartDate))
            {
                orders = orders.Where(o => o.DateCreated >= parsedStartDate);
            }
            DateTime parsedEndDate;
            if (DateTime.TryParse(endDate, out parsedEndDate))
            {
                orders = orders.Where(o => o.DateCreated <= parsedEndDate);
            }
            ViewBag.DateSort = String.IsNullOrEmpty(orderSortOrder) ? "date" : "";
            ViewBag.UserSort = orderSortOrder == "user" ? "user_desc" : "user";
            ViewBag.PriceSort = orderSortOrder == "price" ? "price_desc" : "price";
            ViewBag.CurrentOrderSearch = orderSearch;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            switch (orderSortOrder)
            {
                case "user":
                    orders = orders.OrderBy(o => o.UserID);
                    break;
                case "user_desc":
                    orders = orders.OrderByDescending(o => o.UserID);
                    break;
                case "price":
                    orders = orders.OrderBy(o => o.TotalPrice);
                    break;
                case "price_desc":
                    orders = orders.OrderByDescending(o => o.TotalPrice);
                    break;
                case "date":
                    orders = orders.OrderBy(o => o.DateCreated);
                    break;
                default:
                    orders = orders.OrderByDescending(o => o.DateCreated);
                    break;
            }

            int currentPage = (pageNumber ?? 1);
            //var currentPageOfOrders = await AsyncPaging.ReturnPages(orders, currentPage, Constants.PageItems);    This is simple without using extension method scenario when IQuerable<T> not used with prefix 'this' keyword
            ViewBag.CurrentPage = currentPage;
            ViewBag.TotalPages = (int)Math.Ceiling((decimal)orders.Count() / Constants.PageItems);
            var currentPageOfOrders = await orders.ReturnPages(currentPage, Constants.PageItems);    //this is new with extension method
            ViewBag.CurrentSortOrder = orderSortOrder;
            return View(currentPageOfOrders); 

        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Order.Include(o=>o.OrderLines).Include(a=>a.DeliveryAddress)
                .FirstOrDefaultAsync(m => m.OrderID == id);
            if (order == null)
            {
                return NotFound();
            }
            if(order.UserID==User.Identity.Name || User.IsInRole("Admin"))
            {
                return View(order);
            }
            else
            {
                return Unauthorized();
            }
            
        }

        // GET: Orders/Create
        public async Task<IActionResult> Review()
        {
            Basket basket = Basket.GetBasket(_context);
            Order order = new Order();
            order.UserID = User.Identity.Name;
            ApplicationUser user = await _context.Users.Include(a => a.Address).FirstOrDefaultAsync(m => m.UserName == order.UserID);
            order.DeliveryName = user.FirstName + "" + user.LastName;
            order.DeliveryAddress = user.Address;
            order.OrderLines = new List<OrderLine>();
            foreach (var basketLine in basket.GetBasketLines())
            {
                OrderLine line = new OrderLine
                {
                    Product = basketLine.Product,
                    ProductID = basketLine.ProductId,
                    ProductName = basketLine.Product.Name,
                    Quantity = basketLine.Quantity,
                    UnitPrice = basketLine.Product.Price
                };
                Debug.WriteLine(line.ProductName + "GGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGGgg+");
                order.OrderLines.Add(line);
            }
            order.TotalPrice = basket.GetTotalCost();
            return View(order);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserID,DeliveryName,DeliveryAddress")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.DateCreated = DateTime.Now;
                _context.Order.Add(order);
                _context.SaveChanges();
                //add the orderlines to the database after creating the order     
                Basket basket = Basket.GetBasket(_context);
                order.TotalPrice = basket.CreateOrderLines(order.OrderID);
                _context.SaveChanges();
                return RedirectToAction("Details", new { id = order.OrderID }); 
            }
            return RedirectToAction("Review");
        }

    }
}
