namespace api.Models.Article.Request
{
    public class CreateOrEditArticleRqDto
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
    }
}
