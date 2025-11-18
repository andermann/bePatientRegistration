using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bePatientRegistration.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Convenios",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Convenios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pacientes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: true),
                    Rg = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UfRg = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MobilePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LandlinePhone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    HealthPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HealthPlanCardNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HealthPlanCardExpirationMonth = table.Column<int>(type: "int", nullable: false),
                    HealthPlanCardExpirationYear = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pacientes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pacientes_Convenios_HealthPlanId",
                        column: x => x.HealthPlanId,
                        principalTable: "Convenios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_HealthPlanId",
                table: "Pacientes",
                column: "HealthPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pacientes");

            migrationBuilder.DropTable(
                name: "Convenios");
        }
    }
}
