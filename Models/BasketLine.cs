using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BabyStore.Models
{
    public class BasketLine
    {
        public int Id { get; set; }
        public string BasketId { get; set; }
        public int ProductId { get; set; }
        [Range(0,50,ErrorMessage="Please Enter a value between 0 and 50")]
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }

    }
}
