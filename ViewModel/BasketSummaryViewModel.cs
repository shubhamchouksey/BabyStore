using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq;
using System.Threading.Tasks;

namespace BabyStore.ViewModel
{
    public class BasketSummaryViewModel
    {
        public int NumberOfItems { get; set; }
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:c}")]
        public decimal TotalCost { get; set; }
    }
}
