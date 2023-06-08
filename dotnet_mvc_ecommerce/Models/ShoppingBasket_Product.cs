using dotnet_mvc_ecommerce.Migrations;
using System.ComponentModel.DataAnnotations;

namespace dotnet_mvc_ecommerce.Models
{
    public class ShoppingBasket_Product
    {
        public int ShoppingBasketId { get; set; }
        public virtual ShoppingBasket ShoppingBasket { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Display(Name = "quantity")]
        public int Quantity { get; set; }

        public ShoppingBasket_Product() { }

        public ShoppingBasket_Product(ShoppingBasket shoppingBasket, Product product, int quantity)
        {
            ShoppingBasket = shoppingBasket;
            Product = product;
            Quantity = quantity;
        }
    }
}
