using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderRatings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductPresentation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Unit = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductPresentation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProductType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Semester",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semester", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInformation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    LastName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebpageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dni = table.Column<string>(type: "varchar(8)", unicode: false, maxLength: 8, nullable: true),
                    ImageUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    AvatarData = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInformation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CourseBySemester",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: true),
                    SemesterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBySemester", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBySemester_Semester",
                        column: x => x.SemesterId,
                        principalTable: "Semester",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__CourseByS__Cours__3587F3E0",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BankAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CCI = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserInformationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankAccounts_UserInformation_UserInformationId",
                        column: x => x.UserInformationId,
                        principalTable: "UserInformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserInformationId = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    AvatarUrl = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    AccountStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_UserInformation",
                        column: x => x.UserInformationId,
                        principalTable: "UserInformation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseBySemesterEvaluation",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    WeightPercentage = table.Column<int>(type: "int", nullable: false),
                    CourseBySemesterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBySemesterEvaluation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEvaluation_CourseBySemester",
                        column: x => x.CourseBySemesterId,
                        principalTable: "CourseBySemester",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseBySemesterId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schedule_CourseBySemester",
                        column: x => x.CourseBySemesterId,
                        principalTable: "CourseBySemester",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    HarvestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProductTypeId = table.Column<int>(type: "int", nullable: false),
                    ProductPresentationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ProductPresentation_ProductPresentationId",
                        column: x => x.ProductPresentationId,
                        principalTable: "ProductPresentation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_ProductType_ProductTypeId",
                        column: x => x.ProductTypeId,
                        principalTable: "ProductType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UpdateRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FieldName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OldValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdminComment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpdateRequests_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserByType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    UserTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserByType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserByType_User",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserByType_UserType",
                        column: x => x.UserTypeId,
                        principalTable: "UserType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductChatMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserIdFrom = table.Column<int>(type: "int", nullable: false),
                    UserIdTo = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    MessageContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ReadAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UserIdFromNavigationId = table.Column<int>(type: "int", nullable: true),
                    UserIdToNavigationId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductChatMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductChatMessage_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductChatMessage_User_UserIdFromNavigationId",
                        column: x => x.UserIdFromNavigationId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductChatMessage_User_UserIdToNavigationId",
                        column: x => x.UserIdToNavigationId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CourseBySemesterEnroll",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseBySemesterId = table.Column<int>(type: "int", nullable: false),
                    UserByTypeStudentId = table.Column<int>(type: "int", nullable: false),
                    UserByTypeTeacherId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseBySemesterEnroll", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEnroll_CourseBySemester",
                        column: x => x.CourseBySemesterId,
                        principalTable: "CourseBySemester",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEnroll_UserByTypeStudent",
                        column: x => x.UserByTypeStudentId,
                        principalTable: "UserByType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CourseBySemesterEnroll_UserByTypeTeacher",
                        column: x => x.UserByTypeTeacherId,
                        principalTable: "UserByType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PublishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ImageUrl = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    UserByTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_UserByType",
                        column: x => x.UserByTypeId,
                        principalTable: "UserByType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ChatMessageId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    HarvestDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_ProductChatMessage_ChatMessageId",
                        column: x => x.ChatMessageId,
                        principalTable: "ProductChatMessage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_User_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_User_SellerId",
                        column: x => x.SellerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Note",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<decimal>(type: "decimal(4,2)", nullable: true),
                    CourseBySemesterEvaluationId = table.Column<int>(type: "int", nullable: false),
                    CourseBySemesterEnrollId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Note", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Note_CourseBySemesterEnroll",
                        column: x => x.CourseBySemesterEnrollId,
                        principalTable: "CourseBySemesterEnroll",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Note_CourseBySemesterEvaluation",
                        column: x => x.CourseBySemesterEvaluationId,
                        principalTable: "CourseBySemesterEvaluation",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OrderRatings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    RaterUserId = table.Column<int>(type: "int", nullable: false),
                    RatedUserId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderRatings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderRatings_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderRatings_User_RatedUserId",
                        column: x => x.RatedUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderRatings_User_RaterUserId",
                        column: x => x.RaterUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankAccounts_UserInformationId",
                table: "BankAccounts",
                column: "UserInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemester_CourseId",
                table: "CourseBySemester",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemester_SemesterId",
                table: "CourseBySemester",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEnroll_CourseBySemesterId",
                table: "CourseBySemesterEnroll",
                column: "CourseBySemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEnroll_UserByTypeStudentId",
                table: "CourseBySemesterEnroll",
                column: "UserByTypeStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEnroll_UserByTypeTeacherId",
                table: "CourseBySemesterEnroll",
                column: "UserByTypeTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseBySemesterEvaluation_CourseBySemesterId",
                table: "CourseBySemesterEvaluation",
                column: "CourseBySemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductId",
                table: "FavoriteProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_CourseBySemesterEnrollId",
                table: "Note",
                column: "CourseBySemesterEnrollId");

            migrationBuilder.CreateIndex(
                name: "IX_Note_CourseBySemesterEvaluationId",
                table: "Note",
                column: "CourseBySemesterEvaluationId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRatings_OrderId",
                table: "OrderRatings",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRatings_RatedUserId",
                table: "OrderRatings",
                column: "RatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRatings_RaterUserId",
                table: "OrderRatings",
                column: "RaterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ChatMessageId",
                table: "Orders",
                column: "ChatMessageId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ProductId",
                table: "Orders",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SellerId",
                table: "Orders",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserByTypeId",
                table: "Post",
                column: "UserByTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductPresentationId",
                table: "Product",
                column: "ProductPresentationId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductTypeId",
                table: "Product",
                column: "ProductTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_UserId",
                table: "Product",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChatMessage_ProductId",
                table: "ProductChatMessage",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChatMessage_UserIdFromNavigationId",
                table: "ProductChatMessage",
                column: "UserIdFromNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductChatMessage_UserIdToNavigationId",
                table: "ProductChatMessage",
                column: "UserIdToNavigationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_CourseBySemesterId",
                table: "Schedule",
                column: "CourseBySemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_UpdateRequests_UserId",
                table: "UpdateRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserInformationId",
                table: "User",
                column: "UserInformationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserByType_UserId",
                table: "UserByType",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserByType_UserTypeId",
                table: "UserByType",
                column: "UserTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BankAccounts");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.DropTable(
                name: "Note");

            migrationBuilder.DropTable(
                name: "OrderRatings");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "UpdateRequests");

            migrationBuilder.DropTable(
                name: "CourseBySemesterEnroll");

            migrationBuilder.DropTable(
                name: "CourseBySemesterEvaluation");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "UserByType");

            migrationBuilder.DropTable(
                name: "CourseBySemester");

            migrationBuilder.DropTable(
                name: "ProductChatMessage");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropTable(
                name: "Semester");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductPresentation");

            migrationBuilder.DropTable(
                name: "ProductType");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "UserInformation");
        }
    }
}
