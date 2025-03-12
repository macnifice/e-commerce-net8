using api.Business.Interfaces.Article;
using api.Data.Entities;
using api.Data.EntityFramework;
using api.Models.Article;
using api.Models.Article.Request;
using api.Models.Store;
using Microsoft.EntityFrameworkCore;

namespace api.Business.Services.Article
{
    public class ArticleService(AppDbContext _context) : IArticleService
    {

        public async Task<int?> CreateArticle(CreateOrEditArticleRqDto rq)
        {
            ArticleEntity existArticle = await _context.Articles.FirstOrDefaultAsync(a => a.Name == rq.Name);
            if (existArticle != null)
                return null;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ArticleEntity article = new ArticleEntity
                    {
                        Name = rq.Name,
                        Code = rq.Code,
                        Description = rq.Description,
                        ImagePath = rq.ImagePath
                    };
                    _context.Articles.Add(article);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return article.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

        }

        public async Task<int?> Delete(int id)
        {
            if (id <= 0)
                return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ArticleEntity article = await _context.Articles.FindAsync(id);
                    if (article is null)
                        return null;

                    bool hasStoreAssignments = await _context.ArticleStores.AnyAsync(a => a.ArticleId == id);
                    if (hasStoreAssignments)
                    {
                        List<ArticleStoreEntity> storeAssignments = await _context.ArticleStores
                            .Where(a => a.ArticleId == id)
                            .ToListAsync();

                        _context.ArticleStores.RemoveRange(storeAssignments);
                        await _context.SaveChangesAsync();
                    }

                    _context.Articles.Remove(article);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return article.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<List<ArticleDto>> GetAll()
        {
            List<ArticleDto> articlesDto = new List<ArticleDto>();
            List<ArticleEntity> articles = await _context.Articles.ToListAsync();

            foreach (ArticleEntity article in articles)
            {
                ArticleDto articleStoreDto = new ArticleDto
                {
                    Id = article.Id,
                    Name = article.Name,
                    Code = article.Code,
                    Description = article.Description,
                    ImagePath = article.ImagePath
                };
                articlesDto.Add(articleStoreDto);
            }
            return articlesDto;
        }

        public async Task<List<ArticleStoreDto>> GetAllStores(int storeId)
        {
            List<ArticleStoreDto> articlesDto = new List<ArticleStoreDto>();
            List<ArticleStoreEntity> articles = await _context.ArticleStores
                .Where(s => s.StoreId == storeId)
                .Include(a => a.Article)
                .ToListAsync();

            foreach (ArticleStoreEntity article in articles)
            {
                ArticleStoreDto articleStoreDto = new ArticleStoreDto
                {
                    Id = article.Id,
                    Name = article.Article.Name,
                    Code = article.Article.Code,
                    Description = article.Article.Description,
                    Price = article.Price,
                    Stock = article.Stock,
                    StoreId = article.StoreId,
                    ArticleId = article.ArticleId,
                    Date = article.Date
                };
                articlesDto.Add(articleStoreDto);
            }
            return articlesDto;
        }

        public async Task<ArticleDto> GetbyId(int id)
        {
            try
            {
                ArticleEntity article = await _context.Articles.Where(a => a.Id == id)
                    .Include(sa => sa.ArticleStore)
                    .ThenInclude(se => se.Store)
                    .FirstOrDefaultAsync();

                if (article is null)
                    return null;

                ArticleDto articleDto = new ArticleDto
                {
                    Id = article.Id,
                    Name = article.Name,
                    Code = article.Code,
                    Description = article.Description,
                    ImagePath = article.ImagePath,
                    Store = article.ArticleStore.Select(s => new StoreDto
                    {
                        Id = s.Store.Id,
                        Name = s.Store.Name,
                        Address = s.Store.Address,
                        Price = s.Price,
                        Stock = s.Stock,
                        ArticleStoreId = s.Id
                    }).ToList()
                };

                return articleDto;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<int?> Update(int id, CreateOrEditArticleRqDto rq)
        {
            ArticleEntity articleEntity = await _context.Articles.Where(a => a.Id == id).FirstOrDefaultAsync();

            if (articleEntity == null)
                return null;

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    articleEntity.Name = rq.Name;
                    articleEntity.Code = rq.Code;
                    articleEntity.ImagePath = rq.ImagePath;
                    articleEntity.Description = rq.Description;
                    _context.Articles.Update(articleEntity);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return articleEntity.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<int?> AssaignArticleStore(CreateOrEditArticleStoreDto rq)
        {
            ArticleStoreEntity existArticleStore = await _context.ArticleStores.FirstOrDefaultAsync(a => a.StoreId == rq.StoreId && a.ArticleId == rq.ArticleId);
            if (existArticleStore != null)
                return null;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ArticleStoreEntity articleStore = new ArticleStoreEntity { Price = rq.Price, Stock = rq.Stock, Date = rq.Date, ArticleId = rq.ArticleId, StoreId = rq.StoreId };
                    _context.ArticleStores.Add(articleStore);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return articleStore.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<int?> UpdateArticleStore(int id, CreateOrEditArticleStoreDto rq)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ArticleStoreEntity articleStore = await _context.ArticleStores.FindAsync(id);
                    if (articleStore == null)
                        return null;

                    articleStore.Price = rq.Price;
                    articleStore.Stock = rq.Stock;
                    articleStore.Date = rq.Date;

                    _context.ArticleStores.Update(articleStore);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return articleStore.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<int?> DeleteArticleStore(int id)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    ArticleStoreEntity articleStore = await _context.ArticleStores.FindAsync(id);
                    if (articleStore == null)
                        return null;

                    _context.ArticleStores.Remove(articleStore);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return articleStore.Id;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }
    }
}
