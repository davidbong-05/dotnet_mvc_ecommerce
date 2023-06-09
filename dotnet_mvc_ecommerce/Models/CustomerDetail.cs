using dotnet_mvc_ecommerce.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_mvc_ecommerce.Models
{
    public class CustomerDetail
    {

        [Key]
        public int Id { get; set; }

        [Display(Name = "CreditCard")]
        public string? CreditCard { get; set; }

        //User
        [Column(TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }

        public CustomerDetail() { }

        public CustomerDetail(string? creditCard, User? user)
        {
            CreditCard = creditCard;
            User = user;
        }
    }
}
