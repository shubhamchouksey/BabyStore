using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BabyStore.ViewModel
{
    public class ProductViewModel                //This class is different from product class because it is used two property extra of the type SelectList that are used to hold a list of categories and images. 
    {                                        //The properties CategoryID and ProductImages will be used as the name of the SelectLists in the view      
                                              //This class uses a Duplication pattern because it duplicated the code from product class. Inheritance and composition are also patterns like Duplication
        public int ID { get; set; }
        [Required(ErrorMessage = "The product name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Please enter a product name between 3 and 50 characters in length")]
        [RegularExpression(@"^[a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please enter a product name made up of letters and numbers only")]
        public string Name { get; set; }
        [Required(ErrorMessage = "The product description cannot be blank")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Please enter a product description between 10 and 200 characters in length")]
        [RegularExpression(@"^[,;a-zA-Z0-9'-'\s]*$", ErrorMessage = "Please enter a product description made up of letters and numbers only")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [Required(ErrorMessage = "The price cannot be blank")]
        [Range(0.10, 10000, ErrorMessage = "Please enter a price between 0.10 and 10000.00")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        [RegularExpression("[0-9]+(\\.[0-9] [0-9]?)?", ErrorMessage = "The price must be a number up to two decimal places")]
        public decimal Price { get; set; }
        [Display(Name ="Category")]
        public int CategoryID { get; set; }
        public SelectList CategoryList { get; set; }
        public List<SelectList> ImageLists { get; set; }
        public string[] ProductImages { get; set; }
    }
}

