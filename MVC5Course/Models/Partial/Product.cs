using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MVC5Course.Models
{
    [MetadataType(typeof(ProductMetadata))]
    public partial class Product
    {
        private class ProductMetadata
        {
            public int ProductId { get; set; }

            [Required(ErrorMessage = "商品名稱為必填")]
            [StringLength(20, ErrorMessage = "商名名稱不得大於20個字")]
            [Display(Name = "產品名稱")]
            public string ProductName { get; set; }

            [Required]
            [DisplayFormat(DataFormatString = "{0:N0}")]
            public Nullable<decimal> Price { get; set; }

            [Required]
            public Nullable<bool> Active { get; set; }

            [Required]
            public Nullable<decimal> Stock { get; set; }
        }
    }
}