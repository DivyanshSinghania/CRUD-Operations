using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Registration.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BudgetCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CategoryType = table.Column<string>(type: "text", nullable: false),
                    BudgetAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    FinancialYear = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetCategories", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BudgetCategories");
        }
    }
}
