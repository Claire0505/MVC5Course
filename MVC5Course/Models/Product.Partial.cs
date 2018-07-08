namespace MVC5Course.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
    }
    
    public partial class ProductMetaData
    {
        [Required]
        [Display(Name = "商品編號")]
        public int ProductId { get; set; }

        [Required]
        [Display(Name = "商品名稱")]
        [StringLength(80, ErrorMessage = "欄位長度不得大於 80 個字元")]
        public string ProductName { get; set; }

        [Required]
        [Display(Name = "價格")]
        [Range(10, 99999)]
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public Nullable<decimal> Price { get; set; }

        [Required]
        [Display(Name = "是否有效")]
        public Nullable<bool> Active { get; set; }

        [Required]
        [Display(Name = "庫存")]
        public Nullable<decimal> Stock { get; set; }

        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
