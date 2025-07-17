using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LinePayDemo.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class A : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_line_pay_refund_transactions_line_pay_transactions_original_line_pay_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropTable(
                name: "user_balances");

            migrationBuilder.DropIndex(
                name: "ix_line_pay_refund_transactions_original_line_pay_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropColumn(
                name: "price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "product_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "quantity",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "line_pay_transactions");

            migrationBuilder.DropColumn(
                name: "order_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropColumn(
                name: "original_line_pay_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropColumn(
                name: "refund_request_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "orders",
                newName: "buyer_id");

            migrationBuilder.RenameColumn(
                name: "request_date_time",
                table: "line_pay_transactions",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "line_pay_refund_transactions",
                newName: "line_pay_transaction_id");

            migrationBuilder.RenameColumn(
                name: "request_date_time",
                table: "line_pay_refund_transactions",
                newName: "created_at");

            migrationBuilder.AddColumn<decimal>(
                name: "current_point_balance",
                table: "users",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "unit_price",
                table: "products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "ledger_transaction_id",
                table: "orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "line_pay_transaction_id",
                table: "line_pay_transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "line_pay_transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_at",
                table: "line_pay_transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ledger_transaction_id",
                table: "line_pay_transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "pending_at",
                table: "line_pay_transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "line_pay_refund_transaction_id",
                table: "line_pay_refund_transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<DateTime>(
                name: "cancelled_at",
                table: "line_pay_refund_transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "confirmed_at",
                table: "line_pay_refund_transactions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ledger_transaction_id",
                table: "line_pay_refund_transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "order_details",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    order_item_data_product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    order_item_data_quantity = table.Column<int>(type: "int", nullable: false),
                    order_item_data_unit_price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_details_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "order_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_details_products_order_item_data_product_id",
                        column: x => x.order_item_data_product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transactions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "transaction_entries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    data_account_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    data_debit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    data_credit = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transaction_entries", x => x.id);
                    table.ForeignKey(
                        name: "fk_transaction_entries_accounts_data_account_id",
                        column: x => x.data_account_id,
                        principalTable: "accounts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_transaction_entries_transactions_transaction_id",
                        column: x => x.transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_point_transactions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    transaction_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ledger_transaction_id = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_point_transactions", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_point_transactions_transactions_ledger_transaction_id",
                        column: x => x.ledger_transaction_id,
                        principalTable: "transactions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_orders_ledger_transaction_id",
                table: "orders",
                column: "ledger_transaction_id",
                unique: true,
                filter: "[ledger_transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_line_pay_transactions_ledger_transaction_id",
                table: "line_pay_transactions",
                column: "ledger_transaction_id",
                unique: true,
                filter: "[ledger_transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_line_pay_refund_transactions_ledger_transaction_id",
                table: "line_pay_refund_transactions",
                column: "ledger_transaction_id",
                unique: true,
                filter: "[ledger_transaction_id] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "ix_line_pay_refund_transactions_line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                column: "line_pay_transaction_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_details_order_id",
                table: "order_details",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_details_order_item_data_product_id",
                table: "order_details",
                column: "order_item_data_product_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_entries_data_account_id",
                table: "transaction_entries",
                column: "data_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_transaction_entries_transaction_id",
                table: "transaction_entries",
                column: "transaction_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_point_transactions_ledger_transaction_id",
                table: "user_point_transactions",
                column: "ledger_transaction_id",
                unique: true,
                filter: "[ledger_transaction_id] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "fk_line_pay_refund_transactions_line_pay_transactions_line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                column: "line_pay_transaction_id",
                principalTable: "line_pay_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_line_pay_refund_transactions_transactions_ledger_transaction_id",
                table: "line_pay_refund_transactions",
                column: "ledger_transaction_id",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_line_pay_transactions_transactions_ledger_transaction_id",
                table: "line_pay_transactions",
                column: "ledger_transaction_id",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_transactions_ledger_transaction_id",
                table: "orders",
                column: "ledger_transaction_id",
                principalTable: "transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_line_pay_refund_transactions_line_pay_transactions_line_pay_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_line_pay_refund_transactions_transactions_ledger_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_line_pay_transactions_transactions_ledger_transaction_id",
                table: "line_pay_transactions");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_transactions_ledger_transaction_id",
                table: "orders");

            migrationBuilder.DropTable(
                name: "order_details");

            migrationBuilder.DropTable(
                name: "transaction_entries");

            migrationBuilder.DropTable(
                name: "user_point_transactions");

            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropIndex(
                name: "ix_orders_ledger_transaction_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_line_pay_transactions_ledger_transaction_id",
                table: "line_pay_transactions");

            migrationBuilder.DropIndex(
                name: "ix_line_pay_refund_transactions_ledger_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropIndex(
                name: "ix_line_pay_refund_transactions_line_pay_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropColumn(
                name: "current_point_balance",
                table: "users");

            migrationBuilder.DropColumn(
                name: "unit_price",
                table: "products");

            migrationBuilder.DropColumn(
                name: "ledger_transaction_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "line_pay_transactions");

            migrationBuilder.DropColumn(
                name: "confirmed_at",
                table: "line_pay_transactions");

            migrationBuilder.DropColumn(
                name: "ledger_transaction_id",
                table: "line_pay_transactions");

            migrationBuilder.DropColumn(
                name: "pending_at",
                table: "line_pay_transactions");

            migrationBuilder.DropColumn(
                name: "cancelled_at",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropColumn(
                name: "confirmed_at",
                table: "line_pay_refund_transactions");

            migrationBuilder.DropColumn(
                name: "ledger_transaction_id",
                table: "line_pay_refund_transactions");

            migrationBuilder.RenameColumn(
                name: "buyer_id",
                table: "orders",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "line_pay_transactions",
                newName: "request_date_time");

            migrationBuilder.RenameColumn(
                name: "line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "line_pay_refund_transactions",
                newName: "request_date_time");

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "product_id",
                table: "orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "quantity",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<long>(
                name: "line_pay_transaction_id",
                table: "line_pay_transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "line_pay_transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<long>(
                name: "line_pay_refund_transaction_id",
                table: "line_pay_refund_transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "order_id",
                table: "line_pay_refund_transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "original_line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "refund_request_id",
                table: "line_pay_refund_transactions",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "user_balances",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user_balances", x => x.user_id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_line_pay_refund_transactions_original_line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                column: "original_line_pay_transaction_id");

            migrationBuilder.AddForeignKey(
                name: "fk_line_pay_refund_transactions_line_pay_transactions_original_line_pay_transaction_id",
                table: "line_pay_refund_transactions",
                column: "original_line_pay_transaction_id",
                principalTable: "line_pay_transactions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
