using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class talents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserTalents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Pyro = table.Column<int>(type: "int", nullable: false),
                    Cryo = table.Column<int>(type: "int", nullable: false),
                    Hydro = table.Column<int>(type: "int", nullable: false),
                    Electro = table.Column<int>(type: "int", nullable: false),
                    Anemo = table.Column<int>(type: "int", nullable: false),
                    Geo = table.Column<int>(type: "int", nullable: false),
                    Dendro = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTalents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTalents");
        }
    }
}
