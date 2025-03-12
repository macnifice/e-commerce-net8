using api.Models.Store;

namespace api.Models.Article
{
    public class ArticleStoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Stock { get; set; }
        public int StoreId { get; set; }
        public StoreDto Store { get; set; }
        public int ArticleId { get; set; }
        public DateTime Date { get; set; }

    }
}
