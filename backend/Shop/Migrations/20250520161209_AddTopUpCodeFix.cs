using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Shop.Migrations
{
    /// <inheritdoc />
    public partial class AddTopUpCodeFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
           name: "TopUpCodes",
           columns: table => new
           {
               TopUpCodeId = table.Column<int>(nullable: false)
                   .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
               Code = table.Column<string>(maxLength: 50, nullable: false),
               Amount = table.Column<int>(nullable: false),
               IsUsed = table.Column<bool>(nullable: false),
               CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
               UsedByUserId = table.Column<int>(nullable: true)
           },
           constraints: table =>
           {
               table.PrimaryKey("PK_TopUpCodes", x => x.TopUpCodeId);
               table.ForeignKey(
                   name: "FK_TopUpCodes_Users_UsedByUserId",
                   column: x => x.UsedByUserId,
                   principalTable: "Users",
                   principalColumn: "Id");
           });

                migrationBuilder.CreateIndex(
                    name: "IX_TopUpCodes_UsedByUserId",
                    table: "TopUpCodes",
                    column: "UsedByUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TopUpCodes");

        }
    }
}
