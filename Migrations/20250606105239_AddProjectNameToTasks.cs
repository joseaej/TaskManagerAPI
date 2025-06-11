using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TasksManagerAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectNameToTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_Task_TasksId",
                table: "Accounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_Projects_ProjectId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Task_ProjectId",
                table: "Task");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_TasksId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "TasksId",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "AccountsUsername",
                table: "Task",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectName",
                table: "Task",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountsUsername",
                table: "Task");

            migrationBuilder.DropColumn(
                name: "ProjectName",
                table: "Task");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "Task",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TasksId",
                table: "Accounts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Task_ProjectId",
                table: "Task",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_TasksId",
                table: "Accounts",
                column: "TasksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_Task_TasksId",
                table: "Accounts",
                column: "TasksId",
                principalTable: "Task",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_Projects_ProjectId",
                table: "Task",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
