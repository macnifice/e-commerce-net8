namespace api.Models.Store
{
    public class StoreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int ArticleStoreId { get; set; }
    }
}
