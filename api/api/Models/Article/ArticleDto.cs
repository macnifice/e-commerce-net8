using api.Models.Store;

namespace api.Models.Article
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }       
        public List<StoreDto> Store { get; set; }

    }
}
