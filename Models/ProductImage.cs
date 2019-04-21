using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BabyStore.Models
{
    public class ProductImage
    {
        public int ID { get; set; }
        [Display(Name = "File")]
        [StringLength(100)]
        public string FileName { get; set; }
        public virtual ICollection<ProductImageMapping> ProductImageMappings { get; set; }
    }
}
