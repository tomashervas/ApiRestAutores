using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace apiAutores.Migrations
{
    /// <inheritdoc />
    public partial class ComentarioUsario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UsuarioId",
                table: "Commentarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Commentarios_UsuarioId",
                table: "Commentarios",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commentarios_AspNetUsers_UsuarioId",
                table: "Commentarios",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commentarios_AspNetUsers_UsuarioId",
                table: "Commentarios");

            migrationBuilder.DropIndex(
                name: "IX_Commentarios_UsuarioId",
                table: "Commentarios");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Commentarios");
        }
    }
}
