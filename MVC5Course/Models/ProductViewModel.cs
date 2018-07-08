using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC5Course.Models
{
    public class ProductViewModel
    {
        [Display(Name = "商品編號")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "商品名稱")]
        [StringLength(10, ErrorMessage = "商品名稱不得大於 10 個字")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "價格")]
        [Range(10, 99999)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [Display(Name = "庫存")]
        public Nullable<decimal> Stock { get; set; }
    }
}