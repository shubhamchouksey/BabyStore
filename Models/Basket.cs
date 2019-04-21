using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace BabyStore.Models
{
    public class Basket
    {
        private string BasketID { get; set; }
        private const string BasketSessionKey = "BasketID";
        private readonly BabyStoreContext _context;
        private HttpContext _httpContext => new HttpContextAccessor().HttpContext;
        public Basket() { }
        public Basket(BabyStoreContext context)
        {
            _context = context;
        }


        private string GetBasketId()
        {
            if (_httpContext.Session.GetString(BasketSessionKey) == null)
            {
                if (!string.IsNullOrWhiteSpace(_httpContext.User.Identity.Name))
                {
                    _httpContext.Session.SetString(BasketSessionKey, _httpContext.User.Identity.Name);
                    Debug.WriteLine("HUHIUHIHIHIOHHIOHHHLKHLKHKLHKLHLKHLKKLKLHKLHKLHKLHLKLJLKHKLHLK");
                }
                else
                {
                    Guid tempBasketID = Guid.NewGuid();
                    _httpContext.Session.SetString(BasketSessionKey, tempBasketID.ToString());
                }
            }
            return _httpContext.Session.GetString(BasketSessionKey).ToString();
        }

        public static Basket GetBasket(BabyStoreContext context)
        {
            Basket basket = new Basket(context);
            basket.BasketID = basket.GetBasketId();
            Debug.WriteLine(basket.BasketID + "====================================================================");
            return basket;
        }
        public void AddToBasket(int productId, int quantity)
        {
            Debug.WriteLine(BasketID + "=======================AAAAAAAAAAAAAAAAAAAAAAAAAAAAA============");
            var basketLine = _context.BasketLine.FirstOrDefault(b => b.BasketId == BasketID && b.ProductId == productId);

            if (basketLine == null)
            {
                basketLine = new BasketLine
                {
                    ProductId = productId,
                    BasketId = BasketID,
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };
                _context.Add(basketLine);
            }
            else
            {
                basketLine.Quantity += quantity;
            }
            _context.SaveChanges();
        }
        public void RemoveLine(int productID)
        {
            var basketLine = _context.BasketLine.FirstOrDefault(b => b.BasketId == BasketID && b.ProductId == productID);
            if (basketLine != null)
            {
                _context.BasketLine.Remove(basketLine);
            }
            _context.SaveChanges();
        }
        public void UpdateBasket(List<BasketLine> lines)
        {
            foreach (var line in lines)
            {
                var basketLine = _context.BasketLine.FirstOrDefault(b => b.BasketId == BasketID && b.ProductId == line.ProductId);
                if (basketLine != null)
                {
                    if (line.Quantity == 0)
                    {
                        RemoveLine(line.ProductId);
                    }
                    else
                    {
                        basketLine.Quantity = line.Quantity;
                    }
                }
            }
            _context.SaveChanges();
        }
        public void EmptyBasket()
        {
            var basketLines = _context.BasketLine.Where(b => b.BasketId == BasketID);
            foreach (var basketLine in basketLines)
            {
                _context.BasketLine.Remove(basketLine);
            }
            _context.SaveChanges();
        }
        public List<BasketLine> GetBasketLines()
        {
            return _context.BasketLine.Include(p => p.Product).ThenInclude(p => p.ProductImageMappings).Where(b => b.BasketId == BasketID).ToList();
        }
        public decimal GetTotalCost()
        {
            decimal basketTotal = decimal.Zero;
            if (GetBasketLines().Count > 0)
            {
                basketTotal = _context.BasketLine.Where(b => b.BasketId == BasketID).Sum(b => b.Product.Price * b.Quantity);
            }
            return basketTotal;
        }
        public int GetNumberOfItems()
        {
            int numberOfItems = 0;
            if (GetBasketLines().Count > 0)
            {
                numberOfItems = _context.BasketLine.Where(b => b.BasketId == BasketID).Sum(b => b.Quantity);
            }
            return numberOfItems;
        }
        public void MigrateBasket(string userName)
        {
            //find the current basket and store it in memory using ToList()           
            var basket = _context.BasketLine.Where(b => b.BasketId == BasketID).ToList();
            //find if the user already has a basket or not and store it in memory using //ToList()  
            var usersBasket = _context.BasketLine.Where(b => b.BasketId == userName).ToList();
            //if the user has a basket then add the current items to it        
            if (usersBasket != null)
            {
                //set the basketID to the username    
                string prevID = BasketID;
                BasketID = userName;
                //add the lines in anonymous basket to the user's basket         
                foreach (var line in basket)
                {
                    AddToBasket(line.ProductId, line.Quantity);
                }
                //delete the lines in the anonymous basket from the database 
                BasketID = prevID;
                EmptyBasket();
            }
            else
            {
                //if the user does not have a basket then just migrate this one       
                foreach (var basketLine in basket)
                {
                    basketLine.BasketId = userName;
                }
                _context.SaveChanges();
            }
            _httpContext.Session.SetString(BasketSessionKey, userName);
        }

        public decimal CreateOrderLines(int orderID)
        {
            decimal orderTotal = 0;
            var basketLines = GetBasketLines();
            foreach (var item in basketLines)
            {
                OrderLine orderLine = new OrderLine
                {
                    Product = item.Product,
                    ProductID = item.ProductId,
                    ProductName = item.Product.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.Product.Price, OrderID = orderID
                };
                orderTotal += (item.Quantity * item.Product.Price);
                _context.OrderLine.Add(orderLine);
            }
            _context.SaveChanges();
            EmptyBasket();
            return orderTotal;
        }

    }
}