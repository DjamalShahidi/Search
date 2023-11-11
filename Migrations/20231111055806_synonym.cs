using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmootSearch.Migrations
{
    public partial class synonym : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Synonyms",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Main = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Equivalent = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Synonyms", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_Equivalent",
                table: "Synonyms",
                column: "Equivalent");

            migrationBuilder.CreateIndex(
                name: "IX_Synonyms_Main",
                table: "Synonyms",
                column: "Main");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Synonyms");
        }
    }
}
