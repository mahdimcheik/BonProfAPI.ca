using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BonProfCa.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryCursuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCursuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LevelCursuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelCursuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrivacyDocumentTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyDocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Color = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusAccount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusAccount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusReservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusReservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatusTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatusTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeAddresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeAddresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeSlots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSlots", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TypeTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    LastName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImgUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    DataProcessingConsent = table.Column<bool>(type: "boolean", nullable: false),
                    PrivacyPolicyConsent = table.Column<bool>(type: "boolean", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    GenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Genders_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Genders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_StatusAccount_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Street = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    City = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Country = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ZipCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    AdditionalInfo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    Longitude = table.Column<float>(type: "real", nullable: true),
                    Latitude = table.Column<float>(type: "real", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_TypeAddresses_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TypeAddresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Addresses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleAppId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserAppId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Roles_RoleAppId",
                        column: x => x.RoleAppId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserAppId",
                        column: x => x.UserAppId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Formations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Institute = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    DateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Formations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserAppId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Languages_Users_UserAppId",
                        column: x => x.UserAppId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    IsSeen = table.Column<bool>(type: "boolean", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    LinkedIn = table.Column<string>(type: "text", nullable: true),
                    FaceBook = table.Column<string>(type: "text", nullable: true),
                    GitHub = table.Column<string>(type: "text", nullable: true),
                    Twitter = table.Column<string>(type: "text", nullable: true),
                    PriceIndicative = table.Column<decimal>(type: "numeric", nullable: false),
                    IsProfessionnal = table.Column<bool>(type: "boolean", nullable: false),
                    Siret = table.Column<long>(type: "bigint", nullable: true),
                    LanguageId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Teachers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Cursuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    LevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Color = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    Icon = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cursuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cursuses_LevelCursuses_LevelId",
                        column: x => x.LevelId,
                        principalTable: "LevelCursuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cursuses_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    Company = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Experiences_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Slots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DateFrom = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTo = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Slots_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Slots_TypeSlots_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TypeSlots",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TeacherPayouts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherPayouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherPayouts_StatusTransactions_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TeacherPayouts_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CategoryCursusCursus",
                columns: table => new
                {
                    CategoriesId = table.Column<Guid>(type: "uuid", nullable: false),
                    CursusesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryCursusCursus", x => new { x.CategoriesId, x.CursusesId });
                    table.ForeignKey(
                        name: "FK_CategoryCursusCursus_CategoryCursuses_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "CategoryCursuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryCursusCursus_Cursuses_CursusesId",
                        column: x => x.CursusesId,
                        principalTable: "Cursuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    CursusId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Cursuses_CursusId",
                        column: x => x.CursusId,
                        principalTable: "Cursuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: false),
                    SenderId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Conversations_Users_SenderId",
                        column: x => x.SenderId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    MimeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: true),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Documents_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PrivacyDocuments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacyDocuments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrivacyDocuments_Documents_Id",
                        column: x => x.Id,
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrivacyDocuments_PrivacyDocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "PrivacyDocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    ReductionPercentage = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    ReductionAmount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_StatusOrders_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    MethodId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransactionRef = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_MethodId",
                        column: x => x.MethodId,
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    SlotId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatusId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reservations_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Slots_SlotId",
                        column: x => x.SlotId,
                        principalTable: "Slots",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_StatusReservations_StatusId",
                        column: x => x.StatusId,
                        principalTable: "StatusReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TeacherWalletTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    ReservationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ArchivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeacherWalletTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TeacherWalletTransactions_Reservations_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservations",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TeacherWalletTransactions_TypeTransactions_TypeId",
                        column: x => x.TypeId,
                        principalTable: "TypeTransactions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CategoryCursuses",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0b6865c4-76fc-4caa-8171-07f449ca6e5c"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Front", null },
                    { new Guid("4133e2f8-f0e0-44f8-8cfb-8a8e1aaa86d7"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Back", null },
                    { new Guid("86969bd8-e51f-4558-ac44-fc159ed31c53"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Soft", null },
                    { new Guid("a7f8b05d-3d2d-43fa-870d-987c21f4e41d"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Technics", null }
                });

            migrationBuilder.InsertData(
                table: "Genders",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b07b2445-f39c-4b26-8ffe-e40fe561d8bc"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Other", null },
                    { new Guid("b68c151b-db34-462d-a65c-90989cc96e5e"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Female", null },
                    { new Guid("dcb8b01b-205a-4ea2-a281-004a7b1bb972"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Male", null }
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt", "UserAppId" },
                values: new object[,]
                {
                    { new Guid("3aa916ed-53d2-4f93-80e9-b49171a7ebe1"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "English", null, null },
                    { new Guid("52b54b82-1f37-4a66-a263-708b53cd685d"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "French", null, null },
                    { new Guid("ff34f5ba-6201-45bf-9217-dcda019976a3"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Arab", null, null }
                });

            migrationBuilder.InsertData(
                table: "LevelCursuses",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("19702cef-0a3b-46f8-932b-cf78634e741d"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Intermediate", null },
                    { new Guid("a518f61e-8a19-44e6-b365-fb285ad0811e"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Advanced", null },
                    { new Guid("b70134eb-060c-4069-a325-1251c75a1ac9"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "All", null },
                    { new Guid("eb4bd576-6855-4123-bb92-e921c8610542"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Beginner", null }
                });

            migrationBuilder.InsertData(
                table: "PrivacyDocumentTypes",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "DisplayName", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("3817d1eb-594e-40ba-8544-959121aaa179"), null, "#1169b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pièce d'identité", null, "Identification", null },
                    { new Guid("8bcfa1b8-70bb-4e9c-a1d1-e1c64230a579"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "K-Bis", null, "CompanyRegistration", null },
                    { new Guid("d918182e-d9d7-452c-9607-ab83f78139d0"), null, "#aa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Diplôme", null, "Diploma", null },
                    { new Guid("f709ecfd-82f3-4d07-851f-419e9016135f"), null, "#1169b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Autre", null, "Other", null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ArchivedAt", "Color", "ConcurrencyStamp", "CreatedAt", "Name", "NormalizedName", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4a5eaf2f-0496-4035-a4b7-9210da39501c"), null, "#31bdd6", "ADMIN-STAMP-2025", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Admin", "ADMIN", null },
                    { new Guid("87a0a5ed-c7bb-4394-a163-7ed7560b3703"), null, "#31d68f", "TEACHER-STAMP-2025", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Teacher", "TEACHER", null },
                    { new Guid("87a0a5ed-c7bb-4394-a163-7ed7560b4a01"), null, "#f57ad0", "STUDENT-STAMP-2025", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Student", "STUDENT", null },
                    { new Guid("bde5556b-562d-431f-9ff9-d31a5f5cb8c5"), null, "#3d82f2", "SUPERADMIN-STAMP-2025", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "SuperAdmin", "SUPERADMIN", null }
                });

            migrationBuilder.InsertData(
                table: "StatusAccount",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("12e65875-bec4-422a-aad4-c28de0e06fff"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Active", null },
                    { new Guid("acc9ddb6-824b-42cb-8276-5d91b6af2003"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Pending", null },
                    { new Guid("fd2621ef-85f0-46a6-acdd-c77fb111630f"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Banned", null }
                });

            migrationBuilder.InsertData(
                table: "StatusOrders",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2a6822ed-9116-47b7-a225-be832db4beb8"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "PartialRefund", null },
                    { new Guid("36fe7f5e-61f3-4ce1-88bb-8942d3b094a1"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Paid", null },
                    { new Guid("4470502b-fce9-4d5f-913c-3340af86b532"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Pending", null },
                    { new Guid("8b84e43c-d208-4c18-9c23-2b13438eb109"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "FullRefund", null }
                });

            migrationBuilder.InsertData(
                table: "StatusReservations",
                columns: new[] { "Id", "ArchivedAt", "Code", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("2ec60a91-aab8-4753-a5d8-b131b9441e77"), null, 0, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Pending", null },
                    { new Guid("32d854b6-6d4e-445a-9209-31a492970f2d"), null, 1, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Accepted", null },
                    { new Guid("6d281aec-d093-4071-8bf4-c8363361b5b4"), null, 3, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Done", null },
                    { new Guid("c5420623-8b51-4ce9-9ec1-3bc546b9ffff"), null, 4, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Cancelled", null },
                    { new Guid("caab85f5-d37b-4ea0-b035-5ba3ca8dd49f"), null, 2, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Rejected", null }
                });

            migrationBuilder.InsertData(
                table: "StatusTransactions",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("584f233a-bf58-4db9-a24e-90baac3f6d42"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Failed", null },
                    { new Guid("73f7fa42-196b-4040-b727-64a7b1e56458"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Paid", null },
                    { new Guid("ead0ecc1-a58d-4436-a87f-89c2dbc665a8"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Pending", null }
                });

            migrationBuilder.InsertData(
                table: "TypeAddresses",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "DisplayName", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("b8b8a8fc-ca60-440b-815f-1e44b89c9803"), null, "#fa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Facturation", "", "Billing", null },
                    { new Guid("e1fee3ea-6190-48c3-8e40-c1f053fea79d"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Principale", "", "Main", null }
                });

            migrationBuilder.InsertData(
                table: "TypeSlots",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("4043e32b-4d92-49b5-b885-505155ff2fe9"), null, "#aa69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi pi-desktop", "Visio", null },
                    { new Guid("79f538c3-5f2b-4e45-a5f8-4d7cda8b3df8"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi pi-arrow-down-left-and-arrow-up-right-to-center", "Presential", null },
                    { new Guid("c25c18a2-af88-4132-a27a-0025417edb56"), null, "#1169b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi pi-crown", "All", null }
                });

            migrationBuilder.InsertData(
                table: "TypeTransactions",
                columns: new[] { "Id", "ArchivedAt", "Color", "CreatedAt", "Icon", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("50412518-6c82-40c1-b6bf-b9c7aadf67d1"), null, "#ab69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Refund", null },
                    { new Guid("7305a3d5-bfa9-41ce-be10-174c406cb842"), null, "#ff69b4", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "", "Payment", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_TypeId",
                table: "Addresses",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleAppId",
                table: "AspNetUserRoles",
                column: "RoleAppId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserAppId",
                table: "AspNetUserRoles",
                column: "UserAppId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryCursusCursus_CursusesId",
                table: "CategoryCursusCursus",
                column: "CursusesId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_ReservationId",
                table: "Conversations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_SenderId",
                table: "Conversations",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursuses_LevelId",
                table: "Cursuses",
                column: "LevelId");

            migrationBuilder.CreateIndex(
                name: "IX_Cursuses_TeacherId",
                table: "Cursuses",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ReservationId",
                table: "Documents",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StudentId",
                table: "Documents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TeacherId",
                table: "Documents",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_TeacherId",
                table: "Experiences",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Formations_UserId",
                table: "Formations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Languages_UserAppId",
                table: "Languages",
                column: "UserAppId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                table: "Orders",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusId",
                table: "Orders",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StudentId",
                table: "Orders",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TeacherId",
                table: "Orders",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MethodId",
                table: "Payments",
                column: "MethodId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PrivacyDocuments_TypeId",
                table: "PrivacyDocuments",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CursusId",
                table: "Products",
                column: "CursusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_OrderId",
                table: "Reservations",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ProductId",
                table: "Reservations",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_SlotId",
                table: "Reservations",
                column: "SlotId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_StatusId",
                table: "Reservations",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_StudentId",
                table: "Reservations",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Slots_TeacherId",
                table: "Slots",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Slots_TypeId",
                table: "Slots",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_UserId",
                table: "Students",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPayouts_StatusId",
                table: "TeacherPayouts",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherPayouts_TeacherId",
                table: "TeacherPayouts",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_LanguageId",
                table: "Teachers",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_UserId",
                table: "Teachers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TeacherWalletTransactions_ReservationId",
                table: "TeacherWalletTransactions",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_TeacherWalletTransactions_TypeId",
                table: "TeacherWalletTransactions",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Users_GenderId",
                table: "Users",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatusId",
                table: "Users",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Reservations_ReservationId",
                table: "Conversations",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Reservations_ReservationId",
                table: "Documents",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Languages_Users_UserAppId",
                table: "Languages");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_Users_UserId",
                table: "Students");

            migrationBuilder.DropForeignKey(
                name: "FK_Teachers_Users_UserId",
                table: "Teachers");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Teachers_TeacherId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Students_StudentId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CategoryCursusCursus");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "Formations");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "PrivacyDocuments");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "TeacherPayouts");

            migrationBuilder.DropTable(
                name: "TeacherWalletTransactions");

            migrationBuilder.DropTable(
                name: "TypeAddresses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CategoryCursuses");

            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropTable(
                name: "PrivacyDocumentTypes");

            migrationBuilder.DropTable(
                name: "StatusTransactions");

            migrationBuilder.DropTable(
                name: "TypeTransactions");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Slots");

            migrationBuilder.DropTable(
                name: "StatusReservations");

            migrationBuilder.DropTable(
                name: "Cursuses");

            migrationBuilder.DropTable(
                name: "TypeSlots");

            migrationBuilder.DropTable(
                name: "LevelCursuses");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Genders");

            migrationBuilder.DropTable(
                name: "StatusAccount");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "StatusOrders");
        }
    }
}
