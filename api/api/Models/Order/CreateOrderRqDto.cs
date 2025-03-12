namespace api.Models.Order
{
    public class CreateOrderRqDto
    {      
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public int ArticleStoreId { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
