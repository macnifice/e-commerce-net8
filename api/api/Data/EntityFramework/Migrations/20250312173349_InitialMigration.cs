using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Data.EntityFramework.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArticleStores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticleId = table.Column<int>(type: "int", nullable: false),
                    StoreId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleStores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ArticleStores_Articles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "Articles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ArticleStores_Stores_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Stores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerArticles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ArticleStoreId = table.Column<int>(type: "int", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ArticleEntityId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerArticles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerArticles_ArticleStores_ArticleStoreId",
                        column: x => x.ArticleStoreId,
                        principalTable: "ArticleStores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerArticles_Articles_ArticleEntityId",
                        column: x => x.ArticleEntityId,
                        principalTable: "Articles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerArticles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Articles",
                columns: new[] { "Id", "Code", "Description", "ImagePath", "Name" },
                values: new object[,]
                {
                    { 1, "LAP001", "Ultrabook de alto rendimiento con pantalla táctil 4K.", "https://m.media-amazon.com/images/I/91MXLpouhoL.jpg", "Laptop Dell XPS 13" },
                    { 2, "IPH014", "Smartphone con cámara de 48 MP y procesador A16 Bionic.", "https://ss632.liverpool.com.mx/xl/1145923731.jpg", "iPhone 14 Pro" },
                    { 3, "MON002", "Monitor UHD de 32 pulgadas con HDR10+.", "https://example.com/images/monitor-samsung-4k.jpg", "Monitor Samsung 4K" },
                    { 4, "TEC004", "Teclado mecánico inalámbrico con switches táctiles.", "https://m.media-amazon.com/images/I/71dc-E1RYyL._AC_UF894,1000_QL80_.jpg", "Teclado Mecánico Logitech G915" },
                    { 5, "MOU005", "Mouse ergonómico para gaming con sensor óptico de 20K DPI.", "https://ss637.liverpool.com.mx/xl/1134205390.jpg", "Mouse Razer DeathAdder" },
                    { 6, "AUD006", "Auriculares inalámbricos con cancelación de ruido líder en la industria.", "https://www.sony.com.mx/image/6145c1d32e6ac8e63a46c912dc33c5bb?fmt=pjpeg&wid=330&bgcolor=FFFFFF&bgc=FFFFFF", "Auriculares Sony WH-1000XM5" },
                    { 7, "SIL007", "Silla ergonómica con soporte lumbar ajustable y cuero sintético premium.", "https://images.secretlab.co/turntable/tr:n-w_450/M07-E24SU-MCLRN1R_02.jpg", "Silla Gamer Secretlab Titan Evo" },
                    { 8, "TAB008", "Tablet con chip M2, pantalla Liquid Retina XDR y compatibilidad con Apple Pencil.", "https://cdsassets.apple.com/live/SZLF0YNV/images/sp/112024_SP723-iPad_Pro.png", "Tablet iPad Pro 12.9" },
                    { 9, "SSD009", "SSD NVMe con velocidades de lectura hasta 7000 MB/s.", "https://m.media-amazon.com/images/I/71GLjKuxf7L.jpg", "Disco SSD Samsung 980 Pro 1TB" },
                    { 10, "REL010", "Smartwatch multideporte con GPS y batería de larga duración.", "https://cdn1.coppel.com/images/catalog/mkp/7463/3000/74631252-1.jpg", "Reloj Inteligente Garmin Fenix 7" }
                });

            migrationBuilder.InsertData(
                table: "Stores",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[,]
                {
                    { 1, "Av. Principal 123, Ciudad Central, CP 10000", "Tech Store Centro" },
                    { 2, "Calle Secundaria 456, Zona Norte, CP 20000", "Tech Store Norte" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Address", "Email", "LastName", "Name", "PasswordHash", "RefreshToken", "RefreshTokenExpiryTime", "Role", "UserName" },
                values: new object[] { 1, null, null, "admin", "admin", "AQAAAAIAAYagAAAAEHCc0DphWlrKK1YwXC2Zytn0VnO5irnJF83/0PSceRJO9K4xfy0Ly8MCrUQpaVcBQg==", null, null, "Admin", "admin" });

            migrationBuilder.InsertData(
                table: "ArticleStores",
                columns: new[] { "Id", "ArticleId", "Date", "Price", "Stock", "StoreId" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1299.99, 10, 1 },
                    { 2, 2, new DateTime(2024, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 999.99000000000001, 15, 1 },
                    { 3, 3, new DateTime(2024, 3, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 299.99000000000001, 20, 2 },
                    { 4, 4, new DateTime(2024, 3, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 199.99000000000001, 25, 2 },
                    { 5, 5, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 79.989999999999995, 30, 1 },
                    { 6, 6, new DateTime(2024, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), 349.99000000000001, 12, 2 },
                    { 7, 7, new DateTime(2024, 3, 7, 0, 0, 0, 0, DateTimeKind.Unspecified), 499.99000000000001, 8, 1 },
                    { 8, 8, new DateTime(2024, 3, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1099.99, 5, 2 },
                    { 9, 9, new DateTime(2024, 3, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), 159.99000000000001, 40, 1 },
                    { 10, 10, new DateTime(2024, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 599.99000000000001, 10, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ArticleStores_ArticleId",
                table: "ArticleStores",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_ArticleStores_StoreId",
                table: "ArticleStores",
                column: "StoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerArticles_ArticleEntityId",
                table: "CustomerArticles",
                column: "ArticleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerArticles_ArticleStoreId",
                table: "CustomerArticles",
                column: "ArticleStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerArticles_UserId",
                table: "CustomerArticles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerArticles");

            migrationBuilder.DropTable(
                name: "ArticleStores");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
