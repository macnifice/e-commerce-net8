namespace api.Models.Article.Request
{
    public class CreateOrEditArticleStoreDto
    {
        public double Price { get; set; }
        public int Stock { get; set; }
        public DateTime Date { get; set; }
        public int StoreId { get; set; }
        public int ArticleId { get; set; }
    }
}
