using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.PaymentGateway.Infrastructure.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankingPaymentId = table.Column<string>(maxLength: 128, nullable: false),
                    SuccessfulPayment = table.Column<bool>(nullable: false),
                    CardNumber = table.Column<string>(maxLength: 19, nullable: false),
                    ExpiryMonth = table.Column<int>(nullable: false),
                    ExpiryYear = table.Column<int>(nullable: false),
                    CVV = table.Column<string>(maxLength: 4, nullable: false),
                    Amount = table.Column<decimal>(type: "DECIMAL(6,3)", nullable: false),
                    Currency = table.Column<string>(maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
