using dotnet_mvc_ecommerce.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_mvc_ecommerce.Models
{
    public class Product
    {
        [Key]
        public int id { set; get; }

        [Display(Name = "name")]
        [Column(TypeName = "nvarchar(256)")]
        public string ProductName { get; set; }

        [Display(Name = "category")]
        [Column(TypeName = "nvarchar(256)")]
        public string ProductCategory { get; set; }

        [Display(Name = "price")]
        public float ProductPrice { get; set; }

        //relationship
        public virtual ICollection<Order_Product> Order_Products { get; set; }

        //ShoppingBasketId
        public int ShoppingBasketId { get; set; }
        [ForeignKey(" ShoppingBasketId")]
        public virtual ShoppingBasket ShoppingBasket { get; set; }
    }
}
