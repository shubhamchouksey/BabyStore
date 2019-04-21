using BabyStore.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using X.PagedList;

namespace BabyStore.ViewModel
{
    public class ProductIndexViewModel
    {
        public IPagedList<Product> Products { get; set; }
        public string Search { get; set; }
        public string SortBy;
        public Dictionary<string, string> Sorts;
        public IEnumerable<CategoryWithCount> CatsWithCount { get; set; }
        public string Category { get; set; }
        public IEnumerable<SelectListItem> CatFilterItems
        {
            get
            {
                var allCats = CatsWithCount.Select(cc => new SelectListItem
                {
                    Value = cc.CategoryName,
                    Text = cc.CatNameWithCount
                });
                    return allCats;
            }
        }
    }
    public class CategoryWithCount
    {
        public int ProductCount { get; set; }
        public string CategoryName { get; set; }
        public string CatNameWithCount
        {
            get
            {
                return CategoryName + " (" + ProductCount.ToString() + ")";
            }
        }
    }
}
