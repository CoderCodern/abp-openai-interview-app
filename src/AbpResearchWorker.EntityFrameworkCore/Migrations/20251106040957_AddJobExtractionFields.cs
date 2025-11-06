using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbpResearchWorker.Migrations
{
    /// <inheritdoc />
    public partial class AddJobExtractionFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppJobDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Company = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SalaryRange = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OriginalUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    JobBoardName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SourceDomain = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    InputType = table.Column<int>(type: "int", nullable: false),
                    ExternalJobId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExtractedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Language = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    JobType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApplicationDeadline = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OriginalPostDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CompanyLogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RawContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    KeySkills = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExperienceLevel = table.Column<int>(type: "int", nullable: false),
                    Complexity = table.Column<int>(type: "int", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CompanySize = table.Column<int>(type: "int", nullable: false),
                    CompanyInsights = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnalyzedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ErrorMessage = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: false),
                    AnalysisCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ModelUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppJobDescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppInterviewQuestions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobDescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ExpectedAnswerPoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SampleAnswer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FollowUpQuestions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterviewerTips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SkillsEvaluated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokensUsed = table.Column<int>(type: "int", nullable: false),
                    GenerationCost = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    ModelUsed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QualityScore = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: false),
                    UsageCount = table.Column<int>(type: "int", nullable: false),
                    EffectivenessRating = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppInterviewQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppInterviewQuestions_AppJobDescriptions_JobDescriptionId",
                        column: x => x.JobDescriptionId,
                        principalTable: "AppJobDescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppInterviewQuestions_Category",
                table: "AppInterviewQuestions",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_AppInterviewQuestions_Category_Difficulty",
                table: "AppInterviewQuestions",
                columns: new[] { "Category", "Difficulty" });

            migrationBuilder.CreateIndex(
                name: "IX_AppInterviewQuestions_Difficulty",
                table: "AppInterviewQuestions",
                column: "Difficulty");

            migrationBuilder.CreateIndex(
                name: "IX_AppInterviewQuestions_IsApproved",
                table: "AppInterviewQuestions",
                column: "IsApproved");

            migrationBuilder.CreateIndex(
                name: "IX_AppInterviewQuestions_JobDescriptionId",
                table: "AppInterviewQuestions",
                column: "JobDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppInterviewQuestions_JobDescriptionId_Order",
                table: "AppInterviewQuestions",
                columns: new[] { "JobDescriptionId", "Order" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_ApplicationDeadline",
                table: "AppJobDescriptions",
                column: "ApplicationDeadline");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_Company",
                table: "AppJobDescriptions",
                column: "Company");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_CountryCode",
                table: "AppJobDescriptions",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_CountryCode_Language",
                table: "AppJobDescriptions",
                columns: new[] { "CountryCode", "Language" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_CreationTime",
                table: "AppJobDescriptions",
                column: "CreationTime");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_ExternalJobId",
                table: "AppJobDescriptions",
                column: "ExternalJobId");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_InputType",
                table: "AppJobDescriptions",
                column: "InputType");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_InputType_SourceDomain",
                table: "AppJobDescriptions",
                columns: new[] { "InputType", "SourceDomain" });

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_OriginalUrl",
                table: "AppJobDescriptions",
                column: "OriginalUrl");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_SourceDomain",
                table: "AppJobDescriptions",
                column: "SourceDomain");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_Status",
                table: "AppJobDescriptions",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_AppJobDescriptions_Status_CreationTime",
                table: "AppJobDescriptions",
                columns: new[] { "Status", "CreationTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppInterviewQuestions");

            migrationBuilder.DropTable(
                name: "AppJobDescriptions");
        }
    }
}
