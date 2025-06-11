using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountTaskEntity");

            migrationBuilder.AddColumn<int>(
                name: "TaskEntityId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TaskEntityId",
                table: "Accounts",
                column: "TaskEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_TasksEntity_TaskEntityId",
                table: "Accounts",
                column: "TaskEntityId",
                principalTable: "TasksEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_TasksEntity_TaskEntityId",
                table: "Accounts");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_TaskEntityId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TaskEntityId",
                table: "Accounts");

            migrationBuilder.CreateTable(
                name: "AccountTaskEntity",
                columns: table => new
                {
                    AccountsUsernameId = table.Column<int>(type: "int", nullable: false),
                    TasksId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountTaskEntity", x => new { x.AccountsUsernameId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_AccountTaskEntity_Accounts_AccountsUsernameId",
                        column: x => x.AccountsUsernameId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AccountTaskEntity_TasksEntity_TasksId",
                        column: x => x.TasksId,
                        principalTable: "TasksEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccountTaskEntity_TasksId",
                table: "AccountTaskEntity",
                column: "TasksId");
        }
    }
}
