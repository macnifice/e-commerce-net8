namespace api.Models.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string ArticleName { get; set; }
        public string StoreName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
} 