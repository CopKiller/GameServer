using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Database.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "NVARCHAR(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(60)", maxLength: 60, nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(20)", maxLength: 20, nullable: false),
                    SlotNumber = table.Column<byte>(type: "TINYINT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "DATE", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "DATE", nullable: false),
                    Level = table.Column<int>(type: "INT", nullable: false),
                    Experience = table.Column<int>(type: "INT", nullable: false),
                    Golds = table.Column<int>(type: "INT", nullable: false),
                    Diamonds = table.Column<int>(type: "INT", nullable: false),
                    Vitals_Id = table.Column<int>(type: "int", nullable: false),
                    Vitals_Health = table.Column<int>(type: "INT", nullable: false),
                    Vitals_MaxHealth = table.Column<double>(type: "float", nullable: false),
                    Vitals_Mana = table.Column<int>(type: "INT", nullable: false),
                    Vitals_MaxMana = table.Column<double>(type: "float", nullable: false),
                    Stats_Id = table.Column<int>(type: "int", nullable: false),
                    Stats_Strength = table.Column<int>(type: "INT", nullable: false),
                    Stats_Defense = table.Column<int>(type: "INT", nullable: false),
                    Stats_Agility = table.Column<int>(type: "INT", nullable: false),
                    Stats_Intelligence = table.Column<int>(type: "INT", nullable: false),
                    Stats_Willpower = table.Column<int>(type: "INT", nullable: false),
                    Position_Id = table.Column<int>(type: "int", nullable: false),
                    Position_X = table.Column<double>(type: "FLOAT", nullable: false),
                    Position_Y = table.Column<double>(type: "FLOAT", nullable: false),
                    Position_Z = table.Column<int>(type: "int", nullable: false),
                    Position_Rotation = table.Column<double>(type: "float", nullable: false),
                    AccountModelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Player_Account_AccountModelId",
                        column: x => x.AccountModelId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_Email",
                table: "Account",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_Username",
                table: "Account",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_AccountModelId",
                table: "Player",
                column: "AccountModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Name",
                table: "Player",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Account");
        }
    }
}
