using api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Data.EntityFramework
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<CustomerArticleEntity> CustomerArticles { get; set; }
        public DbSet<ArticleStoreEntity> ArticleStores { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }
        public DbSet<ArticleEntity> Articles { get; set; }
        public DbSet<UserEntity> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed de datos
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = 1,
                    UserName = "admin",
                    Name = "admin",
                    LastName = "admin",
                    PasswordHash = "AQAAAAIAAYagAAAAEHCc0DphWlrKK1YwXC2Zytn0VnO5irnJF83/0PSceRJO9K4xfy0Ly8MCrUQpaVcBQg==",
                    Role = "Admin"
                }                
                );

            // Relación Tienda ↔ Artículo
            modelBuilder.Entity<ArticleStoreEntity>()
                .HasKey(sa => sa.Id);

            modelBuilder.Entity<ArticleStoreEntity>()
                .HasOne(sa => sa.Store)
                .WithMany(s => s.ArticleStore)
                .HasForeignKey(sa => sa.StoreId);

            modelBuilder.Entity<ArticleStoreEntity>()
                .HasOne(sa => sa.Article)
                .WithMany(a => a.ArticleStore)
                .HasForeignKey(sa => sa.ArticleId);

            // Relación Cliente ↔ Artículo Tienda
            modelBuilder.Entity<CustomerArticleEntity>()
                .HasKey(ca => ca.Id);

            modelBuilder.Entity<CustomerArticleEntity>()
                .HasOne(ca => ca.User)
                .WithMany(c => c.CustomerArticle)
                .HasForeignKey(ca => ca.UserId);

            modelBuilder.Entity<CustomerArticleEntity>()
                .HasOne(ca => ca.ArticleStore)
                .WithMany(a => a.CustomerArticle)
                .HasForeignKey(ca => ca.ArticleStoreId);

        }
    }
}
