namespace api.Data.Entities
{
    public class ArticleStoreEntity
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime Date { get; set; }        
        public int ArticleId { get; set; }
        public ArticleEntity Article { get; set; }
        public int StoreId { get; set; }
        public StoreEntity Store { get; set; }
        public ICollection<CustomerArticleEntity> CustomerArticle { get; set; }
    }
}
