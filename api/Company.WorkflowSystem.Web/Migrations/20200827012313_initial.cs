using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Company.WorkflowSystem.Web.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Address",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true),
                    ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Address", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AttachmentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Other = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttachmentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationGroup",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Identifier = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealCategories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    UnitOfMeasure = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealCodeSequences",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    NextSequence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealCodeSequences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealItemFieldsets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealItemFieldsets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DealItemSourceData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<long>(nullable: true),
                    Type = table.Column<int>(nullable: true),
                    CreationDate = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealItemSourceData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Functionalities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FunctionalityEnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Functionalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesForecasts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MonthYear = table.Column<DateTimeOffset>(nullable: false),
                    Volume = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesForecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TraderAuthorityPolicies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderAuthorityPolicies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ApprovalLevel = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigurationEntry",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConfigurationGroupId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Identifier = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    ContentType = table.Column<int>(nullable: false),
                    FunctionalityForLookup = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigurationEntry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigurationEntry_ConfigurationGroup_ConfigurationGroupId",
                        column: x => x.ConfigurationGroupId,
                        principalTable: "ConfigurationGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Counterparties",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    AddressId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true),
                    CompanyNumber = table.Column<string>(nullable: true),
                    BusinessNumber = table.Column<string>(nullable: true),
                    NzemParticipant = table.Column<bool>(nullable: true),
                    NzemParticipantId = table.Column<string>(nullable: true),
                    Conditions = table.Column<string>(nullable: true),
                    ExposureLimit = table.Column<decimal>(nullable: false),
                    ExpiryDate = table.Column<DateTimeOffset>(nullable: true),
                    ApprovalDate = table.Column<DateTimeOffset>(nullable: true),
                    SecurityHeld = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counterparties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Counterparties_Address_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Address",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Counterparties_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DealCategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_DealCategories_DealCategoryId",
                        column: x => x.DealCategoryId,
                        principalTable: "DealCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealItemFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealItemFieldsetId = table.Column<int>(nullable: false),
                    DisplayOrder = table.Column<int>(nullable: false),
                    Field = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Execution = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealItemFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealItemFields_DealItemFieldsets_DealItemFieldsetId",
                        column: x => x.DealItemFieldsetId,
                        principalTable: "DealItemFieldsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubFunctionalities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    FunctionalityId = table.Column<int>(nullable: false),
                    ParentSubFunctionalityId = table.Column<int>(nullable: true),
                    SubFunctionalityEnum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFunctionalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubFunctionalities_Functionalities_FunctionalityId",
                        column: x => x.FunctionalityId,
                        principalTable: "Functionalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubFunctionalities_SubFunctionalities_ParentSubFunctionalityId",
                        column: x => x.ParentSubFunctionalityId,
                        principalTable: "SubFunctionalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FunctionalitiesInUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserRoleId = table.Column<int>(nullable: false),
                    FunctionalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionalitiesInUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FunctionalitiesInUserRoles_Functionalities_FunctionalityId",
                        column: x => x.FunctionalityId,
                        principalTable: "Functionalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FunctionalitiesInUserRoles_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Username = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    UserRoleId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_UserRoles_UserRoleId",
                        column: x => x.UserRoleId,
                        principalTable: "UserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TraderAuthorityPoliciesCriteria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    TraderAuthorityPolicyId = table.Column<int>(nullable: false),
                    WorkflowRoleId = table.Column<int>(nullable: false),
                    OnlyBuy = table.Column<bool>(nullable: false),
                    OnlySell = table.Column<bool>(nullable: false),
                    MaxBuyVolume = table.Column<decimal>(nullable: true),
                    MaxSellVolume = table.Column<decimal>(nullable: true),
                    MaxVolume = table.Column<decimal>(nullable: true),
                    MaxVolumeForecastPercentage = table.Column<decimal>(nullable: true),
                    MaxAcquisitionCost = table.Column<decimal>(nullable: true),
                    MaxSellAcquisitionCost = table.Column<decimal>(nullable: true),
                    MaxTermInMonths = table.Column<int>(nullable: true),
                    MaxDurationInMonths = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraderAuthorityPoliciesCriteria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TraderAuthorityPoliciesCriteria_TraderAuthorityPolicies_TraderAuthorityPolicyId",
                        column: x => x.TraderAuthorityPolicyId,
                        principalTable: "TraderAuthorityPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraderAuthorityPoliciesCriteria_WorkflowRoles_WorkflowRoleId",
                        column: x => x.WorkflowRoleId,
                        principalTable: "WorkflowRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<int>(nullable: false),
                    ForcePosition = table.Column<bool>(nullable: false),
                    UnitOfMeasure = table.Column<string>(nullable: true),
                    HasLossFactors = table.Column<bool>(nullable: false),
                    HasExpiryDate = table.Column<bool>(nullable: false),
                    HasDelegatedAuthority = table.Column<bool>(nullable: false),
                    CanChangeProductOnExecution = table.Column<bool>(nullable: false),
                    DealItemFieldsetId = table.Column<int>(nullable: true),
                    WorkflowSetId = table.Column<int>(nullable: true),
                    TraderAuthorityPolicyId = table.Column<int>(nullable: true),
                    ItemExecutionImportTemplateType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealTypes_DealItemFieldsets_DealItemFieldsetId",
                        column: x => x.DealItemFieldsetId,
                        principalTable: "DealItemFieldsets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealTypes_TraderAuthorityPolicies_TraderAuthorityPolicyId",
                        column: x => x.TraderAuthorityPolicyId,
                        principalTable: "TraderAuthorityPolicies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealTypes_WorkflowSets_WorkflowSetId",
                        column: x => x.WorkflowSetId,
                        principalTable: "WorkflowSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowSetId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    AssignmentType = table.Column<int>(nullable: false),
                    WorkflowRoleId = table.Column<int>(nullable: true),
                    AllowsDealEditing = table.Column<bool>(nullable: false),
                    FinalizeDeal = table.Column<bool>(nullable: false),
                    CancelDeal = table.Column<bool>(nullable: false),
                    AllowsEditDelegatedAuthority = table.Column<bool>(nullable: false),
                    AllowsDealExecution = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStatuses_WorkflowRoles_WorkflowRoleId",
                        column: x => x.WorkflowRoleId,
                        principalTable: "WorkflowRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowStatuses_WorkflowSets_WorkflowSetId",
                        column: x => x.WorkflowSetId,
                        principalTable: "WorkflowSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CounterpartiesInDealCategories",
                columns: table => new
                {
                    DealCategoryId = table.Column<int>(nullable: false),
                    CounterpartyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CounterpartiesInDealCategories", x => new { x.DealCategoryId, x.CounterpartyId });
                    table.ForeignKey(
                        name: "FK_CounterpartiesInDealCategories_Counterparties_CounterpartyId",
                        column: x => x.CounterpartyId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CounterpartiesInDealCategories_DealCategories_DealCategoryId",
                        column: x => x.DealCategoryId,
                        principalTable: "DealCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubFunctionalitiesInUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FunctionalityInUserRoleId = table.Column<int>(nullable: false),
                    SubFunctionalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubFunctionalitiesInUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubFunctionalitiesInUserRoles_FunctionalitiesInUserRoles_FunctionalityInUserRoleId",
                        column: x => x.FunctionalityInUserRoleId,
                        principalTable: "FunctionalitiesInUserRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubFunctionalitiesInUserRoles_SubFunctionalities_SubFunctionalityId",
                        column: x => x.SubFunctionalityId,
                        principalTable: "SubFunctionalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTimeOffset>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FunctionalityId = table.Column<int>(nullable: false),
                    EntityId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntries_Functionalities_FunctionalityId",
                        column: x => x.FunctionalityId,
                        principalTable: "Functionalities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationRuns",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Started = table.Column<DateTimeOffset>(nullable: false),
                    Ended = table.Column<DateTimeOffset>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Payload = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationRuns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationRuns_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserIntegrationData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    IntegrationType = table.Column<int>(nullable: false),
                    Field = table.Column<int>(nullable: false),
                    Data = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserIntegrationData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserIntegrationData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersInWorkflowRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    WorkflowRoleId = table.Column<int>(nullable: false),
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersInWorkflowRoles", x => new { x.UserId, x.WorkflowRoleId });
                    table.ForeignKey(
                        name: "FK_UsersInWorkflowRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersInWorkflowRoles_WorkflowRoles_WorkflowRoleId",
                        column: x => x.WorkflowRoleId,
                        principalTable: "WorkflowRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealTypesInDealCategories",
                columns: table => new
                {
                    DealCategoryId = table.Column<int>(nullable: false),
                    DealTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealTypesInDealCategories", x => new { x.DealCategoryId, x.DealTypeId });
                    table.ForeignKey(
                        name: "FK_DealTypesInDealCategories_DealCategories_DealCategoryId",
                        column: x => x.DealCategoryId,
                        principalTable: "DealCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealTypesInDealCategories_DealTypes_DealTypeId",
                        column: x => x.DealTypeId,
                        principalTable: "DealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowActions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    SourceWorkflowStatusId = table.Column<int>(nullable: true),
                    TargetWorkflowStatusId = table.Column<int>(nullable: false),
                    TargetAlternateDescriptionSuffix = table.Column<string>(nullable: true),
                    DirectActionOnEmailNotification = table.Column<bool>(nullable: false),
                    IsSubmission = table.Column<bool>(nullable: false),
                    PerformsExecutionAutomatically = table.Column<bool>(nullable: false),
                    CantBePerformedBySameUser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowActions_WorkflowStatuses_SourceWorkflowStatusId",
                        column: x => x.SourceWorkflowStatusId,
                        principalTable: "WorkflowStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowActions_WorkflowStatuses_TargetWorkflowStatusId",
                        column: x => x.TargetWorkflowStatusId,
                        principalTable: "WorkflowStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryTables",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditEntryId = table.Column<int>(nullable: false),
                    TableName = table.Column<string>(maxLength: 128, nullable: false),
                    Action = table.Column<string>(maxLength: 50, nullable: false),
                    KeyValues = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryTables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntryTables_AuditEntries_AuditEntryId",
                        column: x => x.AuditEntryId,
                        principalTable: "AuditEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationRunEntries",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IntegrationRunId = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    Details = table.Column<string>(nullable: true),
                    Payload = table.Column<string>(nullable: true),
                    AffectedId = table.Column<int>(nullable: true),
                    FunctionalityOfAffectedId = table.Column<int>(nullable: true),
                    DateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationRunEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationRunEntries_IntegrationRuns_IntegrationRunId",
                        column: x => x.IntegrationRunId,
                        principalTable: "IntegrationRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditEntryFields",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuditEntryTableId = table.Column<int>(nullable: false),
                    FieldName = table.Column<string>(nullable: true),
                    OldValue = table.Column<string>(nullable: true),
                    NewValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditEntryFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditEntryFields_AuditEntryTables_AuditEntryTableId",
                        column: x => x.AuditEntryTableId,
                        principalTable: "AuditEntryTables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealId = table.Column<int>(nullable: false),
                    AttachmentTypeId = table.Column<int>(nullable: true),
                    AttachmentTypeOtherText = table.Column<string>(nullable: true),
                    LinkType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealAttachments_AttachmentTypes_AttachmentTypeId",
                        column: x => x.AttachmentTypeId,
                        principalTable: "AttachmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealAttachmentVersions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealAttachmentId = table.Column<int>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FileExtension = table.Column<string>(nullable: true),
                    File = table.Column<byte[]>(nullable: true),
                    FileSizeInBytes = table.Column<long>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreationUserId = table.Column<int>(nullable: false),
                    IsLocked = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealAttachmentVersions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealAttachmentVersions_Users_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealAttachmentVersions_DealAttachments_DealAttachmentId",
                        column: x => x.DealAttachmentId,
                        principalTable: "DealAttachments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTaskAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    WorkflowTaskId = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    AttachmentTypeToVerifyId = table.Column<int>(nullable: true),
                    AlternateWorkflowActionId = table.Column<int>(nullable: true),
                    AnswerType = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTaskAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskAnswers_WorkflowActions_AlternateWorkflowActionId",
                        column: x => x.AlternateWorkflowActionId,
                        principalTable: "WorkflowActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskAnswers_AttachmentTypes_AttachmentTypeToVerifyId",
                        column: x => x.AttachmentTypeToVerifyId,
                        principalTable: "AttachmentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Active = table.Column<bool>(nullable: false),
                    WorkflowActionId = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Mandatory = table.Column<bool>(nullable: false),
                    PrecedingAnswerId = table.Column<int>(nullable: true),
                    DependingUponAnswerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTasks_WorkflowTaskAnswers_DependingUponAnswerId",
                        column: x => x.DependingUponAnswerId,
                        principalTable: "WorkflowTaskAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTasks_WorkflowTaskAnswers_PrecedingAnswerId",
                        column: x => x.PrecedingAnswerId,
                        principalTable: "WorkflowTaskAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkflowTasks_WorkflowActions_WorkflowActionId",
                        column: x => x.WorkflowActionId,
                        principalTable: "WorkflowActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkflowTaskInDealTypes",
                columns: table => new
                {
                    WorkflowTaskId = table.Column<int>(nullable: false),
                    DealTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTaskInDealTypes", x => new { x.WorkflowTaskId, x.DealTypeId });
                    table.ForeignKey(
                        name: "FK_WorkflowTaskInDealTypes_DealTypes_DealTypeId",
                        column: x => x.DealTypeId,
                        principalTable: "DealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowTaskInDealTypes_WorkflowTasks_WorkflowTaskId",
                        column: x => x.WorkflowTaskId,
                        principalTable: "WorkflowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Deals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreationDate = table.Column<DateTimeOffset>(nullable: false),
                    CreationUserId = table.Column<int>(nullable: false),
                    DealNumber = table.Column<string>(nullable: false),
                    CounterpartyId = table.Column<int>(nullable: false),
                    DealCategoryId = table.Column<int>(nullable: false),
                    DealTypeId = table.Column<int>(nullable: false),
                    ForceMajeure = table.Column<bool>(nullable: false),
                    ExpiryDate = table.Column<DateTimeOffset>(nullable: true),
                    DealItemFieldsetId = table.Column<int>(nullable: true),
                    WorkflowSetId = table.Column<int>(nullable: true),
                    CurrentDealWorkflowStatusId = table.Column<int>(nullable: true),
                    OngoingWorkflowActionId = table.Column<int>(nullable: true),
                    NextDealWorkflowStatusId = table.Column<int>(nullable: true),
                    PreviousDealWorkflowStatusId = table.Column<int>(nullable: true),
                    DelegatedAuthorityUserId = table.Column<int>(nullable: true),
                    SubmissionDate = table.Column<DateTimeOffset>(nullable: true),
                    SubmissionUserId = table.Column<int>(nullable: true),
                    Executed = table.Column<bool>(nullable: false),
                    ExecutionDate = table.Column<DateTimeOffset>(nullable: true),
                    ExecutionUserId = table.Column<int>(nullable: true),
                    TermInMonthsOverride = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deals_Counterparties_CounterpartyId",
                        column: x => x.CounterpartyId,
                        principalTable: "Counterparties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_Users_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_DealCategories_DealCategoryId",
                        column: x => x.DealCategoryId,
                        principalTable: "DealCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_DealTypes_DealTypeId",
                        column: x => x.DealTypeId,
                        principalTable: "DealTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deals_Users_DelegatedAuthorityUserId",
                        column: x => x.DelegatedAuthorityUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_Users_ExecutionUserId",
                        column: x => x.ExecutionUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_WorkflowActions_OngoingWorkflowActionId",
                        column: x => x.OngoingWorkflowActionId,
                        principalTable: "WorkflowActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_Users_SubmissionUserId",
                        column: x => x.SubmissionUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Deals_WorkflowSets_WorkflowSetId",
                        column: x => x.WorkflowSetId,
                        principalTable: "WorkflowSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OriginalItemId = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: false),
                    DealId = table.Column<int>(nullable: false),
                    ProductId = table.Column<int>(nullable: true),
                    Position = table.Column<int>(nullable: true),
                    DayType = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTimeOffset>(nullable: true),
                    HalfHourTradingPeriodStart = table.Column<int>(nullable: true),
                    EndDate = table.Column<DateTimeOffset>(nullable: true),
                    HalfHourTradingPeriodEnd = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: true),
                    MinQuantity = table.Column<decimal>(nullable: true),
                    MaxQuantity = table.Column<decimal>(nullable: true),
                    Price = table.Column<decimal>(nullable: true),
                    Criteria = table.Column<string>(nullable: true),
                    SourceDataId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealItems_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealItems_DealItems_OriginalItemId",
                        column: x => x.OriginalItemId,
                        principalTable: "DealItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealItems_DealItemSourceData_SourceDataId",
                        column: x => x.SourceDataId,
                        principalTable: "DealItemSourceData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealNotes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealId = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(nullable: false),
                    CreationUserId = table.Column<int>(nullable: false),
                    Content = table.Column<string>(nullable: true),
                    IsLocked = table.Column<bool>(nullable: false),
                    ReminderType = table.Column<int>(nullable: true),
                    ReminderUserId = table.Column<int>(nullable: true),
                    ReminderEmailAccounts = table.Column<string>(nullable: true),
                    ReminderDateTime = table.Column<DateTimeOffset>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealNotes_Users_CreationUserId",
                        column: x => x.CreationUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealNotes_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealNotes_Users_ReminderUserId",
                        column: x => x.ReminderUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DealWorkflowStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealId = table.Column<int>(nullable: false),
                    WorkflowStatusId = table.Column<int>(nullable: false),
                    WorkflowStatusName = table.Column<string>(nullable: true),
                    AssigneeUserId = table.Column<int>(nullable: true),
                    AssigneeWorkflowRoleId = table.Column<int>(nullable: true),
                    InitiatedByUserId = table.Column<int>(nullable: true),
                    DateTimeCreated = table.Column<DateTimeOffset>(nullable: false),
                    DateTimeConfirmed = table.Column<DateTimeOffset>(nullable: true),
                    Finalized = table.Column<bool>(nullable: false),
                    PrecedingWorkflowActionId = table.Column<int>(nullable: true),
                    RevertedBackByUser = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealWorkflowStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealWorkflowStatuses_Users_AssigneeUserId",
                        column: x => x.AssigneeUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealWorkflowStatuses_WorkflowRoles_AssigneeWorkflowRoleId",
                        column: x => x.AssigneeWorkflowRoleId,
                        principalTable: "WorkflowRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealWorkflowStatuses_Deals_DealId",
                        column: x => x.DealId,
                        principalTable: "Deals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealWorkflowStatuses_Users_InitiatedByUserId",
                        column: x => x.InitiatedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealWorkflowStatuses_WorkflowActions_PrecedingWorkflowActionId",
                        column: x => x.PrecedingWorkflowActionId,
                        principalTable: "WorkflowActions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealWorkflowStatuses_WorkflowStatuses_WorkflowStatusId",
                        column: x => x.WorkflowStatusId,
                        principalTable: "WorkflowStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealWorkflowActionListeners",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealWorkflowStatusId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    UniqueActionGuid = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealWorkflowActionListeners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealWorkflowActionListeners_DealWorkflowStatuses_DealWorkflowStatusId",
                        column: x => x.DealWorkflowStatusId,
                        principalTable: "DealWorkflowStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealWorkflowActionListeners_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DealWorkflowTasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DealWorkflowStatusId = table.Column<int>(nullable: false),
                    WorkflowTaskId = table.Column<int>(nullable: false),
                    WorkflowTaskDescription = table.Column<string>(nullable: true),
                    WorkflowTaskAnswerId = table.Column<int>(nullable: true),
                    WorkflowTaskAnswerText = table.Column<string>(nullable: true),
                    TextInformation = table.Column<string>(nullable: true),
                    DateInformation = table.Column<DateTimeOffset>(nullable: true),
                    NumberInformation = table.Column<decimal>(nullable: true),
                    Done = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DealWorkflowTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DealWorkflowTasks_DealWorkflowStatuses_DealWorkflowStatusId",
                        column: x => x.DealWorkflowStatusId,
                        principalTable: "DealWorkflowStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DealWorkflowTasks_WorkflowTaskAnswers_WorkflowTaskAnswerId",
                        column: x => x.WorkflowTaskAnswerId,
                        principalTable: "WorkflowTaskAnswers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DealWorkflowTasks_WorkflowTasks_WorkflowTaskId",
                        column: x => x.WorkflowTaskId,
                        principalTable: "WorkflowTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AttachmentTypes",
                columns: new[] { "Id", "Active", "Description", "Name", "Other" },
                values: new object[,]
                {
                    { 1, true, null, "Pricing Summary", false },
                    { 16, true, null, "Other", true },
                    { 15, true, null, "Invoice", false },
                    { 14, true, null, "Contract Note", false },
                    { 13, true, null, "Signed Contract", false },
                    { 12, true, null, "Contracts Awarded Summary", false },
                    { 11, true, null, "Results CSV File", false },
                    { 10, true, null, "Validation Workbook", false },
                    { 9, true, null, "Bid File", false },
                    { 8, true, null, "Signed Confirmation", false },
                    { 7, true, null, "Counterparty Exposure", false },
                    { 6, true, null, "Solicitor's Certificate", false },
                    { 5, true, null, "Signed Agreement", false },
                    { 4, true, null, "Final Volumes", false },
                    { 3, true, null, "Interim Volumes", false },
                    { 2, true, null, "Validation Summary", false },
                    { 17, true, null, "ABC Deal Notification", false }
                });

            migrationBuilder.InsertData(
                table: "ConfigurationGroup",
                columns: new[] { "Id", "Description", "Identifier", "Name" },
                values: new object[,]
                {
                    { 3, "Configuration for reminder notifications", 3, "Reminders Settings" },
                    { 1, "Configuration settings that have system-wide effects", 1, "General Settings" },
                    { 2, "Data related to the integration of ABC Trades into WorkflowSystem", 2, "ABC Integration" }
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "NZ", "New Zealand" },
                    { 2, "AU", "Australia" }
                });

            migrationBuilder.InsertData(
                table: "DealCategories",
                columns: new[] { "Id", "Active", "Name", "UnitOfMeasure" },
                values: new object[,]
                {
                    { 1, true, "General", "MW" },
                    { 2, true, "Foreign Trade", null },
                    { 3, true, "Retail", "GJ" }
                });

            migrationBuilder.InsertData(
                table: "DealItemFieldsets",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Collection of fields for item input on deal types in general", "General" },
                    { 2, "Collection of fields for item input on future deals", "Futures" },
                    { 3, "Collection of fields for item input on retail deals", "Retail" },
                    { 4, "Collection of fields for item input on foreign trade deals", "Foreign Trade" },
                    { 5, "Collection of fields for item input on ABC trades", "ABC trades" },
                    { 6, "Collection of fields for branch to branch sales", "Branch to branch sales" }
                });

            migrationBuilder.InsertData(
                table: "Functionalities",
                columns: new[] { "Id", "FunctionalityEnum", "Name" },
                values: new object[,]
                {
                    { 8, 8, "Products" },
                    { 12, 12, "System Configuration" },
                    { 9, 9, "Deal Summary List" },
                    { 7, 7, "Deal Item Fieldsets" },
                    { 3, 3, "Users" },
                    { 5, 5, "Deal Categories" },
                    { 4, 4, "Counterparties" },
                    { 1, 1, "Deals" },
                    { 6, 6, "Deal Types" }
                });

            migrationBuilder.InsertData(
                table: "TraderAuthorityPolicies",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 1, true, null, "General" },
                    { 2, true, null, "General - branch to branch" },
                    { 3, true, null, "General - futures" },
                    { 4, true, null, "General - swaps" },
                    { 5, true, null, "General - import" },
                    { 6, true, null, "General - future obligations" },
                    { 7, true, null, "General - future options" },
                    { 8, true, null, "Retail" },
                    { 9, true, null, "Foreign Trade" },
                    { 10, true, null, "General - options sell" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 5, true, "Can only view deals", "Other" },
                    { 4, true, "Back Office", "BO" },
                    { 1, true, null, "Trader/EPM/GMSG/FO" },
                    { 2, true, "Middle Office", "MO" },
                    { 3, true, null, "Approver" }
                });

            migrationBuilder.InsertData(
                table: "WorkflowRoles",
                columns: new[] { "Id", "ApprovalLevel", "Name" },
                values: new object[,]
                {
                    { 1, 10, "Junior Trader" },
                    { 2, 20, "Trader" },
                    { 3, 30, "Senior Trader" },
                    { 4, 40, "General Manager" },
                    { 5, 50, "CEO" },
                    { 6, 60, "Board" },
                    { 7, null, "Compliance Office" },
                    { 8, null, "Accounting" }
                });

            migrationBuilder.InsertData(
                table: "WorkflowSets",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 1, true, "This workflow is used by the majority of the deal types", "Main Workflow" },
                    { 2, true, "This workflow is meant to be used by deals where the trade has been already executed before submission.", "Pre-executed Trades" }
                });

            migrationBuilder.InsertData(
                table: "ConfigurationEntry",
                columns: new[] { "Id", "ConfigurationGroupId", "Content", "ContentType", "FunctionalityForLookup", "Identifier", "Name" },
                values: new object[,]
                {
                    { 1, 1, "15", 2, null, 1, "Time-Out In Minutes" },
                    { 12, 3, "user@user.com", 1, null, 11, "E-mail accounts to be notified about deal expiry dates (separated by ;)" },
                    { 11, 3, "-7; 0", 1, null, 10, "days before deal expiry date (separated by ;)" },
                    { 9, 3, "-28; 0", 1, null, 9, "days before counterparty review date (separated by ;)" },
                    { 8, 2, "user@user.com", 1, null, 8, "E-mail accounts that will be notified on integration warnings/errors - use ; (semicolon) for multiple" },
                    { 7, 2, "True", 4, null, 7, "Reintegrate cancelled deals?" },
                    { 10, 3, "user@user.com", 1, null, 12, "E-mail accounts to be notified about counterparty review dates (separated by ;)" },
                    { 5, 2, "60", 2, 8, 5, "Product" },
                    { 4, 2, "94", 2, 4, 4, "Counterparty" },
                    { 3, 2, "101", 2, 6, 3, "Deal Type" },
                    { 2, 2, "3", 2, 5, 2, "Deal Category" },
                    { 6, 2, "160", 2, null, 6, "WorkflowActionId for 'Submit' workflow action" }
                });

            migrationBuilder.InsertData(
                table: "DealItemFields",
                columns: new[] { "Id", "DealItemFieldsetId", "DisplayOrder", "Execution", "Field", "Name" },
                values: new object[,]
                {
                    { 50, 4, 50, false, "Position", "Position" },
                    { 58, 4, 150, false, "Criteria", "Criteria" },
                    { 57, 4, 140, true, "Price", "Price" },
                    { 56, 4, 110, true, "Quantity", "Qty" },
                    { 53, 4, 80, true, "EndDate", "End Date" },
                    { 52, 4, 70, true, "StartDate", "Start Date" },
                    { 45, 3, 130, true, "MaxQuantity", "Max" },
                    { 39, 3, 90, true, "HalfHourTradingPeriodStart", "TP Start" },
                    { 43, 3, 150, false, "Criteria", "Criteria" },
                    { 42, 3, 140, true, "Price", "Price" },
                    { 41, 3, 110, true, "Quantity", "Qty" },
                    { 40, 3, 100, true, "HalfHourTradingPeriodEnd", "TP End" },
                    { 59, 5, 40, false, "ProductId", "Product" },
                    { 38, 3, 80, true, "EndDate", "End Date" },
                    { 44, 3, 120, true, "MinQuantity", "Min" },
                    { 60, 5, 50, false, "Position", "Position" },
                    { 66, 5, 140, false, "Price", "Price" },
                    { 62, 5, 80, false, "EndDate", "End Date" },
                    { 80, 6, 150, false, "Criteria", "Criteria" },
                    { 79, 6, 140, false, "Price", "Price" },
                    { 78, 6, 110, true, "Quantity", "Qty" },
                    { 77, 6, 100, false, "HalfHourTradingPeriodEnd", "TP End" },
                    { 76, 6, 90, false, "HalfHourTradingPeriodStart", "TP Start" },
                    { 75, 6, 80, false, "EndDate", "End Date" },
                    { 74, 6, 70, false, "StartDate", "Start Date" },
                    { 73, 6, 60, false, "DayType", "Day Type" },
                    { 72, 6, 50, false, "Position", "Position" },
                    { 71, 6, 10, true, "ProductId", "Product" },
                    { 67, 5, 150, false, "Criteria", "Criteria" },
                    { 37, 3, 70, true, "StartDate", "Start Date" },
                    { 65, 5, 110, false, "Quantity", "Qty" },
                    { 64, 5, 100, false, "HalfHourTradingPeriodEnd", "TP End" },
                    { 63, 5, 90, false, "HalfHourTradingPeriodStart", "TP Start" },
                    { 61, 5, 70, false, "StartDate", "Start Date" },
                    { 35, 3, 50, false, "Position", "Position" },
                    { 49, 4, 50, false, "ProductId", "Product" },
                    { 28, 2, 150, false, "Criteria", "Criteria" },
                    { 34, 3, 40, false, "ProductId", "Product" },
                    { 1, 1, 10, false, "ProductId", "Product" },
                    { 5, 1, 50, false, "Position", "Position" },
                    { 6, 1, 60, false, "DayType", "Day Type" },
                    { 7, 1, 70, true, "StartDate", "Start Date" },
                    { 8, 1, 80, true, "EndDate", "End Date" },
                    { 9, 1, 90, true, "HalfHourTradingPeriodStart", "TP Start" },
                    { 10, 1, 100, true, "HalfHourTradingPeriodEnd", "TP End" },
                    { 12, 1, 140, true, "Price", "Price" },
                    { 13, 1, 150, false, "Criteria", "Criteria" },
                    { 11, 1, 110, true, "Quantity", "Qty" },
                    { 17, 2, 20, false, "ProductId", "Product" },
                    { 20, 2, 50, false, "Position", "Position" },
                    { 21, 2, 60, false, "DayType", "Day Type" },
                    { 22, 2, 70, false, "StartDate", "Start Date" },
                    { 23, 2, 80, false, "EndDate", "End Date" },
                    { 24, 2, 90, false, "HalfHourTradingPeriodStart", "TP Start" },
                    { 25, 2, 100, false, "HalfHourTradingPeriodEnd", "TP End" },
                    { 26, 2, 110, true, "Quantity", "Qty" },
                    { 27, 2, 140, true, "Price", "Price" }
                });

            migrationBuilder.InsertData(
                table: "DealTypes",
                columns: new[] { "Id", "Active", "CanChangeProductOnExecution", "DealItemFieldsetId", "ForcePosition", "HasDelegatedAuthority", "HasExpiryDate", "HasLossFactors", "ItemExecutionImportTemplateType", "Name", "Position", "TraderAuthorityPolicyId", "UnitOfMeasure", "WorkflowSetId" },
                values: new object[,]
                {
                    { 20, true, false, 2, false, false, false, false, 2, "Future Obligation", 1, 6, "MW", 1 },
                    { 1, false, false, 1, false, false, false, false, null, "ISDA", 1, null, "MW", 1 },
                    { 19, true, false, 4, true, false, false, false, null, "Export - Sell", 0, 9, "TCO2e", 1 },
                    { 16, true, false, 4, true, false, false, false, null, "Import - Buy", 1, 9, "TCO2e", 1 },
                    { 23, true, false, 3, false, false, false, false, null, "General - Retail", 1, 8, "GJ", 1 },
                    { 101, true, false, 5, false, true, false, false, null, "ABC Gas Trade", 1, 8, null, 2 },
                    { 22, true, false, 2, false, false, false, false, 2, "Future Option", 1, 7, "MW", 1 },
                    { 3, false, false, 1, false, false, false, false, null, "Futures", 1, null, "MW", 1 },
                    { 29, true, false, 1, true, false, false, false, null, "Options - Sell", 0, 10, "MW", 1 },
                    { 24, true, false, 1, true, false, false, false, null, "Options - Buy", 1, 5, "MW", 1 },
                    { 5, true, false, 1, false, false, false, false, null, "Financial", 1, 4, "MW", 1 },
                    { 4, true, false, 1, false, false, false, false, null, "Financial Future - Buy", 1, 4, "pc", 1 },
                    { 2, true, false, 1, false, false, false, false, null, "General", 1, 1, "pc", 1 },
                    { 6, true, true, 6, false, false, true, true, 1, "Branch to Branch Sales", 0, 2, "pc", 1 },
                    { 30, true, false, 1, false, false, false, false, null, "Financial Futures - Sell", 0, 4, "MW", 1 }
                });

            migrationBuilder.InsertData(
                table: "FunctionalitiesInUserRoles",
                columns: new[] { "Id", "FunctionalityId", "UserRoleId" },
                values: new object[,]
                {
                    { 15, 1, 3 },
                    { 42, 12, 2 },
                    { 31, 9, 2 },
                    { 14, 8, 2 },
                    { 13, 7, 2 },
                    { 12, 6, 2 },
                    { 8, 1, 2 },
                    { 9, 3, 2 },
                    { 16, 3, 3 },
                    { 41, 12, 1 },
                    { 30, 9, 1 },
                    { 7, 8, 1 },
                    { 6, 7, 1 },
                    { 5, 6, 1 },
                    { 11, 5, 2 },
                    { 17, 4, 3 },
                    { 20, 7, 3 },
                    { 19, 6, 3 },
                    { 4, 5, 1 },
                    { 34, 9, 5 },
                    { 29, 1, 5 },
                    { 33, 9, 4 },
                    { 28, 8, 4 },
                    { 27, 7, 4 },
                    { 18, 5, 3 },
                    { 26, 6, 4 },
                    { 24, 4, 4 },
                    { 23, 3, 4 },
                    { 22, 1, 4 },
                    { 43, 12, 3 },
                    { 32, 9, 3 },
                    { 21, 8, 3 },
                    { 25, 5, 4 },
                    { 3, 4, 1 },
                    { 10, 4, 2 },
                    { 1, 1, 1 },
                    { 2, 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Active", "DealCategoryId", "Name" },
                values: new object[,]
                {
                    { 15, true, 1, "Cheesecake" },
                    { 14, true, 1, "Mudcake" },
                    { 13, true, 1, "Cake" },
                    { 12, true, 1, "Cupcake" },
                    { 11, true, 1, "Bread" },
                    { 10, true, 1, "Water" },
                    { 9, true, 1, "Beans" },
                    { 8, true, 1, "Rice" },
                    { 7, true, 1, "Pork" },
                    { 5, true, 1, "Lemon" },
                    { 4, true, 1, "Tomato" },
                    { 3, true, 1, "Banana" },
                    { 2, true, 1, "Lettuce" },
                    { 6, true, 1, "Chicken" },
                    { 1, true, 1, "Meat" }
                });

            migrationBuilder.InsertData(
                table: "SubFunctionalities",
                columns: new[] { "Id", "FunctionalityId", "Name", "ParentSubFunctionalityId", "SubFunctionalityEnum" },
                values: new object[,]
                {
                    { 18, 6, "View", null, 2 },
                    { 1, 1, "Create", null, 1 },
                    { 29, 6, "View Audit Logs", null, 8 },
                    { 19, 7, "Create", null, 1 },
                    { 20, 7, "Edit", null, 3 },
                    { 21, 7, "View", null, 2 },
                    { 2, 1, "Edit", null, 3 },
                    { 30, 7, "View Audit Logs", null, 8 },
                    { 23, 8, "Edit", null, 3 },
                    { 24, 8, "View", null, 2 },
                    { 31, 8, "View Audit Logs", null, 8 },
                    { 17, 6, "Edit", null, 3 },
                    { 40, 12, "View", null, 2 },
                    { 41, 12, "View Audit Logs", null, 8 },
                    { 22, 8, "Create", null, 1 },
                    { 3, 1, "View", null, 2 },
                    { 39, 12, "Edit", null, 3 },
                    { 25, 1, "View Audit Logs", null, 8 },
                    { 16, 6, "Create", null, 1 },
                    { 4, 1, "PDF Export", null, 7 },
                    { 15, 5, "View", null, 2 },
                    { 14, 5, "Edit", null, 3 },
                    { 13, 5, "Create", null, 1 },
                    { 27, 4, "View Audit Logs", null, 8 },
                    { 12, 4, "View", null, 2 },
                    { 28, 5, "View Audit Logs", null, 8 },
                    { 10, 4, "Create", null, 1 },
                    { 26, 3, "View Audit Logs", null, 8 },
                    { 9, 3, "View", null, 2 },
                    { 8, 3, "Edit", null, 3 },
                    { 32, 1, "Execute Deal", null, 9 },
                    { 7, 3, "Create", null, 1 },
                    { 11, 4, "Edit", null, 3 }
                });

            migrationBuilder.InsertData(
                table: "TraderAuthorityPoliciesCriteria",
                columns: new[] { "Id", "Active", "MaxAcquisitionCost", "MaxBuyVolume", "MaxDurationInMonths", "MaxSellAcquisitionCost", "MaxSellVolume", "MaxTermInMonths", "MaxVolume", "MaxVolumeForecastPercentage", "OnlyBuy", "OnlySell", "TraderAuthorityPolicyId", "WorkflowRoleId" },
                values: new object[,]
                {
                    { 5005, true, 5000000m, null, null, null, null, null, null, null, true, false, 5, 5 },
                    { 4011, true, null, null, 36, null, null, 6, 60m, null, false, false, 4, 5 },
                    { 4010, true, null, null, 48, null, null, 36, null, 0.20m, false, false, 4, 5 },
                    { 4009, true, null, null, 72, null, null, 60, 10m, null, false, false, 4, 5 },
                    { 2010, true, null, null, 48, null, null, 36, null, 0.20m, false, false, 2, 5 },
                    { 2011, true, null, null, 36, null, null, 6, 60m, null, false, false, 2, 5 },
                    { 2009, true, null, null, 72, null, null, 60, 10m, null, false, false, 2, 5 },
                    { 1006, true, null, null, 72, null, null, 60, 10m, null, false, false, 1, 5 },
                    { 1005, true, null, null, 60, null, null, 48, null, 0.20m, false, false, 1, 5 },
                    { 6005, true, null, null, 36, null, null, 1, 40m, null, false, false, 6, 5 },
                    { 10004, true, null, null, 36, null, null, 3, 60m, null, false, true, 10, 4 },
                    { 3005, true, null, 100m, 51, null, 100m, 51, null, null, false, false, 3, 5 },
                    { 7005, true, 5000000m, null, null, null, null, null, null, null, false, false, 7, 5 },
                    { 3007, true, null, null, null, null, null, null, null, null, false, false, 3, 6 },
                    { 9005, true, null, 100000m, null, null, 150000m, null, null, null, false, false, 9, 5 },
                    { 10005, true, null, null, 72, null, null, 60, 10m, null, false, true, 10, 5 },
                    { 10006, true, null, null, 48, null, null, 36, null, 0.20m, false, true, 10, 5 },
                    { 10007, true, null, null, 36, null, null, 6, 60m, null, false, true, 10, 5 },
                    { 1007, true, null, null, null, null, null, null, null, null, false, false, 1, 6 },
                    { 2012, true, null, null, null, null, null, null, null, null, false, false, 2, 6 },
                    { 4012, true, null, null, null, null, null, null, null, null, false, false, 4, 6 },
                    { 5007, true, null, null, null, null, null, null, null, null, true, false, 5, 6 },
                    { 6007, true, null, null, null, null, null, null, null, null, false, false, 6, 6 },
                    { 7007, true, null, null, null, null, null, null, null, null, false, false, 7, 6 },
                    { 8007, true, null, null, null, null, null, null, null, null, false, false, 8, 6 },
                    { 9007, true, null, null, null, null, null, null, null, null, false, false, 9, 6 },
                    { 10008, true, null, null, null, null, null, null, null, null, false, true, 10, 6 },
                    { 10003, true, null, null, 36, null, null, 24, null, 0.10m, false, false, 10, 4 },
                    { 8005, true, null, 10000m, 60, null, 10000m, 48, null, null, false, false, 8, 5 },
                    { 9004, true, null, 0m, null, null, 100000m, null, null, null, false, false, 9, 4 },
                    { 10001, true, null, null, 36, null, null, 24, 10m, null, false, true, 10, 3 },
                    { 7004, true, 4000000m, null, null, null, null, null, null, null, false, false, 7, 4 },
                    { 6002, true, null, null, 36, null, null, 1, 10m, null, false, false, 6, 2 },
                    { 5002, true, 1000000m, null, null, null, null, null, null, null, true, false, 5, 2 },
                    { 4004, true, null, null, 36, null, null, 3, 45m, null, false, false, 4, 2 },
                    { 4003, true, null, null, 36, null, null, 24, 10m, null, false, false, 4, 2 },
                    { 8004, true, null, 7000m, 36, null, 7000m, 24, null, null, false, false, 8, 4 },
                    { 2004, true, null, null, 36, null, null, 3, 45m, null, false, false, 2, 2 },
                    { 2003, true, null, null, 36, null, null, 24, 10m, null, false, false, 2, 2 },
                    { 1002, true, null, null, 36, null, null, 24, 10m, null, false, false, 1, 2 },
                    { 9001, true, null, 0m, null, null, 25000m, null, null, null, false, false, 9, 1 },
                    { 8001, true, null, 3000m, 12, null, 3000m, 6, null, null, false, false, 8, 1 },
                    { 7001, true, 1000000m, null, null, null, null, null, null, null, true, false, 7, 1 },
                    { 6001, true, null, null, 36, null, null, 1, 5m, null, false, false, 6, 1 },
                    { 5001, true, 500000m, null, null, null, null, null, null, null, true, false, 5, 1 },
                    { 4002, true, null, null, 36, null, null, 3, 12m, null, false, false, 4, 1 },
                    { 4001, true, null, null, 36, null, null, 24, 5m, null, false, false, 4, 1 },
                    { 3001, true, null, 12m, 51, null, 12m, 6, null, null, false, false, 3, 1 },
                    { 2002, true, null, null, 36, null, null, 3, 12m, null, false, false, 2, 1 },
                    { 2001, true, null, null, 36, null, null, 24, 5m, null, false, false, 2, 1 },
                    { 1001, true, null, null, 36, null, null, 24, 5m, null, false, false, 1, 1 },
                    { 7002, true, 2000000m, null, null, null, null, null, null, null, false, false, 7, 2 },
                    { 8002, true, null, 5000m, 12, null, 5000m, 6, null, null, false, false, 8, 2 },
                    { 3002, true, null, 25m, 51, null, 25m, 12, null, null, false, false, 3, 2 },
                    { 1003, true, null, null, 48, null, null, 36, 15m, null, false, false, 1, 3 },
                    { 6004, true, null, null, 36, null, null, 1, 20m, null, false, false, 6, 4 },
                    { 5004, true, 2000000m, null, null, null, null, null, null, null, true, false, 5, 4 },
                    { 9002, true, null, 0m, null, null, 50000m, null, null, null, false, false, 9, 2 },
                    { 4007, true, null, null, 36, null, null, 24, null, 0.10m, false, false, 4, 4 },
                    { 3004, true, null, 50m, 51, null, 50m, 24, null, null, false, false, 3, 4 },
                    { 2008, true, null, null, 36, null, null, 3, 60m, null, false, false, 2, 4 },
                    { 2007, true, null, null, 36, null, null, 24, null, 0.10m, false, false, 2, 4 },
                    { 1004, true, null, null, 48, null, null, 36, 20m, null, false, false, 1, 4 },
                    { 10002, true, null, null, 36, null, null, 3, 50m, null, false, true, 10, 3 },
                    { 4008, true, null, null, 36, null, null, 3, 60m, null, false, false, 4, 4 },
                    { 8003, true, null, 6000m, 24, null, 6000m, 12, null, null, false, false, 8, 3 },
                    { 7003, true, 3000000m, null, null, null, null, null, null, null, false, false, 7, 3 },
                    { 6003, true, null, null, 36, null, null, 1, 15m, null, false, false, 6, 3 },
                    { 5003, true, 1500000m, null, null, null, null, null, null, null, true, false, 5, 3 },
                    { 4006, true, null, null, 36, null, null, 3, 50m, null, false, false, 4, 3 },
                    { 4005, true, null, null, 36, null, null, 24, 10m, null, false, false, 4, 3 },
                    { 3003, true, null, 40m, 51, null, 40m, 18, null, null, false, false, 3, 3 },
                    { 2006, true, null, null, 36, null, null, 3, 50m, null, false, false, 2, 3 },
                    { 2005, true, null, null, 36, null, null, 24, 10m, null, false, false, 2, 3 },
                    { 9003, true, null, 0m, null, null, 75000m, null, null, null, false, false, 9, 3 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Active", "Name", "UserRoleId", "Username" },
                values: new object[] { 1, true, "Generic User", 2, "user@user.com" });

            migrationBuilder.InsertData(
                table: "WorkflowStatuses",
                columns: new[] { "Id", "AllowsDealEditing", "AllowsDealExecution", "AllowsEditDelegatedAuthority", "AssignmentType", "CancelDeal", "FinalizeDeal", "Name", "Order", "WorkflowRoleId", "WorkflowSetId" },
                values: new object[,]
                {
                    { 130, false, false, false, 3, false, false, "Checked by Compliance Office", 30, 8, 2 },
                    { 120, false, false, true, 3, false, false, "Submitted", 20, 7, 2 },
                    { 110, true, false, true, 1, false, false, "Entered", 10, null, 2 },
                    { 140, false, false, false, 3, false, true, "Completed", 40, 7, 2 },
                    { 8, false, false, false, 1, true, true, "Cancelled", 80, null, 1 },
                    { 2, false, false, false, 3, false, false, "Submitted", 20, 7, 1 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowStatuses",
                columns: new[] { "Id", "AllowsDealEditing", "AllowsDealExecution", "AllowsEditDelegatedAuthority", "AssignmentType", "CancelDeal", "FinalizeDeal", "Name", "Order", "WorkflowRoleId", "WorkflowSetId" },
                values: new object[,]
                {
                    { 6, false, false, false, 3, false, false, "Checked by Compliance Office", 60, 8, 1 },
                    { 5, false, false, false, 3, false, false, "Executed (Deprecated)", 50, 7, 1 },
                    { 4, false, true, false, 3, false, false, "Approved", 40, 7, 1 },
                    { 3, false, false, false, 4, false, false, "Validated", 30, null, 1 },
                    { 1, true, false, false, 1, false, false, "Entered", 10, null, 1 },
                    { 7, false, false, false, 3, false, true, "Completed", 70, 7, 1 },
                    { 150, false, false, false, 1, true, true, "Cancelled", 50, null, 2 }
                });

            migrationBuilder.InsertData(
                table: "DealTypesInDealCategories",
                columns: new[] { "DealCategoryId", "DealTypeId" },
                values: new object[,]
                {
                    { 1, 5 },
                    { 3, 101 },
                    { 2, 19 },
                    { 2, 16 },
                    { 3, 23 },
                    { 1, 20 },
                    { 1, 22 },
                    { 1, 30 },
                    { 1, 29 },
                    { 1, 24 },
                    { 1, 4 },
                    { 1, 2 },
                    { 1, 6 }
                });

            migrationBuilder.InsertData(
                table: "SubFunctionalitiesInUserRoles",
                columns: new[] { "Id", "FunctionalityInUserRoleId", "SubFunctionalityId" },
                values: new object[,]
                {
                    { 76, 19, 29 },
                    { 43, 20, 21 },
                    { 79, 20, 30 },
                    { 1, 1, 3 },
                    { 82, 21, 31 },
                    { 42, 19, 18 },
                    { 44, 21, 24 },
                    { 73, 18, 28 },
                    { 37, 15, 2 },
                    { 70, 17, 27 },
                    { 67, 16, 26 },
                    { 39, 16, 9 },
                    { 63, 15, 25 },
                    { 38, 15, 4 },
                    { 105, 43, 40 },
                    { 36, 15, 3 },
                    { 41, 18, 15 },
                    { 106, 43, 41 },
                    { 49, 24, 12 },
                    { 46, 22, 2 },
                    { 65, 29, 25 },
                    { 54, 29, 3 },
                    { 83, 28, 31 },
                    { 53, 28, 24 },
                    { 80, 27, 30 },
                    { 52, 27, 21 },
                    { 77, 26, 29 },
                    { 45, 22, 3 },
                    { 51, 26, 18 },
                    { 50, 25, 15 },
                    { 71, 24, 27 },
                    { 104, 42, 41 },
                    { 68, 23, 26 },
                    { 48, 23, 9 },
                    { 64, 22, 25 },
                    { 47, 22, 4 },
                    { 74, 25, 28 },
                    { 103, 42, 40 },
                    { 40, 17, 12 },
                    { 81, 14, 31 },
                    { 62, 8, 25 },
                    { 15, 8, 4 },
                    { 14, 8, 2 },
                    { 13, 8, 1 },
                    { 12, 8, 3 },
                    { 101, 41, 41 },
                    { 100, 41, 40 },
                    { 102, 42, 39 },
                    { 11, 7, 24 },
                    { 59, 6, 30 },
                    { 10, 6, 21 },
                    { 85, 8, 32 },
                    { 58, 5, 29 },
                    { 57, 4, 28 },
                    { 8, 4, 15 },
                    { 56, 3, 27 },
                    { 7, 3, 12 },
                    { 55, 2, 26 },
                    { 6, 2, 9 },
                    { 84, 1, 32 },
                    { 61, 1, 25 },
                    { 4, 1, 4 },
                    { 3, 1, 2 },
                    { 2, 1, 1 },
                    { 9, 5, 18 },
                    { 18, 9, 9 },
                    { 60, 7, 31 },
                    { 20, 9, 7 },
                    { 34, 14, 23 },
                    { 33, 14, 24 },
                    { 78, 13, 30 },
                    { 32, 13, 19 },
                    { 31, 13, 20 },
                    { 30, 13, 21 },
                    { 75, 12, 29 },
                    { 29, 12, 16 },
                    { 35, 14, 22 },
                    { 28, 12, 17 },
                    { 72, 11, 28 },
                    { 26, 11, 13 },
                    { 25, 11, 14 },
                    { 24, 11, 15 },
                    { 69, 10, 27 },
                    { 23, 10, 10 },
                    { 22, 10, 11 },
                    { 21, 10, 12 },
                    { 66, 9, 26 },
                    { 27, 12, 18 },
                    { 19, 9, 8 }
                });

            migrationBuilder.InsertData(
                table: "UsersInWorkflowRoles",
                columns: new[] { "UserId", "WorkflowRoleId", "Active", "Id" },
                values: new object[,]
                {
                    { 1, 7, true, 1 },
                    { 1, 8, true, 3 },
                    { 1, 2, true, 2 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowActions",
                columns: new[] { "Id", "Active", "CantBePerformedBySameUser", "Description", "DirectActionOnEmailNotification", "IsSubmission", "Name", "PerformsExecutionAutomatically", "SourceWorkflowStatusId", "TargetAlternateDescriptionSuffix", "TargetWorkflowStatusId" },
                values: new object[,]
                {
                    { 6, false, false, "Executes the deal, sending it to Compliance Office checking.", false, false, "Execute (deprecated)", false, null, null, 1 },
                    { 190, true, false, "Performs the Accounting checks, completing/finalising the deal.", false, false, "Check - Back Office", false, 130, null, 140 },
                    { 200, true, false, "Sends deal back to Compliance Office.", false, false, "Revert", false, 130, "Reversed By Accounting Checks", 120 },
                    { 170, true, false, "Performs the Middle Office checks, and sends the deal for Back Office checking.", false, false, "Check - Middle Office", false, 120, null, 130 },
                    { 180, true, false, "Sends deal back to the trader.", false, false, "Revert", false, 120, "Reversed By Compliance Office Checks", 110 },
                    { 160, true, false, "Submits the deal for Middle Office validation.", false, true, "Submit", true, 110, null, 120 },
                    { 210, true, false, "Reopens a completed deal.", false, false, "Reopen", false, 140, "Reopened", 130 },
                    { 13, true, false, "Cancels a deal permanently.", false, false, "Cancel", false, null, null, 8 },
                    { 12, true, false, "Reopens a completed deal.", false, false, "Reopen", false, 7, "Reopened", 6 },
                    { 10, true, false, "Performs the Accounting checks, completing/finalising the deal.", false, false, "Check - Accounting", false, 6, null, 7 },
                    { 8, true, false, "Performs the Compliance Office checks, and sends the deal for Back Office checking.", false, false, "Check - Compliance Office", false, 4, null, 6 },
                    { 9, true, false, "Sends deal back to the trader in an entered state.", false, false, "Revert", false, 4, "Reversed By Compliance Office Checks", 1 },
                    { 4, true, true, "Approves the deal so it can be executed.", true, false, "Approve", false, 3, null, 4 },
                    { 5, true, false, "Sends deal back to Compliance Office.", true, false, "Revert", false, 3, "Reverted - Approval", 2 },
                    { 2, true, true, "Validates the deal in regards to compliance concerns, so the deal can proceed for approval.", false, false, "Validate", false, 2, null, 3 },
                    { 3, true, false, "Sends deal back to the trader.", false, false, "Revert", false, 2, "Reverted - Validation", 1 },
                    { 1, true, false, "Submits the deal for Compliance validation.", false, true, "Submit", false, 1, null, 2 },
                    { 7, false, false, "Cancels the execution, sending it back to Compliance Office.", false, false, "Cancel Execution (deprecated)", false, null, null, 1 },
                    { 11, true, false, "Sends deal back to Compliance Office.", false, false, "Revert", false, 6, "Reversed By Accounting Checks", 4 },
                    { 220, true, false, "Cancels a deal permanently.", false, false, "Cancel", false, null, null, 150 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTasks",
                columns: new[] { "Id", "Active", "DependingUponAnswerId", "Description", "Mandatory", "Order", "PrecedingAnswerId", "Type", "WorkflowActionId" },
                values: new object[,]
                {
                    { 400, true, null, "Does the deal have at least one dealItem?", true, 400, null, 12, 1 },
                    { 151, true, null, "Has the contract been signed in terms of the Trading Guidelines?", true, 151, null, 2, 8 },
                    { 149, true, null, "Do terms of signed Contract match WorkflowSystem?", true, 149, null, 2, 8 },
                    { 144, true, null, "Contracts Awarded Summary attached?", true, 144, null, 2, 8 },
                    { 142, true, null, "FTR CSV Files attached?", true, 142, null, 2, 8 },
                    { 140, true, null, "Validation & Results Check Workbook attached?", true, 140, null, 2, 8 },
                    { 138, true, null, "Bid File attached?", true, 138, null, 2, 8 },
                    { 135, true, null, "Have the contracts awarded vs bid/approved checks been completed?", true, 135, null, 2, 8 },
                    { 133, true, null, "Has the Hedge Contracts Database been updated?", true, 133, null, 2, 8 },
                    { 131, true, null, "CSV files downloaded from ABC website", true, 131, null, 2, 8 },
                    { 129, true, null, "Signed Confirmation attached?", true, 129, null, 2, 8 },
                    { 127, true, null, "Counterparty Exposure attached?", true, 127, null, 2, 8 },
                    { 125, true, null, "Is counterparty included in the Counterparty Exposure report?", true, 125, null, 2, 8 },
                    { 114, true, null, "Have AML and FMCA requirements been satisfied?", true, 114, null, 2, 8 },
                    { 112, true, null, "Has the Confirmation been signed in terms of the parties' respective Trading Guidelines?", true, 112, null, 2, 8 },
                    { 107, true, null, "Do terms of signed Confirmation match WorkflowSystem?", true, 107, null, 2, 8 },
                    { 105, true, null, "Solicitor's Certificate attached?", true, 105, null, 2, 8 },
                    { 103, true, null, "Signed PPA attached?", true, 103, null, 2, 8 },
                    { 156, true, null, "Signed Contract Attached?", true, 156, null, 2, 8 },
                    { 100, true, null, "Is a new settlement process required?", true, 100, null, 2, 8 },
                    { 158, true, null, "Has transfer via NZEUR been completed?", true, 158, null, 2, 8 },
                    { 162, true, null, "Has settlement occurred?", true, 162, null, 2, 8 },
                    { 1110, true, null, "Does this deal pass all Accounting checks?", true, 1110, null, 1, 190 },
                    { 1120, true, null, "Provide reason for reversion", true, 1120, null, 3, 200 },
                    { 1090, true, null, "Produce Daily report", true, 1090, null, 1, 170 },
                    { 1080, true, null, "Produce Summary report", true, 1080, null, 1, 170 },
                    { 1060, true, null, "ABC deal notification attached?", true, 1060, null, 2, 170 },
                    { 1030, true, null, "Has Hedge Contracts Database been updated?", true, 1030, null, 2, 170 },
                    { 1010, true, null, "Are all trades within respective Trader Authority levels?", true, 1010, null, 15, 170 },
                    { 1100, true, null, "Provide reason for reversion", true, 1100, null, 3, 180 },
                    { 500, true, null, "Ensure deal is not executed or had its execution cancelled", true, 500, null, 11, 13 },
                    { 175, true, null, "Provide reason for cancelling deal", true, 175, null, 3, 13 },
                    { 174, true, null, "Provide reason for reopening deal", true, 174, null, 3, 12 },
                    { 172, false, null, "Check B.O. # 3", true, 172, null, 1, 10 },
                    { 171, false, null, "Check B.O. # 2", true, 171, null, 1, 10 },
                    { 170, true, null, "Does this deal pass all Accounting checks?", true, 170, null, 1, 10 },
                    { 173, true, null, "Provide reason for reversion", true, 173, null, 3, 11 },
                    { 167, true, null, "Invoice Attached?", true, 167, null, 2, 8 },
                    { 165, true, null, "Contract Note Attached?", true, 165, null, 2, 8 },
                    { 160, true, null, "Has the Carbon Stocks Register been updated?", true, 160, null, 2, 8 },
                    { 1130, true, null, "Provide reason for reopening deal", true, 1130, null, 3, 210 },
                    { 95, true, null, "Is EA Disclosure/Verification required?", true, 95, null, 2, 8 },
                    { 87, true, null, "Has the PPA been signed in terms of the Trading Guidelines?", true, 87, null, 2, 8 },
                    { 38, false, null, "Will deal be IFRS accounted?", true, 38, null, 2, 2 },
                    { 37, true, null, "Will legal overview of documents be required?", true, 37, null, 2, 2 },
                    { 33, true, null, "Will deal be under ISDA or Long Form Confirmation (LFC)?", true, 33, null, 2, 2 },
                    { 29, true, null, "Is proposed peak exposure within approved limit?", true, 29, null, 2, 2 },
                    { 27, true, null, "Will legal overview of PPA be required?", true, 27, null, 2, 2 },
                    { 25, true, null, "Is the counterparty verified?", true, 25, null, 2, 2 },
                    { 22, true, null, "Is price path check required?", true, 22, null, 2, 2 },
                    { 18, true, null, "Is Stress Limits testing required?", true, 18, null, 2, 2 },
                    { 14, true, null, "Is Market test required?", true, 14, null, 2, 2 },
                    { 10, true, null, "Are Long Term Limits checks required?", true, 10, null, 2, 2 },
                    { 6, true, null, "Is CFaR/Effective Length test required?", true, 6, null, 2, 2 },
                    { 4, true, null, "Does the deal meet the current trading stategy?", true, 4, null, 2, 2 },
                    { 51, true, null, "Provide reason for reversion", true, 51, null, 3, 3 },
                    { 2, true, null, "Is IFRS accounted?", true, 2, null, 2, 1 },
                    { 1, true, null, "Was the Deal Expiry Date recorded?", true, 1, null, 9, 1 },
                    { 402, true, null, "Was a document attached to this deal?", false, 402, null, 14, 1 },
                    { 401, true, null, "Was a note created on this deal?", false, 401, null, 13, 1 },
                    { 39, true, null, "Will deal be Market settled?", true, 39, null, 2, 2 },
                    { 89, true, null, "Has PPA been filed in LEX?", true, 89, null, 2, 8 },
                    { 40, true, null, "Have Acquisition Cost checks been completed & match FO Bid File?", true, 40, null, 2, 2 },
                    { 46, true, null, "Are we buying or selling?", true, 46, null, 2, 2 },
                    { 85, true, null, "Do terms of signed PPA match WorkflowSystem?", true, 85, null, 2, 8 },
                    { 83, true, null, "Final Volumes attached?", true, 83, null, 2, 8 },
                    { 81, true, null, "Interim Volumes attached?", true, 81, null, 2, 8 },
                    { 79, true, null, "Validation Summary attached?", true, 79, null, 2, 8 },
                    { 77, true, null, "Pricing Summary attached?", true, 77, null, 2, 8 },
                    { 74, true, null, "Has Hedge Contracts Database been updated?", true, 74, null, 2, 8 },
                    { 71, true, null, "Is EA Disclosure required for any deals?", true, 71, null, 2, 8 },
                    { 68, true, null, "Are Final Transfer Volumes within approved limits?", true, 68, null, 2, 8 },
                    { 66, true, null, "Final Transfer Volumes received from Commercial Sales Manager?", true, 66, null, 2, 8 },
                    { 63, true, null, "Interim Transfer Volumes received from Commercial Sales Manager?", true, 63, null, 2, 8 },
                    { 60, true, null, "Final/Expiry Notice sent?", true, 60, null, 2, 8 },
                    { 57, true, null, "7 Day Notice sent?", true, 57, null, 2, 8 },
                    { 54, true, null, "Deal Approval Notice sent?", true, 54, null, 2, 8 },
                    { 302, true, null, "Ensure deal is executed", true, 302, null, 10, 8 },
                    { 301, true, null, "Ensure deal is not executed or had its execution cancelled", true, 301, null, 11, 9 },
                    { 169, true, null, "Provide reason for reversion", true, 169, null, 3, 9 },
                    { 52, true, null, "Provide reason for reversion", true, 52, null, 3, 5 },
                    { 300, true, null, "Is this a sell deal?", true, 300, null, 2, 2 },
                    { 1140, true, null, "Provide reason for cancelling deal", true, 1140, null, 3, 220 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTaskAnswers",
                columns: new[] { "Id", "Active", "AlternateWorkflowActionId", "AnswerType", "AttachmentTypeToVerifyId", "Description", "WorkflowTaskId" },
                values: new object[,]
                {
                    { 790, true, null, 1, 2, "Yes", 79 },
                    { 540, true, null, 1, null, "Yes", 54 },
                    { 1140, true, null, 1, null, "Yes", 114 },
                    { 1141, true, null, 2, null, "No", 114 },
                    { 461, true, null, 3, null, "Selling", 46 },
                    { 460, true, null, 3, null, "Buying", 46 },
                    { 3001, true, null, 2, null, "No", 300 },
                    { 3000, true, null, 1, null, "Yes", 300 },
                    { 401, true, null, 2, null, "No", 40 },
                    { 400, true, null, 1, null, "Yes", 40 },
                    { 1250, true, null, 1, null, "Yes", 125 },
                    { 1251, true, null, 2, null, "No", 125 },
                    { 391, true, null, 2, null, "No", 39 },
                    { 390, true, null, 1, null, "Yes", 39 },
                    { 1270, true, null, 1, 7, "Yes", 127 },
                    { 381, true, null, 2, null, "No", 38 },
                    { 380, true, null, 1, null, "Yes", 38 },
                    { 1271, true, null, 2, null, "No", 127 },
                    { 1380, true, null, 1, 9, "Yes", 138 },
                    { 1350, true, null, 1, null, "Yes", 135 },
                    { 270, true, null, 1, null, "Yes", 27 },
                    { 271, true, null, 2, null, "No", 27 },
                    { 1330, true, null, 1, null, "Yes", 133 },
                    { 290, true, null, 1, null, "Yes", 29 },
                    { 541, true, null, 2, null, "No", 54 },
                    { 291, true, null, 2, null, "No", 29 },
                    { 330, true, null, 3, null, "ISDA", 33 },
                    { 331, true, null, 3, null, "LFC", 33 },
                    { 1291, true, null, 2, null, "No", 129 },
                    { 1290, true, null, 1, 8, "Yes", 129 },
                    { 370, true, null, 1, null, "Yes", 37 },
                    { 1001, true, null, 2, null, "No", 100 },
                    { 1310, true, null, 1, null, "Yes", 131 },
                    { 1381, true, null, 2, null, "No", 138 },
                    { 570, true, null, 1, null, "Yes", 57 },
                    { 600, true, null, 1, null, "Yes", 60 },
                    { 951, true, null, 2, null, "No", 95 },
                    { 950, true, null, 1, null, "Yes", 95 },
                    { 891, true, 9, 2, null, "No", 89 },
                    { 890, true, null, 1, null, "Yes", 89 },
                    { 871, true, 9, 2, null, "No", 87 },
                    { 870, true, null, 1, null, "Yes", 87 },
                    { 851, true, 9, 2, null, "No", 85 },
                    { 850, true, null, 1, null, "Yes", 85 },
                    { 1030, true, null, 1, 5, "Yes", 103 },
                    { 830, true, null, 1, 4, "Yes", 83 },
                    { 1031, true, null, 2, null, "No", 103 },
                    { 811, true, null, 2, null, "No", 81 },
                    { 810, true, null, 1, 3, "Yes", 81 },
                    { 1050, true, null, 1, 6, "Yes", 105 },
                    { 1051, true, null, 2, null, "No", 105 },
                    { 791, true, null, 2, null, "No", 79 },
                    { 771, true, null, 2, null, "No", 77 },
                    { 601, true, null, 2, null, "No", 60 },
                    { 630, true, null, 1, null, "Yes", 63 },
                    { 631, true, null, 3, null, "N/A", 63 },
                    { 660, true, null, 1, null, "Yes", 66 },
                    { 680, true, null, 1, null, "Yes", 68 },
                    { 681, true, null, 2, null, "No", 68 },
                    { 571, true, null, 2, null, "No", 57 },
                    { 1121, true, 9, 2, null, "No", 112 },
                    { 711, true, null, 2, null, "No", 71 },
                    { 1120, true, null, 1, null, "Yes", 112 },
                    { 740, true, null, 1, null, "Yes", 74 },
                    { 1071, true, 9, 2, null, "No", 107 },
                    { 1070, true, null, 1, null, "Yes", 107 },
                    { 770, true, null, 1, 1, "Yes", 77 },
                    { 710, true, null, 1, null, "Yes", 71 },
                    { 251, true, null, 2, null, "No", 25 },
                    { 371, true, null, 2, null, "No", 37 },
                    { 1000, true, null, 1, null, "Yes", 100 },
                    { 10100, true, null, 1, null, "Yes", 1010 },
                    { 1671, true, null, 2, null, "No", 167 },
                    { 1490, true, null, 1, null, "Yes", 149 },
                    { 100, true, null, 1, null, "Yes", 10 },
                    { 181, true, null, 2, null, "No", 18 },
                    { 180, true, null, 1, null, "Yes", 18 },
                    { 1670, true, null, 1, 15, "Yes", 167 },
                    { 1510, true, null, 1, null, "Yes", 151 },
                    { 60, true, null, 1, null, "Yes", 6 },
                    { 61, true, null, 2, null, "No", 6 },
                    { 1511, true, 9, 2, null, "No", 151 },
                    { 1651, true, null, 2, null, "No", 165 },
                    { 141, true, null, 2, null, "No", 14 },
                    { 140, true, null, 1, null, "Yes", 14 },
                    { 1560, true, null, 1, 13, "Yes", 156 },
                    { 1561, true, null, 2, null, "No", 156 },
                    { 1650, true, null, 1, 14, "Yes", 165 },
                    { 250, true, null, 1, null, "Yes", 25 },
                    { 1580, true, null, 1, null, "Yes", 158 },
                    { 1620, true, null, 1, null, "Yes", 162 },
                    { 101, true, null, 2, null, "No", 10 },
                    { 10101, true, null, 2, null, "No", 1010 },
                    { 1441, true, null, 2, null, "No", 144 },
                    { 1491, true, 9, 2, null, "No", 149 },
                    { 221, true, null, 2, null, "No", 22 },
                    { 1400, true, null, 1, 10, "Yes", 140 },
                    { 1401, true, null, 2, null, "No", 140 },
                    { 20, true, null, 1, null, "Yes", 2 },
                    { 21, true, null, 2, null, "No", 2 },
                    { 1420, true, null, 1, 11, "Yes", 142 },
                    { 1421, true, null, 2, null, "No", 142 },
                    { 1440, true, null, 1, 12, "Yes", 144 },
                    { 10601, true, null, 2, null, "No", 1060 },
                    { 220, true, null, 1, null, "Yes", 22 },
                    { 10600, true, null, 1, 17, "Yes", 1060 },
                    { 1600, true, null, 1, null, "Yes", 160 },
                    { 10300, true, null, 1, null, "Yes", 1030 },
                    { 41, true, null, 2, null, "No", 4 },
                    { 40, true, null, 1, null, "Yes", 4 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTaskInDealTypes",
                columns: new[] { "WorkflowTaskId", "DealTypeId" },
                values: new object[,]
                {
                    { 100, 30 },
                    { 103, 2 },
                    { 167, 19 },
                    { 100, 24 },
                    { 100, 5 },
                    { 162, 19 },
                    { 162, 16 },
                    { 100, 4 },
                    { 160, 19 },
                    { 160, 16 },
                    { 112, 4 },
                    { 100, 2 },
                    { 100, 29 },
                    { 167, 16 },
                    { 107, 30 },
                    { 107, 29 },
                    { 105, 5 },
                    { 105, 24 },
                    { 100, 23 },
                    { 105, 29 },
                    { 105, 30 },
                    { 165, 19 },
                    { 1030, 101 },
                    { 165, 16 },
                    { 105, 4 },
                    { 1060, 101 },
                    { 105, 2 },
                    { 107, 4 },
                    { 107, 5 },
                    { 107, 24 },
                    { 105, 23 },
                    { 1010, 101 },
                    { 156, 16 },
                    { 112, 24 },
                    { 127, 30 },
                    { 127, 16 },
                    { 127, 19 },
                    { 144, 20 },
                    { 144, 22 },
                    { 129, 4 },
                    { 129, 5 },
                    { 129, 24 },
                    { 142, 20 },
                    { 142, 22 },
                    { 129, 29 },
                    { 129, 30 },
                    { 131, 22 },
                    { 131, 20 },
                    { 140, 20 },
                    { 140, 22 },
                    { 133, 22 },
                    { 133, 20 },
                    { 135, 22 },
                    { 135, 20 },
                    { 138, 20 },
                    { 127, 29 },
                    { 127, 24 },
                    { 127, 5 },
                    { 127, 4 },
                    { 158, 19 },
                    { 158, 16 },
                    { 112, 29 },
                    { 156, 19 },
                    { 112, 30 },
                    { 156, 23 },
                    { 114, 4 },
                    { 151, 19 },
                    { 151, 16 },
                    { 114, 5 },
                    { 112, 5 },
                    { 114, 24 },
                    { 114, 29 },
                    { 114, 30 },
                    { 149, 19 },
                    { 125, 4 },
                    { 149, 16 },
                    { 149, 23 },
                    { 125, 5 },
                    { 125, 24 },
                    { 125, 29 },
                    { 125, 30 },
                    { 151, 23 },
                    { 138, 22 },
                    { 1, 6 },
                    { 95, 29 },
                    { 14, 5 },
                    { 14, 24 },
                    { 14, 29 },
                    { 14, 30 },
                    { 14, 20 },
                    { 18, 6 },
                    { 18, 2 },
                    { 18, 4 },
                    { 18, 5 },
                    { 18, 24 },
                    { 18, 29 },
                    { 18, 30 },
                    { 18, 20 },
                    { 22, 6 },
                    { 22, 2 },
                    { 22, 4 },
                    { 22, 5 },
                    { 22, 24 },
                    { 22, 29 },
                    { 22, 30 },
                    { 25, 2 },
                    { 25, 4 },
                    { 25, 5 },
                    { 25, 24 },
                    { 25, 29 },
                    { 25, 30 },
                    { 25, 16 },
                    { 25, 19 },
                    { 27, 2 },
                    { 14, 4 },
                    { 29, 4 },
                    { 14, 2 },
                    { 10, 20 },
                    { 2, 4 },
                    { 2, 5 },
                    { 2, 24 },
                    { 2, 29 },
                    { 2, 30 },
                    { 4, 6 },
                    { 4, 2 },
                    { 4, 4 },
                    { 4, 5 },
                    { 4, 24 },
                    { 4, 29 },
                    { 4, 30 },
                    { 4, 22 },
                    { 4, 20 },
                    { 6, 6 },
                    { 6, 2 },
                    { 6, 4 },
                    { 6, 5 },
                    { 6, 24 },
                    { 6, 29 },
                    { 6, 30 },
                    { 6, 20 },
                    { 10, 6 },
                    { 10, 2 },
                    { 10, 4 },
                    { 10, 5 },
                    { 10, 24 },
                    { 10, 29 },
                    { 10, 30 },
                    { 14, 6 },
                    { 95, 30 },
                    { 29, 5 },
                    { 29, 29 },
                    { 66, 6 },
                    { 68, 6 },
                    { 71, 6 },
                    { 74, 6 },
                    { 74, 2 },
                    { 74, 4 },
                    { 74, 5 },
                    { 74, 24 },
                    { 74, 29 },
                    { 74, 30 },
                    { 74, 23 },
                    { 77, 6 },
                    { 1080, 101 },
                    { 79, 6 },
                    { 79, 2 },
                    { 79, 4 },
                    { 79, 5 },
                    { 79, 24 },
                    { 79, 29 },
                    { 79, 30 },
                    { 81, 6 },
                    { 83, 6 },
                    { 85, 2 },
                    { 87, 2 },
                    { 89, 2 },
                    { 95, 2 },
                    { 95, 4 },
                    { 95, 5 },
                    { 95, 24 },
                    { 63, 6 },
                    { 29, 24 },
                    { 60, 6 },
                    { 54, 6 },
                    { 29, 30 },
                    { 33, 4 },
                    { 33, 5 },
                    { 33, 24 },
                    { 33, 29 },
                    { 33, 30 },
                    { 37, 4 },
                    { 37, 5 },
                    { 37, 24 },
                    { 37, 29 },
                    { 37, 30 },
                    { 37, 23 },
                    { 37, 16 },
                    { 37, 19 },
                    { 38, 4 },
                    { 38, 5 },
                    { 38, 24 },
                    { 38, 29 },
                    { 38, 30 },
                    { 39, 4 },
                    { 39, 5 },
                    { 39, 24 },
                    { 39, 29 },
                    { 39, 30 },
                    { 40, 22 },
                    { 40, 20 },
                    { 300, 23 },
                    { 46, 16 },
                    { 46, 19 },
                    { 57, 6 },
                    { 1090, 101 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTasks",
                columns: new[] { "Id", "Active", "DependingUponAnswerId", "Description", "Mandatory", "Order", "PrecedingAnswerId", "Type", "WorkflowActionId" },
                values: new object[,]
                {
                    { 122, true, 20, "Has IFRS documentation been completed by Front Office & deal included in Hedge Valuation report?", true, 122, null, 2, 8 },
                    { 128, true, null, "Please explain", true, 128, 1271, 3, 8 },
                    { 126, true, null, "Why not?", true, 126, 1251, 3, 8 },
                    { 116, true, null, "Please explain", true, 116, 1141, 3, 8 },
                    { 115, true, null, "Please explain", true, 115, 1140, 3, 8 },
                    { 113, true, null, "Please explain", true, 113, 1121, 3, 8 },
                    { 108, true, null, "Please explain", true, 108, 1071, 3, 8 },
                    { 106, true, null, "Please explain", true, 106, 1051, 3, 8 },
                    { 104, true, null, "Please explain", true, 104, 1031, 3, 8 },
                    { 130, true, null, "Please explain", true, 130, 1291, 3, 8 },
                    { 102, true, null, "Please explain", true, 102, 1001, 3, 8 },
                    { 99, true, null, "Please explain", true, 99, 951, 3, 8 },
                    { 98, true, null, "Disclosure ID", true, 98, 950, 3, 8 },
                    { 97, true, null, "Date actioned", true, 97, 950, 4, 8 },
                    { 96, true, null, "Disclosed or Verified", true, 96, 950, 2, 8 },
                    { 90, true, null, "Please explain", true, 90, 891, 3, 8 },
                    { 92, true, null, "LEX ID", true, 92, 890, 3, 8 },
                    { 91, true, null, "Date Actioned", true, 91, 890, 4, 8 },
                    { 88, true, null, "Please explain", true, 88, 871, 3, 8 },
                    { 101, true, null, "Date actioned", true, 101, 1000, 4, 8 },
                    { 86, true, null, "Please explain", true, 86, 851, 3, 8 },
                    { 132, true, null, "Date actioned", true, 132, 1310, 4, 8 },
                    { 136, true, null, "Date actioned", true, 136, 1350, 4, 8 },
                    { 1040, true, null, "Date", true, 1040, 10300, 4, 170 },
                    { 1020, true, null, "Please investigate/report breach", true, 1020, 10101, 3, 170 },
                    { 168, true, null, "Please explain", true, 168, 1671, 3, 8 },
                    { 166, true, null, "Please explain", true, 166, 1651, 3, 8 },
                    { 164, true, null, "Invoice Number", true, 164, 1620, 3, 8 },
                    { 163, true, null, "Settlement Date", true, 163, 1620, 4, 8 },
                    { 161, true, null, "Date actioned", true, 161, 1600, 4, 8 },
                    { 159, true, null, "Date actioned", true, 159, 1580, 4, 8 },
                    { 134, true, null, "Date actioned", true, 134, 1330, 4, 8 },
                    { 157, true, null, "Please explain", true, 157, 1561, 3, 8 },
                    { 152, true, null, "LEX ID", true, 152, 1510, 3, 8 },
                    { 150, true, null, "Please explain", true, 150, 1491, 3, 8 },
                    { 148, true, null, "Please explain", true, 148, 1441, 3, 8 },
                    { 145, true, null, "Did you spot check the summary against the CSV files?", true, 145, 1440, 2, 8 },
                    { 143, true, null, "Please explain", true, 143, 1421, 3, 8 },
                    { 141, true, null, "Please explain", true, 141, 1401, 3, 8 },
                    { 139, true, null, "Please explain", true, 139, 1381, 3, 8 },
                    { 137, true, null, "Mandatory note", true, 137, 1350, 3, 8 },
                    { 153, true, null, "Please explain", true, 153, 1511, 3, 8 },
                    { 1050, true, null, "Contract IDs", true, 1050, 10300, 8, 170 },
                    { 82, true, null, "Please explain", true, 82, 811, 3, 8 },
                    { 78, true, null, "Please explain", true, 78, 771, 3, 8 },
                    { 32, true, null, "Mandatory Note", true, 32, 291, 3, 2 },
                    { 31, true, null, "Approved limit", true, 31, 291, 3, 2 },
                    { 30, true, null, "Approved limit", true, 30, 290, 3, 2 },
                    { 28, true, null, "Why not?", true, 28, 271, 3, 2 },
                    { 93, true, 270, "Is satisfactory legal opinion held?", true, 93, null, 2, 8 },
                    { 26, true, null, "Why not?", true, 26, 251, 3, 2 },
                    { 24, true, null, "Please comment:", true, 24, 221, 3, 2 },
                    { 23, true, null, "Please comment:", true, 23, 220, 3, 2 },
                    { 34, true, null, "ISDA Held?", true, 34, 330, 2, 2 },
                    { 19, true, null, "Please explain:", true, 19, 181, 3, 2 },
                    { 15, true, null, "Please explain:", true, 15, 141, 3, 2 },
                    { 16, true, null, "Within governance limits?", true, 16, 140, 2, 2 },
                    { 11, true, null, "Please explain:", true, 11, 101, 3, 2 },
                    { 12, true, null, "Within governance limits?", true, 12, 100, 2, 2 },
                    { 7, true, null, "Please explain:", true, 7, 61, 3, 2 },
                    { 8, true, null, "Within governance limits?", true, 8, 60, 2, 2 },
                    { 5, true, null, "Please explain:", true, 5, 41, 3, 2 },
                    { 3, false, null, "Please explain:", true, 3, 21, 3, 1 },
                    { 20, true, null, "Within governance limits?", true, 20, 180, 2, 2 },
                    { 80, true, null, "Please explain", true, 80, 791, 3, 8 },
                    { 117, true, 370, "Is satisfactory legal opinion held?", true, 117, null, 2, 8 },
                    { 41, true, null, "Please explain:", true, 41, 401, 3, 2 },
                    { 76, true, null, "Contract IDs", true, 76, 740, 8, 8 },
                    { 75, true, null, "Date", true, 75, 740, 4, 8 },
                    { 73, true, null, "Has confirmation been received from Commercial Sales Manager?", true, 73, 711, 1, 8 },
                    { 72, true, null, "Obtain details from Commercial Sales Manager.", true, 72, 710, 1, 8 },
                    { 70, true, null, "Record in Breach db, and report.", true, 70, 681, 1, 8 },
                    { 69, true, null, "Mandatory note", true, 69, 681, 3, 8 },
                    { 67, true, null, "Date it was received", true, 67, 660, 4, 8 },
                    { 65, true, null, "Month/Year data is for", true, 65, 630, 6, 8 },
                    { 119, true, 390, "Has Hedge Settlement Agreement (HSA) been sent to and confirmed by Clearing Manager?", true, 119, null, 2, 8 },
                    { 64, true, null, "Date it was received", true, 64, 630, 4, 8 },
                    { 61, true, null, "Date it was sent", true, 61, 600, 4, 8 },
                    { 59, true, null, "Why not?", true, 59, 571, 3, 8 },
                    { 58, true, null, "Date it was sent", true, 58, 570, 4, 8 },
                    { 56, true, null, "Why not?", true, 56, 541, 3, 8 },
                    { 55, true, null, "Date it was sent", true, 55, 540, 4, 8 },
                    { 47, true, null, "Is the deal value (Price x Quantity) less than or equal to the Approved Limit?", true, 47, 461, 2, 2 },
                    { 154, true, 3000, "Counterparty Exposure (sell deals) Attached?", true, 154, null, 2, 8 },
                    { 42, true, null, "Is proposed exposure within approved limit?", true, 42, 3000, 2, 2 },
                    { 62, true, null, "Why not?", true, 62, 601, 3, 8 },
                    { 1070, true, null, "Please explain", true, 1070, 10601, 3, 170 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTaskAnswers",
                columns: new[] { "Id", "Active", "AlternateWorkflowActionId", "AnswerType", "AttachmentTypeToVerifyId", "Description", "WorkflowTaskId" },
                values: new object[,]
                {
                    { 1220, true, null, 1, null, "Yes", 122 },
                    { 421, true, null, 2, null, "No", 42 },
                    { 420, true, null, 1, null, "Yes", 42 },
                    { 201, true, null, 2, null, "No", 20 },
                    { 161, true, null, 2, null, "No", 16 },
                    { 930, true, null, 1, null, "Yes", 93 },
                    { 931, true, 9, 2, null, "No", 93 },
                    { 1191, true, 9, 2, null, "No", 119 },
                    { 1190, true, null, 1, null, "Yes", 119 },
                    { 1450, true, null, 1, null, "Yes", 145 },
                    { 120, true, null, 1, null, "Yes", 12 },
                    { 121, true, null, 2, null, "No", 12 },
                    { 960, true, null, 3, null, "Disclosed", 96 },
                    { 961, true, null, 3, null, "Verified", 96 },
                    { 1171, true, 9, 2, null, "No", 117 },
                    { 1170, true, null, 1, null, "Yes", 117 },
                    { 341, true, null, 2, null, "No", 34 },
                    { 340, true, null, 1, null, "Yes", 34 },
                    { 81, true, null, 2, null, "No", 8 },
                    { 80, true, null, 1, null, "Yes", 8 },
                    { 200, true, null, 1, null, "Yes", 20 },
                    { 1540, true, null, 1, 7, "Yes", 154 },
                    { 1221, true, 9, 2, null, "No", 122 },
                    { 471, true, 3, 2, null, "No", 47 },
                    { 160, true, null, 1, null, "Yes", 16 },
                    { 1541, true, null, 2, null, "No", 154 },
                    { 470, true, null, 1, null, "Yes", 47 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTaskInDealTypes",
                columns: new[] { "WorkflowTaskId", "DealTypeId" },
                values: new object[,]
                {
                    { 101, 2 },
                    { 97, 24 },
                    { 97, 29 },
                    { 106, 2 },
                    { 97, 30 },
                    { 98, 2 },
                    { 98, 4 },
                    { 98, 5 },
                    { 98, 24 },
                    { 104, 2 },
                    { 102, 23 },
                    { 98, 29 },
                    { 102, 30 },
                    { 102, 29 },
                    { 98, 30 },
                    { 101, 4 },
                    { 99, 2 },
                    { 99, 4 },
                    { 99, 5 },
                    { 99, 24 },
                    { 102, 4 },
                    { 102, 2 },
                    { 101, 23 },
                    { 99, 29 },
                    { 101, 30 },
                    { 101, 29 },
                    { 101, 24 },
                    { 101, 5 },
                    { 99, 30 },
                    { 102, 24 },
                    { 102, 5 },
                    { 96, 4 },
                    { 97, 4 },
                    { 80, 6 },
                    { 78, 6 },
                    { 76, 23 },
                    { 76, 30 },
                    { 76, 29 },
                    { 76, 24 },
                    { 76, 5 },
                    { 76, 4 },
                    { 76, 2 },
                    { 76, 6 },
                    { 75, 23 },
                    { 75, 30 },
                    { 75, 29 },
                    { 75, 24 },
                    { 75, 5 },
                    { 80, 2 },
                    { 97, 5 },
                    { 80, 4 },
                    { 80, 24 },
                    { 97, 2 },
                    { 96, 30 },
                    { 96, 29 },
                    { 96, 24 },
                    { 96, 5 },
                    { 106, 4 },
                    { 96, 2 },
                    { 90, 2 },
                    { 92, 2 },
                    { 91, 2 },
                    { 88, 2 },
                    { 86, 2 },
                    { 82, 6 },
                    { 80, 30 },
                    { 80, 29 },
                    { 80, 5 },
                    { 106, 5 },
                    { 108, 4 },
                    { 106, 29 },
                    { 152, 23 },
                    { 150, 19 },
                    { 150, 16 },
                    { 150, 23 },
                    { 148, 20 },
                    { 148, 22 },
                    { 145, 20 },
                    { 145, 22 },
                    { 143, 20 },
                    { 143, 22 },
                    { 141, 20 },
                    { 141, 22 },
                    { 139, 20 },
                    { 139, 22 },
                    { 137, 20 },
                    { 137, 22 },
                    { 136, 20 },
                    { 152, 16 },
                    { 152, 19 },
                    { 153, 23 },
                    { 153, 16 },
                    { 1040, 101 },
                    { 1020, 101 },
                    { 168, 19 },
                    { 168, 16 },
                    { 166, 19 },
                    { 166, 16 },
                    { 164, 19 },
                    { 164, 16 },
                    { 136, 22 },
                    { 163, 19 },
                    { 161, 19 },
                    { 161, 16 },
                    { 159, 19 },
                    { 159, 16 },
                    { 157, 19 },
                    { 157, 16 },
                    { 157, 23 },
                    { 153, 19 },
                    { 163, 16 },
                    { 134, 20 },
                    { 134, 22 },
                    { 132, 20 },
                    { 116, 4 },
                    { 115, 30 },
                    { 115, 29 },
                    { 115, 24 },
                    { 115, 5 },
                    { 115, 4 },
                    { 113, 30 },
                    { 113, 29 },
                    { 116, 5 },
                    { 113, 24 },
                    { 113, 4 },
                    { 108, 30 },
                    { 108, 29 },
                    { 108, 24 },
                    { 108, 5 },
                    { 75, 4 },
                    { 106, 23 },
                    { 106, 30 },
                    { 113, 5 },
                    { 106, 24 },
                    { 116, 24 },
                    { 116, 30 },
                    { 132, 22 },
                    { 130, 30 },
                    { 130, 29 },
                    { 130, 24 },
                    { 130, 5 },
                    { 130, 4 },
                    { 128, 19 },
                    { 128, 16 },
                    { 116, 29 },
                    { 128, 30 },
                    { 128, 24 },
                    { 128, 5 },
                    { 128, 4 },
                    { 126, 30 },
                    { 126, 29 },
                    { 126, 24 },
                    { 126, 5 },
                    { 126, 4 },
                    { 128, 29 },
                    { 75, 2 },
                    { 58, 6 },
                    { 73, 6 },
                    { 16, 29 },
                    { 16, 24 },
                    { 16, 5 },
                    { 16, 4 },
                    { 16, 2 },
                    { 16, 6 },
                    { 11, 20 },
                    { 16, 30 },
                    { 11, 30 },
                    { 11, 24 },
                    { 11, 5 },
                    { 11, 4 },
                    { 11, 2 },
                    { 11, 6 },
                    { 12, 20 },
                    { 12, 30 },
                    { 11, 29 },
                    { 12, 29 },
                    { 16, 20 },
                    { 15, 2 },
                    { 19, 2 },
                    { 19, 6 },
                    { 20, 20 },
                    { 20, 30 },
                    { 20, 29 },
                    { 20, 24 },
                    { 20, 5 },
                    { 15, 6 },
                    { 20, 4 },
                    { 20, 6 },
                    { 15, 20 },
                    { 15, 30 },
                    { 15, 29 },
                    { 15, 24 },
                    { 15, 5 },
                    { 15, 4 },
                    { 20, 2 },
                    { 12, 24 },
                    { 12, 5 },
                    { 12, 4 },
                    { 5, 29 },
                    { 5, 24 },
                    { 5, 5 },
                    { 5, 4 },
                    { 5, 2 },
                    { 5, 6 },
                    { 3, 30 },
                    { 5, 30 },
                    { 3, 29 },
                    { 3, 5 },
                    { 3, 4 },
                    { 122, 30 },
                    { 122, 29 },
                    { 122, 24 },
                    { 122, 5 },
                    { 122, 4 },
                    { 3, 24 },
                    { 5, 22 },
                    { 5, 20 },
                    { 8, 6 },
                    { 12, 2 },
                    { 12, 6 },
                    { 7, 20 },
                    { 7, 30 },
                    { 7, 29 },
                    { 7, 24 },
                    { 7, 5 },
                    { 7, 4 },
                    { 7, 2 },
                    { 7, 6 },
                    { 8, 20 },
                    { 8, 30 },
                    { 8, 29 },
                    { 8, 24 },
                    { 8, 5 },
                    { 8, 4 },
                    { 8, 2 },
                    { 19, 4 },
                    { 75, 6 },
                    { 19, 5 },
                    { 19, 29 },
                    { 119, 24 },
                    { 119, 5 },
                    { 119, 4 },
                    { 117, 19 },
                    { 117, 16 },
                    { 117, 23 },
                    { 117, 30 },
                    { 119, 29 },
                    { 117, 29 },
                    { 117, 5 },
                    { 117, 4 },
                    { 34, 30 },
                    { 34, 29 },
                    { 34, 24 },
                    { 34, 5 },
                    { 34, 4 },
                    { 117, 24 },
                    { 32, 30 },
                    { 119, 30 },
                    { 41, 20 },
                    { 72, 6 },
                    { 70, 6 },
                    { 69, 6 },
                    { 67, 6 },
                    { 65, 6 },
                    { 64, 6 },
                    { 62, 6 },
                    { 41, 22 },
                    { 61, 6 },
                    { 1050, 101 },
                    { 56, 6 },
                    { 55, 6 },
                    { 47, 19 },
                    { 47, 16 },
                    { 154, 23 },
                    { 42, 23 },
                    { 59, 6 },
                    { 32, 29 },
                    { 32, 24 },
                    { 32, 5 },
                    { 24, 30 },
                    { 24, 29 },
                    { 24, 24 },
                    { 24, 5 },
                    { 24, 4 },
                    { 24, 2 },
                    { 24, 6 },
                    { 26, 2 },
                    { 23, 30 },
                    { 23, 24 },
                    { 23, 5 },
                    { 23, 4 },
                    { 23, 2 },
                    { 23, 6 },
                    { 19, 20 },
                    { 19, 30 },
                    { 23, 29 },
                    { 26, 4 },
                    { 26, 5 },
                    { 26, 24 },
                    { 32, 4 },
                    { 31, 30 },
                    { 31, 29 },
                    { 31, 24 },
                    { 31, 5 },
                    { 31, 4 },
                    { 30, 30 },
                    { 30, 29 },
                    { 30, 24 },
                    { 30, 5 },
                    { 30, 4 },
                    { 28, 2 },
                    { 93, 2 },
                    { 26, 19 },
                    { 26, 16 },
                    { 26, 30 },
                    { 26, 29 },
                    { 19, 24 },
                    { 1070, 101 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTasks",
                columns: new[] { "Id", "Active", "DependingUponAnswerId", "Description", "Mandatory", "Order", "PrecedingAnswerId", "Type", "WorkflowActionId" },
                values: new object[,]
                {
                    { 123, true, null, "Date actioned", true, 123, 1220, 4, 8 },
                    { 48, true, null, "Mandatory Note", true, 48, 471, 3, 2 },
                    { 49, true, null, "Are there sufficient NZU/AAU stocks on hand?", true, 49, 470, 2, 2 },
                    { 155, true, null, "Please explain", true, 155, 1541, 3, 8 },
                    { 45, true, null, "Mandatory Note", true, 45, 421, 3, 2 },
                    { 44, true, null, "Approved limit", true, 44, 421, 3, 2 },
                    { 43, true, null, "Approved limit", true, 43, 420, 3, 2 },
                    { 121, true, null, "Please explain", true, 121, 1191, 3, 8 },
                    { 120, true, null, "Date received from Clearing Manager", true, 120, 1190, 4, 8 },
                    { 146, true, null, "Date actioned", true, 146, 1450, 4, 8 },
                    { 118, true, null, "Please explain", true, 118, 1171, 3, 8 },
                    { 36, true, null, "LEX ID", true, 36, 340, 3, 2 },
                    { 35, true, null, "ISDA Date", true, 35, 340, 4, 2 },
                    { 94, true, null, "Please explain", true, 94, 931, 3, 8 },
                    { 21, true, null, "Please explain:", true, 21, 201, 3, 2 },
                    { 17, true, null, "Please explain:", true, 17, 161, 3, 2 },
                    { 13, true, null, "Please explain:", true, 13, 121, 3, 2 },
                    { 9, true, null, "Please explain:", true, 9, 81, 3, 2 },
                    { 124, true, null, "Please explain", true, 124, 1221, 3, 8 },
                    { 109, true, 341, "Is signed ISDA to hand and in order?", true, 109, null, 2, 8 },
                    { 147, true, null, "Mandatory note", true, 147, 1450, 3, 8 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTaskAnswers",
                columns: new[] { "Id", "Active", "AlternateWorkflowActionId", "AnswerType", "AttachmentTypeToVerifyId", "Description", "WorkflowTaskId" },
                values: new object[,]
                {
                    { 1090, true, null, 1, null, "Yes", 109 },
                    { 491, true, 3, 2, null, "No", 49 },
                    { 490, true, null, 1, null, "Yes", 49 },
                    { 1091, true, 9, 2, null, "No", 109 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTaskInDealTypes",
                columns: new[] { "WorkflowTaskId", "DealTypeId" },
                values: new object[,]
                {
                    { 123, 4 },
                    { 118, 23 },
                    { 118, 30 },
                    { 118, 29 },
                    { 118, 24 },
                    { 118, 5 },
                    { 118, 4 },
                    { 109, 30 },
                    { 109, 29 },
                    { 109, 5 },
                    { 118, 16 },
                    { 109, 4 },
                    { 36, 30 },
                    { 36, 29 },
                    { 36, 24 },
                    { 36, 5 },
                    { 36, 4 },
                    { 109, 24 },
                    { 118, 19 },
                    { 120, 4 },
                    { 120, 5 },
                    { 146, 20 },
                    { 146, 22 },
                    { 48, 19 },
                    { 48, 16 },
                    { 49, 19 },
                    { 49, 16 },
                    { 155, 23 },
                    { 45, 23 },
                    { 35, 30 },
                    { 44, 23 },
                    { 121, 30 },
                    { 121, 29 },
                    { 121, 24 },
                    { 121, 5 },
                    { 121, 4 },
                    { 120, 30 },
                    { 120, 29 },
                    { 120, 24 },
                    { 43, 23 },
                    { 35, 29 },
                    { 35, 24 },
                    { 35, 5 },
                    { 13, 2 },
                    { 13, 6 },
                    { 9, 20 },
                    { 9, 30 },
                    { 9, 29 },
                    { 9, 24 },
                    { 9, 5 },
                    { 9, 4 },
                    { 9, 2 },
                    { 9, 6 },
                    { 124, 30 },
                    { 124, 29 },
                    { 124, 24 },
                    { 124, 5 },
                    { 124, 4 },
                    { 123, 30 },
                    { 123, 29 },
                    { 123, 24 },
                    { 123, 5 },
                    { 13, 4 },
                    { 13, 5 },
                    { 13, 24 },
                    { 13, 29 },
                    { 35, 4 },
                    { 94, 2 },
                    { 21, 20 },
                    { 21, 30 },
                    { 21, 29 },
                    { 21, 24 },
                    { 21, 5 },
                    { 21, 4 },
                    { 21, 2 },
                    { 147, 22 },
                    { 21, 6 },
                    { 17, 30 },
                    { 17, 29 },
                    { 17, 24 },
                    { 17, 5 },
                    { 17, 4 },
                    { 17, 2 },
                    { 17, 6 },
                    { 13, 20 },
                    { 13, 30 },
                    { 17, 20 },
                    { 147, 20 }
                });

            migrationBuilder.InsertData(
                table: "WorkflowTasks",
                columns: new[] { "Id", "Active", "DependingUponAnswerId", "Description", "Mandatory", "Order", "PrecedingAnswerId", "Type", "WorkflowActionId" },
                values: new object[] { 110, true, null, "ISDA Date", true, 110, 1090, 4, 8 });

            migrationBuilder.InsertData(
                table: "WorkflowTasks",
                columns: new[] { "Id", "Active", "DependingUponAnswerId", "Description", "Mandatory", "Order", "PrecedingAnswerId", "Type", "WorkflowActionId" },
                values: new object[] { 111, true, null, "LEX ID", true, 111, 1090, 3, 8 });

            migrationBuilder.InsertData(
                table: "WorkflowTasks",
                columns: new[] { "Id", "Active", "DependingUponAnswerId", "Description", "Mandatory", "Order", "PrecedingAnswerId", "Type", "WorkflowActionId" },
                values: new object[] { 50, true, null, "Mandatory Note", true, 50, 491, 3, 2 });

            migrationBuilder.InsertData(
                table: "WorkflowTaskInDealTypes",
                columns: new[] { "WorkflowTaskId", "DealTypeId" },
                values: new object[,]
                {
                    { 110, 4 },
                    { 110, 5 },
                    { 110, 24 },
                    { 110, 29 },
                    { 110, 30 },
                    { 111, 4 },
                    { 111, 5 },
                    { 111, 24 },
                    { 111, 29 },
                    { 111, 30 },
                    { 50, 16 },
                    { 50, 19 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_UserId",
                table: "AuditEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntries_FunctionalityId_EntityId_DateTime",
                table: "AuditEntries",
                columns: new[] { "FunctionalityId", "EntityId", "DateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryFields_AuditEntryTableId",
                table: "AuditEntryFields",
                column: "AuditEntryTableId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditEntryTables_AuditEntryId",
                table: "AuditEntryTables",
                column: "AuditEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigurationEntry_ConfigurationGroupId",
                table: "ConfigurationEntry",
                column: "ConfigurationGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Counterparties_AddressId",
                table: "Counterparties",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Counterparties_CountryId",
                table: "Counterparties",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_CounterpartiesInDealCategories_CounterpartyId",
                table: "CounterpartiesInDealCategories",
                column: "CounterpartyId");

            migrationBuilder.CreateIndex(
                name: "IX_DealAttachments_AttachmentTypeId",
                table: "DealAttachments",
                column: "AttachmentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DealAttachments_DealId",
                table: "DealAttachments",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealAttachmentVersions_CreationUserId",
                table: "DealAttachmentVersions",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealAttachmentVersions_DealAttachmentId",
                table: "DealAttachmentVersions",
                column: "DealAttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DealItemFields_DealItemFieldsetId",
                table: "DealItemFields",
                column: "DealItemFieldsetId");

            migrationBuilder.CreateIndex(
                name: "IX_DealItems_DealId",
                table: "DealItems",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealItems_OriginalItemId",
                table: "DealItems",
                column: "OriginalItemId");

            migrationBuilder.CreateIndex(
                name: "IX_DealItems_ProductId",
                table: "DealItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_DealItems_SourceDataId",
                table: "DealItems",
                column: "SourceDataId");

            migrationBuilder.CreateIndex(
                name: "IX_DealItemSourceData_SourceId",
                table: "DealItemSourceData",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_DealNotes_CreationUserId",
                table: "DealNotes",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealNotes_DealId",
                table: "DealNotes",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealNotes_ReminderUserId",
                table: "DealNotes",
                column: "ReminderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CounterpartyId",
                table: "Deals",
                column: "CounterpartyId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CreationUserId",
                table: "Deals",
                column: "CreationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_CurrentDealWorkflowStatusId",
                table: "Deals",
                column: "CurrentDealWorkflowStatusId",
                unique: true,
                filter: "[CurrentDealWorkflowStatusId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_DealCategoryId",
                table: "Deals",
                column: "DealCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_DealTypeId",
                table: "Deals",
                column: "DealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_DelegatedAuthorityUserId",
                table: "Deals",
                column: "DelegatedAuthorityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_ExecutionUserId",
                table: "Deals",
                column: "ExecutionUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_NextDealWorkflowStatusId",
                table: "Deals",
                column: "NextDealWorkflowStatusId",
                unique: true,
                filter: "[NextDealWorkflowStatusId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_OngoingWorkflowActionId",
                table: "Deals",
                column: "OngoingWorkflowActionId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_PreviousDealWorkflowStatusId",
                table: "Deals",
                column: "PreviousDealWorkflowStatusId",
                unique: true,
                filter: "[PreviousDealWorkflowStatusId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_WorkflowSetId",
                table: "Deals",
                column: "WorkflowSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Deals_SubmissionUserId_DealCategoryId_DealTypeId_CounterpartyId",
                table: "Deals",
                columns: new[] { "SubmissionUserId", "DealCategoryId", "DealTypeId", "CounterpartyId" });

            migrationBuilder.CreateIndex(
                name: "IX_DealTypes_DealItemFieldsetId",
                table: "DealTypes",
                column: "DealItemFieldsetId");

            migrationBuilder.CreateIndex(
                name: "IX_DealTypes_TraderAuthorityPolicyId",
                table: "DealTypes",
                column: "TraderAuthorityPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_DealTypes_WorkflowSetId",
                table: "DealTypes",
                column: "WorkflowSetId");

            migrationBuilder.CreateIndex(
                name: "IX_DealTypesInDealCategories_DealTypeId",
                table: "DealTypesInDealCategories",
                column: "DealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowActionListeners_DealWorkflowStatusId",
                table: "DealWorkflowActionListeners",
                column: "DealWorkflowStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowActionListeners_UserId",
                table: "DealWorkflowActionListeners",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowStatuses_AssigneeUserId",
                table: "DealWorkflowStatuses",
                column: "AssigneeUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowStatuses_AssigneeWorkflowRoleId",
                table: "DealWorkflowStatuses",
                column: "AssigneeWorkflowRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowStatuses_DealId",
                table: "DealWorkflowStatuses",
                column: "DealId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowStatuses_InitiatedByUserId",
                table: "DealWorkflowStatuses",
                column: "InitiatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowStatuses_PrecedingWorkflowActionId",
                table: "DealWorkflowStatuses",
                column: "PrecedingWorkflowActionId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowStatuses_WorkflowStatusId",
                table: "DealWorkflowStatuses",
                column: "WorkflowStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowTasks_DealWorkflowStatusId",
                table: "DealWorkflowTasks",
                column: "DealWorkflowStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowTasks_WorkflowTaskAnswerId",
                table: "DealWorkflowTasks",
                column: "WorkflowTaskAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_DealWorkflowTasks_WorkflowTaskId",
                table: "DealWorkflowTasks",
                column: "WorkflowTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalitiesInUserRoles_FunctionalityId",
                table: "FunctionalitiesInUserRoles",
                column: "FunctionalityId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionalitiesInUserRoles_UserRoleId",
                table: "FunctionalitiesInUserRoles",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationRunEntries_IntegrationRunId",
                table: "IntegrationRunEntries",
                column: "IntegrationRunId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationRuns_UserId",
                table: "IntegrationRuns",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_DealCategoryId",
                table: "Products",
                column: "DealCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFunctionalities_FunctionalityId",
                table: "SubFunctionalities",
                column: "FunctionalityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFunctionalities_ParentSubFunctionalityId",
                table: "SubFunctionalities",
                column: "ParentSubFunctionalityId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFunctionalitiesInUserRoles_FunctionalityInUserRoleId",
                table: "SubFunctionalitiesInUserRoles",
                column: "FunctionalityInUserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SubFunctionalitiesInUserRoles_SubFunctionalityId",
                table: "SubFunctionalitiesInUserRoles",
                column: "SubFunctionalityId");

            migrationBuilder.CreateIndex(
                name: "IX_TraderAuthorityPoliciesCriteria_TraderAuthorityPolicyId",
                table: "TraderAuthorityPoliciesCriteria",
                column: "TraderAuthorityPolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_TraderAuthorityPoliciesCriteria_WorkflowRoleId",
                table: "TraderAuthorityPoliciesCriteria",
                column: "WorkflowRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserIntegrationData_UserId",
                table: "UserIntegrationData",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserRoleId",
                table: "Users",
                column: "UserRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersInWorkflowRoles_WorkflowRoleId",
                table: "UsersInWorkflowRoles",
                column: "WorkflowRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowActions_SourceWorkflowStatusId",
                table: "WorkflowActions",
                column: "SourceWorkflowStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowActions_TargetWorkflowStatusId",
                table: "WorkflowActions",
                column: "TargetWorkflowStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStatuses_WorkflowRoleId",
                table: "WorkflowStatuses",
                column: "WorkflowRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStatuses_WorkflowSetId",
                table: "WorkflowStatuses",
                column: "WorkflowSetId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskAnswers_AlternateWorkflowActionId",
                table: "WorkflowTaskAnswers",
                column: "AlternateWorkflowActionId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskAnswers_AttachmentTypeToVerifyId",
                table: "WorkflowTaskAnswers",
                column: "AttachmentTypeToVerifyId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskAnswers_WorkflowTaskId",
                table: "WorkflowTaskAnswers",
                column: "WorkflowTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTaskInDealTypes_DealTypeId",
                table: "WorkflowTaskInDealTypes",
                column: "DealTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTasks_DependingUponAnswerId",
                table: "WorkflowTasks",
                column: "DependingUponAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTasks_PrecedingAnswerId",
                table: "WorkflowTasks",
                column: "PrecedingAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTasks_WorkflowActionId",
                table: "WorkflowTasks",
                column: "WorkflowActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DealAttachments_Deals_DealId",
                table: "DealAttachments",
                column: "DealId",
                principalTable: "Deals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTaskAnswers_WorkflowTasks_WorkflowTaskId",
                table: "WorkflowTaskAnswers",
                column: "WorkflowTaskId",
                principalTable: "WorkflowTasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_DealWorkflowStatuses_CurrentDealWorkflowStatusId",
                table: "Deals",
                column: "CurrentDealWorkflowStatusId",
                principalTable: "DealWorkflowStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_DealWorkflowStatuses_NextDealWorkflowStatusId",
                table: "Deals",
                column: "NextDealWorkflowStatusId",
                principalTable: "DealWorkflowStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Deals_DealWorkflowStatuses_PreviousDealWorkflowStatusId",
                table: "Deals",
                column: "PreviousDealWorkflowStatusId",
                principalTable: "DealWorkflowStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Users_CreationUserId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Users_DelegatedAuthorityUserId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Users_ExecutionUserId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Users_SubmissionUserId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_DealWorkflowStatuses_Users_AssigneeUserId",
                table: "DealWorkflowStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_DealWorkflowStatuses_Users_InitiatedByUserId",
                table: "DealWorkflowStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_Counterparties_Address_AddressId",
                table: "Counterparties");

            migrationBuilder.DropForeignKey(
                name: "FK_Counterparties_Countries_CountryId",
                table: "Counterparties");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_Counterparties_CounterpartyId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_Deals_DealCategories_DealCategoryId",
                table: "Deals");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTaskAnswers_AttachmentTypes_AttachmentTypeToVerifyId",
                table: "WorkflowTaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_DealWorkflowStatuses_Deals_DealId",
                table: "DealWorkflowStatuses");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTaskAnswers_WorkflowActions_AlternateWorkflowActionId",
                table: "WorkflowTaskAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTasks_WorkflowActions_WorkflowActionId",
                table: "WorkflowTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTasks_WorkflowTaskAnswers_DependingUponAnswerId",
                table: "WorkflowTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTasks_WorkflowTaskAnswers_PrecedingAnswerId",
                table: "WorkflowTasks");

            migrationBuilder.DropTable(
                name: "AuditEntryFields");

            migrationBuilder.DropTable(
                name: "ConfigurationEntry");

            migrationBuilder.DropTable(
                name: "CounterpartiesInDealCategories");

            migrationBuilder.DropTable(
                name: "DealAttachmentVersions");

            migrationBuilder.DropTable(
                name: "DealCodeSequences");

            migrationBuilder.DropTable(
                name: "DealItemFields");

            migrationBuilder.DropTable(
                name: "DealItems");

            migrationBuilder.DropTable(
                name: "DealNotes");

            migrationBuilder.DropTable(
                name: "DealTypesInDealCategories");

            migrationBuilder.DropTable(
                name: "DealWorkflowActionListeners");

            migrationBuilder.DropTable(
                name: "DealWorkflowTasks");

            migrationBuilder.DropTable(
                name: "IntegrationRunEntries");

            migrationBuilder.DropTable(
                name: "SalesForecasts");

            migrationBuilder.DropTable(
                name: "SubFunctionalitiesInUserRoles");

            migrationBuilder.DropTable(
                name: "TraderAuthorityPoliciesCriteria");

            migrationBuilder.DropTable(
                name: "UserIntegrationData");

            migrationBuilder.DropTable(
                name: "UsersInWorkflowRoles");

            migrationBuilder.DropTable(
                name: "WorkflowTaskInDealTypes");

            migrationBuilder.DropTable(
                name: "AuditEntryTables");

            migrationBuilder.DropTable(
                name: "ConfigurationGroup");

            migrationBuilder.DropTable(
                name: "DealAttachments");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "DealItemSourceData");

            migrationBuilder.DropTable(
                name: "IntegrationRuns");

            migrationBuilder.DropTable(
                name: "FunctionalitiesInUserRoles");

            migrationBuilder.DropTable(
                name: "SubFunctionalities");

            migrationBuilder.DropTable(
                name: "AuditEntries");

            migrationBuilder.DropTable(
                name: "Functionalities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Address");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Counterparties");

            migrationBuilder.DropTable(
                name: "DealCategories");

            migrationBuilder.DropTable(
                name: "AttachmentTypes");

            migrationBuilder.DropTable(
                name: "Deals");

            migrationBuilder.DropTable(
                name: "DealWorkflowStatuses");

            migrationBuilder.DropTable(
                name: "DealTypes");

            migrationBuilder.DropTable(
                name: "DealItemFieldsets");

            migrationBuilder.DropTable(
                name: "TraderAuthorityPolicies");

            migrationBuilder.DropTable(
                name: "WorkflowActions");

            migrationBuilder.DropTable(
                name: "WorkflowStatuses");

            migrationBuilder.DropTable(
                name: "WorkflowRoles");

            migrationBuilder.DropTable(
                name: "WorkflowSets");

            migrationBuilder.DropTable(
                name: "WorkflowTaskAnswers");

            migrationBuilder.DropTable(
                name: "WorkflowTasks");
        }
    }
}
