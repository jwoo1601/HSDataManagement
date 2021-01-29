using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HyosungManagement.Migrations
{
    public partial class DataMigrations4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    Content = table.Column<string>(nullable: true),
                    CategoryID = table.Column<int>(nullable: true),
                    WriterID = table.Column<string>(nullable: false),
                    ViewCount = table.Column<long>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Post_PostCategory_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "PostCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Post_Likes",
                columns: table => new
                {
                    LikedBy = table.Column<string>(nullable: false),
                    OwnerID = table.Column<int>(nullable: false),
                    LikedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post_Likes", x => new { x.LikedBy, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_Post_Likes_Post_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "Post",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComment",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    PostID = table.Column<int>(nullable: false),
                    WriterID = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PostComment_Post_PostID",
                        column: x => x.PostID,
                        principalTable: "Post",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentReply",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(maxLength: 1000, nullable: false),
                    CommentID = table.Column<int>(nullable: false),
                    WriterID = table.Column<string>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    LastUpdatedAt = table.Column<DateTime>(nullable: false, defaultValueSql: "GETUTCDATE()"),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReply", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CommentReply_PostComment_CommentID",
                        column: x => x.CommentID,
                        principalTable: "PostComment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PostComment_Likes",
                columns: table => new
                {
                    LikedBy = table.Column<string>(nullable: false),
                    OwnerID = table.Column<int>(nullable: false),
                    LikedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComment_Likes", x => new { x.LikedBy, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_PostComment_Likes_PostComment_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "PostComment",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CommentReply_Likes",
                columns: table => new
                {
                    LikedBy = table.Column<string>(nullable: false),
                    OwnerID = table.Column<int>(nullable: false),
                    LikedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentReply_Likes", x => new { x.LikedBy, x.OwnerID });
                    table.ForeignKey(
                        name: "FK_CommentReply_Likes_CommentReply_OwnerID",
                        column: x => x.OwnerID,
                        principalTable: "CommentReply",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentReply_CommentID",
                table: "CommentReply",
                column: "CommentID");

            migrationBuilder.CreateIndex(
                name: "IX_CommentReply_Likes_OwnerID",
                table: "CommentReply_Likes",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryID",
                table: "Post",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Post_Likes_OwnerID",
                table: "Post_Likes",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_PostID",
                table: "PostComment",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_PostComment_Likes_OwnerID",
                table: "PostComment_Likes",
                column: "OwnerID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentReply_Likes");

            migrationBuilder.DropTable(
                name: "Post_Likes");

            migrationBuilder.DropTable(
                name: "PostComment_Likes");

            migrationBuilder.DropTable(
                name: "CommentReply");

            migrationBuilder.DropTable(
                name: "PostComment");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "PostCategory");
        }
    }
}
