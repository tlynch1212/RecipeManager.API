using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace RecipeManager.Core.Migrations
{
    public partial class importjobmanagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ImportJobs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalCount = table.Column<int>(nullable: false),
                    ImportedCount = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    StatusId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportJobs_ImportStatus_StatusId",
                        column: x => x.StatusId,
                        principalTable: "ImportStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ImportModel",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(nullable: true),
                    ingredients = table.Column<List<string>>(nullable: true),
                    instructions = table.Column<string>(nullable: true),
                    picture_link = table.Column<string>(nullable: true),
                    ImportJobId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ImportModel_ImportJobs_ImportJobId",
                        column: x => x.ImportJobId,
                        principalTable: "ImportJobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportJobs_StatusId",
                table: "ImportJobs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ImportModel_ImportJobId",
                table: "ImportModel",
                column: "ImportJobId");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportModel");

            migrationBuilder.DropTable(
                name: "ImportJobs");

            migrationBuilder.DropTable(
                name: "ImportStatus");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ingredient",
                table: "Ingredient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_Recipes_RecipeId",
                table: "Ingredient",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
