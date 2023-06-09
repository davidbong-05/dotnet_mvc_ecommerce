﻿using dotnet_mvc_ecommerce.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet_mvc_ecommerce.Models
{
    public class ShoppingBasket
    {
        [Key]
        public int Id { get; set; }

        //Relationship
        public virtual ICollection<ShoppingBasket_Product> ShoppingBasket_Products { get; set; }

        //User
        [Column(TypeName = "nvarchar(450)")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }


        public ShoppingBasket() { }

        public ShoppingBasket(User user)
        {
            User = user;
        }
    }
}
