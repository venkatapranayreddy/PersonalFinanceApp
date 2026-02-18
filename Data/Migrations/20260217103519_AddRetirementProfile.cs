using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pratice.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRetirementProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RetirementProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CurrentAge = table.Column<int>(type: "integer", nullable: false),
                    EmploymentStatus = table.Column<int>(type: "integer", nullable: false),
                    RetirementAge = table.Column<int>(type: "integer", nullable: false),
                    MonthlyIncome = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MonthlyExpenses = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    MonthlyInvestment = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ExpectedReturnRate = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RetirementProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RetirementProfiles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RetirementProfiles_UserId",
                table: "RetirementProfiles",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RetirementProfiles");
        }
    }
}
