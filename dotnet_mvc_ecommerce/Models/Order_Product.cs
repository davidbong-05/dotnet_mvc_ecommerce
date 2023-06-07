namespace dotnet_mvc_ecommerce.Models
{
    public class Order_Product
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
