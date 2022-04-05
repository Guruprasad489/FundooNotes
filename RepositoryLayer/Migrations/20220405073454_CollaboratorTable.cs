using Microsoft.EntityFrameworkCore.Migrations;

namespace RepositoryLayer.Migrations
{
    public partial class CollaboratorTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    CollaboratorId = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailId = table.Column<string>(nullable: true),
                    NoteId = table.Column<long>(nullable: false),
                    UserId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.CollaboratorId);
                    table.ForeignKey(
                        name: "FK_Collaborators_notesEntityTable_NoteId",
                        column: x => x.NoteId,
                        principalTable: "notesEntityTable",
                        principalColumn: "NoteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Collaborators_UserEntityTable_UserId",
                        column: x => x.UserId,
                        principalTable: "UserEntityTable",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_NoteId",
                table: "Collaborators",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_UserId",
                table: "Collaborators",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Collaborators");
        }
    }
}
