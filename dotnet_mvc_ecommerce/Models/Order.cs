using dotnet_mvc_ecommerce.Areas.Identity.Data;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_mvc_ecommerce.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Order Detail")]
        [Column(TypeName = "nvarchar(256)")]
        public string OrderDetails { get; set; }

        //Relationship
        public virtual ICollection<Order_Product> Order_Products { get; set; }

        //User
        [Column(TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
    }
}
