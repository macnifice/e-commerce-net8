namespace api.Data.Entities
{
    public class ArticleEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public ICollection<ArticleStoreEntity> ArticleStore { get; set; }
        public ICollection<CustomerArticleEntity> CustomerArticle { get; set; }
    }
}
