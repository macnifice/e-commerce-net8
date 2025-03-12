namespace api.Data.Entities
{
    public class CustomerArticleEntity
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
        public int ArticleStoreId { get; set; }
        public ArticleStoreEntity ArticleStore { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
