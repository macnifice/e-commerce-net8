using api.Models.Article;
using api.Models.Article.Request;

namespace api.Business.Interfaces.Article
{
    public interface IArticleService
    {
        public Task<List<ArticleDto>> GetAll();
        public Task<ArticleDto> GetbyId(int id);
        public Task<int?> CreateArticle(CreateOrEditArticleRqDto rq);

        public Task<int?> Update(int id, CreateOrEditArticleRqDto rq);
        public Task<int?> Delete(int id);

        public Task<List<ArticleStoreDto>> GetAllStores(int storeId);
        public Task<int?> AssaignArticleStore(CreateOrEditArticleStoreDto rq);
        public Task<int?> UpdateArticleStore(int id, CreateOrEditArticleStoreDto rq);
        public Task<int?> DeleteArticleStore(int id);
    }
}
