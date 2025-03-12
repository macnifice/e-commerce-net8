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
            modelBuilder.Entity<ArticleStoreEntity>().HasData(
                new ArticleStoreEntity
                {
                    Id = 1,
                    Price = 1299.99,
                    Stock = 10,
                    Date = new DateTime(2024, 03, 01), 
                    ArticleId = 1,  
                    StoreId = 1      
                },
                new ArticleStoreEntity
                {
                    Id = 2,
                    Price = 999.99,
                    Stock = 15,
                    Date = new DateTime(2024, 03, 02), 
                    ArticleId = 2,  
                    StoreId = 1     
                },
                new ArticleStoreEntity
                {
                    Id = 3,
                    Price = 299.99,
                    Stock = 20,
                    Date = new DateTime(2024, 03, 03), 
                    ArticleId = 3,  
                    StoreId = 2     
                },
                new ArticleStoreEntity
                {
                    Id = 4,
                    Price = 199.99,
                    Stock = 25,
                    Date = new DateTime(2024, 03, 04), 
                    ArticleId = 4,  
                    StoreId = 2    
                },
                new ArticleStoreEntity
                {
                    Id = 5,
                    Price = 79.99,
                    Stock = 30,
                    Date = new DateTime(2024, 03, 05), 
                    ArticleId = 5,  
                    StoreId = 1     
                },
                new ArticleStoreEntity
                {
                    Id = 6,
                    Price = 349.99,
                    Stock = 12,
                    Date = new DateTime(2024, 03, 06), 
                    ArticleId = 6,  
                    StoreId = 2    
                },
                new ArticleStoreEntity
                {
                    Id = 7,
                    Price = 499.99,
                    Stock = 8,
                    Date = new DateTime(2024, 03, 07), 
                    ArticleId = 7,  
                    StoreId = 1    
                },
                new ArticleStoreEntity
                {
                    Id = 8,
                    Price = 1099.99,
                    Stock = 5,
                    Date = new DateTime(2024, 03, 08), 
                    ArticleId = 8, 
                    StoreId = 2    
                },
                new ArticleStoreEntity
                {
                    Id = 9,
                    Price = 159.99,
                    Stock = 40,
                    Date = new DateTime(2024, 03, 09), 
                    ArticleId = 9, 
                    StoreId = 1    
                },
                new ArticleStoreEntity
                {
                    Id = 10,
                    Price = 599.99,
                    Stock = 10,
                    Date = new DateTime(2024, 03, 10), 
                    ArticleId = 10,  
                    StoreId = 2      
                }
            );


            modelBuilder.Entity<StoreEntity>().HasData(
                new StoreEntity
                {
                    Id = 1,
                    Name = "Tech Store Centro",
                    Address = "Av. Principal 123, Ciudad Central, CP 10000"
                },
                new StoreEntity
                {
                    Id = 2,
                    Name = "Tech Store Norte",
                    Address = "Calle Secundaria 456, Zona Norte, CP 20000"
                }
                );

            modelBuilder.Entity<ArticleEntity>().HasData(
               new ArticleEntity
               {
                   Id = 1,
                   Name = "Laptop Dell XPS 13",
                   Code = "LAP001",
                   Description = "Ultrabook de alto rendimiento con pantalla táctil 4K.",
                   ImagePath = "https://m.media-amazon.com/images/I/91MXLpouhoL.jpg"
               },
               new ArticleEntity
               {
                   Id = 2,
                   Name = "iPhone 14 Pro",
                   Code = "IPH014",
                   Description = "Smartphone con cámara de 48 MP y procesador A16 Bionic.",
                   ImagePath = "https://ss632.liverpool.com.mx/xl/1145923731.jpg"
               },
               new ArticleEntity
               {
                   Id = 3,
                   Name = "Monitor Samsung 4K",
                   Code = "MON002",
                   Description = "Monitor UHD de 32 pulgadas con HDR10+.",
                   ImagePath = "https://example.com/images/monitor-samsung-4k.jpg"
               },
               new ArticleEntity
               {
                   Id = 4,
                   Name = "Teclado Mecánico Logitech G915",
                   Code = "TEC004",
                   Description = "Teclado mecánico inalámbrico con switches táctiles.",
                   ImagePath = "https://m.media-amazon.com/images/I/71dc-E1RYyL._AC_UF894,1000_QL80_.jpg"
               },
               new ArticleEntity
               {
                   Id = 5,
                   Name = "Mouse Razer DeathAdder",
                   Code = "MOU005",
                   Description = "Mouse ergonómico para gaming con sensor óptico de 20K DPI.",
                   ImagePath = "https://ss637.liverpool.com.mx/xl/1134205390.jpg"
               },
               new ArticleEntity
               {
                   Id = 6,
                   Name = "Auriculares Sony WH-1000XM5",
                   Code = "AUD006",
                   Description = "Auriculares inalámbricos con cancelación de ruido líder en la industria.",
                   ImagePath = "https://www.sony.com.mx/image/6145c1d32e6ac8e63a46c912dc33c5bb?fmt=pjpeg&wid=330&bgcolor=FFFFFF&bgc=FFFFFF"
               },
               new ArticleEntity
               {
                   Id = 7,
                   Name = "Silla Gamer Secretlab Titan Evo",
                   Code = "SIL007",
                   Description = "Silla ergonómica con soporte lumbar ajustable y cuero sintético premium.",
                   ImagePath = "https://images.secretlab.co/turntable/tr:n-w_450/M07-E24SU-MCLRN1R_02.jpg"
               },
               new ArticleEntity
               {
                   Id = 8,
                   Name = "Tablet iPad Pro 12.9",
                   Code = "TAB008",
                   Description = "Tablet con chip M2, pantalla Liquid Retina XDR y compatibilidad con Apple Pencil.",
                   ImagePath = "https://cdsassets.apple.com/live/SZLF0YNV/images/sp/112024_SP723-iPad_Pro.png"
               },
               new ArticleEntity
               {
                   Id = 9,
                   Name = "Disco SSD Samsung 980 Pro 1TB",
                   Code = "SSD009",
                   Description = "SSD NVMe con velocidades de lectura hasta 7000 MB/s.",
                   ImagePath = "https://m.media-amazon.com/images/I/71GLjKuxf7L.jpg"
               },
               new ArticleEntity
               {
                   Id = 10,
                   Name = "Reloj Inteligente Garmin Fenix 7",
                   Code = "REL010",
                   Description = "Smartwatch multideporte con GPS y batería de larga duración.",
                   ImagePath = "https://cdn1.coppel.com/images/catalog/mkp/7463/3000/74631252-1.jpg"
               }
            );

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
