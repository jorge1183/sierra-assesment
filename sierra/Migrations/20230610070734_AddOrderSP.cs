using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sierra.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql(
            @"CREATE PROCEDURE dbo.AddOrder
              @customerId int,
              @productId int,
              @price decimal(18,2),
              @quantity decimal(18,2)
              AS
              BEGIN
                SET NOCOUNT ON;
                DECLARE @orderId int;
                DECLARE @total AS DECIMAL(18,2) = @price * @quantity;
                
                INSERT INTO dbo.Orders
                (CustomerId, ProductId, Price, Quantity, Total)
                VALUES
                (@customerId, @productId, @price, @quantity, @total);

                SELECT CAST(SCOPE_IDENTITY() AS int) AS Id
              END"
          );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.Sql("DROP PROCEDURE dbo.AddOrder");
        }
    }
}
