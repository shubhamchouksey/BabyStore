using BabyStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BabyStore.ViewModel
{
    public class BestSellersViewModel
    {
        public Product Product { get; set; }
        public int SalesCount { get; set; }
        public string ProductImage { get; set; }
    }
}
