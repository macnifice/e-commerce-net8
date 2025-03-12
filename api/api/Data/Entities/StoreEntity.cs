namespace api.Data.Entities
{
    public class StoreEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<ArticleStoreEntity> ArticleStore { get; set; }
    }
}
