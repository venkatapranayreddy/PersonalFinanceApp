using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pratice.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGoalAmountAndStepUps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RetirementGoalAmount",
                table: "RetirementProfiles",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "InvestmentStepUps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RetirementProfileId = table.Column<Guid>(type: "uuid", nullable: false),
                    AfterMonth = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentStepUps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentStepUps_RetirementProfiles_RetirementProfileId",
                        column: x => x.RetirementProfileId,
                        principalTable: "RetirementProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentStepUps_RetirementProfileId",
                table: "InvestmentStepUps",
                column: "RetirementProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentStepUps");

            migrationBuilder.DropColumn(
                name: "RetirementGoalAmount",
                table: "RetirementProfiles");
        }
    }
}
