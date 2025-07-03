using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinePayDemo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ADD_LinePayRefundTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "line_pay_refund_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    original_line_pay_transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refund_request_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    refund_amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    line_pay_refund_transaction_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    request_date_time = table.Column<DateTime>(type: "datetime2", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_line_pay_refund_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_line_pay_refund_transactions_line_pay_transactions_original_line_pay_transaction_id",
                        column: x => x.original_line_pay_transaction_id,
                        principalTable: "line_pay_transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_line_pay_refund_transactions_original_line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                column: "original_line_pay_transaction_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "line_pay_refund_transactions");
        }
    }
}
