using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Snapsoft.Dora.Adapter.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddComponentDeployment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentDeployments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    ComponentId = table.Column<long>(type: "bigint", nullable: false),
                    Version = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CommitId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TimestampTz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDeployments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentDeployments_Components_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "Components",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeployments_ComponentId_CommitId",
                table: "ComponentDeployments",
                columns: new[] { "ComponentId", "CommitId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDeployments_ComponentId_Version",
                table: "ComponentDeployments",
                columns: new[] { "ComponentId", "Version" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentDeployments");
        }
    }
}
