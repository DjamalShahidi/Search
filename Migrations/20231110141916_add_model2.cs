using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AmootSearch.Migrations
{
    public partial class add_model2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Models",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Index = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TotalNumber = table.Column<int>(type: "int", nullable: false),
                    Info = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Models_Index",
                table: "Models",
                column: "Index",
                unique: true,
                filter: "[Index] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Models");
        }
    }
}
