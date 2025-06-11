using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTaskEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "TasksEntity",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TaskEntityId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TasksEntity_ProjectId",
                table: "TasksEntity",
                column: "ProjectId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TasksEntity_Projects_ProjectId",
                table: "TasksEntity",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_TasksEntity_TaskEntityId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_TasksEntity_Projects_ProjectId",
                table: "TasksEntity");

            migrationBuilder.DropIndex(
                name: "IX_TasksEntity_ProjectId",
                table: "TasksEntity");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_TaskEntityId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "TasksEntity");

            migrationBuilder.DropColumn(
                name: "TaskEntityId",
                table: "Accounts");
        }
    }
}
