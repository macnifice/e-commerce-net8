using api.Business.Interfaces.Order;
using api.Data.Entities;
using api.Data.EntityFramework;
using api.Models.Article;
using api.Models.Order;
using Microsoft.EntityFrameworkCore;

namespace api.Business.Services.Order
{
    public class OrderService(AppDbContext _context) : IOrderService
    {
        public async Task<List<OrderDto>> GetOrdersByUserId(int userId)
        {
            try
            {
                List<CustomerArticleEntity> customerArticles = await _context.CustomerArticles
                    .Where(ca => ca.UserId == userId)
                    .Include(ca => ca.ArticleStore)
                    .ThenInclude(ase => ase.Article)
                    .Include(ca => ca.ArticleStore)
                    .ThenInclude(ase => ase.Store)
                    .OrderByDescending(ca => ca.PurchaseDate)
                    .ToListAsync();

                if (customerArticles == null || customerArticles.Count == 0)
                    return new List<OrderDto>();

                List<OrderDto> orders = customerArticles.Select(ca => new OrderDto
                {
                    Id = ca.Id,
                    ArticleName = ca.ArticleStore.Article.Name,
                    StoreName = ca.ArticleStore.Store.Name,
                    Quantity = ca.Quantity,
                    Price = (decimal)ca.ArticleStore.Price,
                    TotalAmount = ca.Quantity * (decimal)ca.ArticleStore.Price,
                    PurchaseDate = ca.PurchaseDate
                }).ToList();

                return orders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<OrderRsDto> Create(List<CreateOrderRqDto> rq, int userId)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                { 
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null)
                        return null;

                    decimal totalAmount = 0;
                    List<CustomerArticleEntity> newOrders = new List<CustomerArticleEntity>();

                    foreach (var item in rq)
                    {
                        var articleStore = await _context.ArticleStores
                            .FirstOrDefaultAsync(a => a.Id == item.ArticleStoreId);

                        if (articleStore == null || articleStore.Stock < item.Quantity)
                            return null;

                        articleStore.Stock -= item.Quantity;
                        _context.ArticleStores.Update(articleStore);

                        var customerArticle = new CustomerArticleEntity
                        {
                            UserId = userId,
                            ArticleStoreId = item.ArticleStoreId,
                            Quantity = item.Quantity,
                            PurchaseDate = DateTime.Now
                        };

                        _context.CustomerArticles.Add(customerArticle);
                        newOrders.Add(customerArticle);

                        totalAmount += (decimal)articleStore.Price * item.Quantity;
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return new OrderRsDto
                    {
                        OrderIds = newOrders.Select(o => o.Id).ToList(),
                        TotalAmount = totalAmount,
                        Message = "Orden creada exitosamente"
                    };
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
