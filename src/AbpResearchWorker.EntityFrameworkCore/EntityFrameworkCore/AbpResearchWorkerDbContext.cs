using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.BlobStoring.Database.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.TenantManagement;
using Volo.Abp.TenantManagement.EntityFrameworkCore;
using AbpResearchWorker.JobDescriptions;

namespace AbpResearchWorker.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ReplaceDbContext(typeof(ITenantManagementDbContext))]
[ConnectionStringName("Default")]
public class AbpResearchWorkerDbContext :
    AbpDbContext<AbpResearchWorkerDbContext>,
    ITenantManagementDbContext,
    IIdentityDbContext
{
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

    #region Job Description System Entities
    
    /// <summary>
    /// Job descriptions with AI analysis results
    /// </summary>
    public DbSet<JobDescription> JobDescriptions { get; set; }
    
    /// <summary>
    /// AI-generated interview questions for job descriptions
    /// </summary>
    public DbSet<InterviewQuestion> InterviewQuestions { get; set; }
    
    #endregion

    #region Entities from the modules

    /* Notice: We only implemented IIdentityProDbContext and ISaasDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityProDbContext and ISaasDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    // Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    public DbSet<IdentityUserDelegation> UserDelegations { get; set; }
    public DbSet<IdentitySession> Sessions { get; set; }

    // Tenant Management
    public DbSet<Tenant> Tenants { get; set; }
    public DbSet<TenantConnectionString> TenantConnectionStrings { get; set; }

    #endregion

    public AbpResearchWorkerDbContext(DbContextOptions<AbpResearchWorkerDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureFeatureManagement();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureTenantManagement();
        builder.ConfigureBlobStoring();
        
        /* Configure your own tables/entities inside here */

        // Configure JobDescription entity
        builder.Entity<JobDescription>(b =>
        {
            b.ToTable(AbpResearchWorkerConsts.DbTablePrefix + "JobDescriptions", AbpResearchWorkerConsts.DbSchema);
            b.ConfigureByConvention(); // Auto configure for the base class props
            
            // Basic job properties
            b.Property(x => x.Title).IsRequired().HasMaxLength(200);
            b.Property(x => x.Company).IsRequired().HasMaxLength(100);
            b.Property(x => x.Description).IsRequired();
            b.Property(x => x.Requirements);
            b.Property(x => x.Location).HasMaxLength(100);
            b.Property(x => x.SalaryRange).HasMaxLength(50);
            
            // URL extraction & source properties
            b.Property(x => x.OriginalUrl).HasMaxLength(500);
            b.Property(x => x.JobBoardName).HasMaxLength(100);
            b.Property(x => x.SourceDomain).HasMaxLength(100);
            b.Property(x => x.InputType).IsRequired();
            b.Property(x => x.ExternalJobId).HasMaxLength(100);
            b.Property(x => x.ExtractedAt);
            b.Property(x => x.CountryCode).HasMaxLength(10);
            b.Property(x => x.Language).HasMaxLength(10);
            b.Property(x => x.Currency).HasMaxLength(10);
            b.Property(x => x.JobType).HasMaxLength(50);
            b.Property(x => x.ApplicationDeadline);
            b.Property(x => x.OriginalPostDate);
            b.Property(x => x.CompanyLogoUrl).HasMaxLength(500);
            b.Property(x => x.RawContent);
            
            // AI analysis properties
            b.Property(x => x.Status).IsRequired();
            b.Property(x => x.KeySkills);
            b.Property(x => x.ExperienceLevel).IsRequired();
            b.Property(x => x.Complexity).IsRequired();
            b.Property(x => x.Industry).HasMaxLength(50);
            b.Property(x => x.CompanySize).IsRequired();
            b.Property(x => x.CompanyInsights);
            b.Property(x => x.AnalyzedAt);
            b.Property(x => x.ErrorMessage).HasMaxLength(500);
            b.Property(x => x.TokensUsed).IsRequired();
            b.Property(x => x.AnalysisCost).HasColumnType("decimal(18,4)").IsRequired();
            b.Property(x => x.ModelUsed).HasMaxLength(50);
            
            // Indexes for performance
            b.HasIndex(x => x.Status);
            b.HasIndex(x => x.Company);
            b.HasIndex(x => x.CreationTime);
            b.HasIndex(x => x.InputType);
            b.HasIndex(x => x.SourceDomain);
            b.HasIndex(x => x.CountryCode);
            b.HasIndex(x => x.ApplicationDeadline);
            b.HasIndex(x => x.OriginalUrl);
            b.HasIndex(x => x.ExternalJobId);
            b.HasIndex(x => new { x.Status, x.CreationTime });
            b.HasIndex(x => new { x.InputType, x.SourceDomain });
            b.HasIndex(x => new { x.CountryCode, x.Language });
            
            // Relationships
            b.HasMany(x => x.Questions)
                .WithOne(x => x.JobDescription)
                .HasForeignKey(x => x.JobDescriptionId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure InterviewQuestion entity
        builder.Entity<InterviewQuestion>(b =>
        {
            b.ToTable(AbpResearchWorkerConsts.DbTablePrefix + "InterviewQuestions", AbpResearchWorkerConsts.DbSchema);
            b.ConfigureByConvention(); // Auto configure for the base class props
            
            // Properties
            b.Property(x => x.JobDescriptionId).IsRequired();
            b.Property(x => x.QuestionText).IsRequired();
            b.Property(x => x.Category).IsRequired();
            b.Property(x => x.Difficulty).IsRequired();
            b.Property(x => x.Order).IsRequired();
            b.Property(x => x.ExpectedAnswerPoints);
            b.Property(x => x.SampleAnswer);
            b.Property(x => x.FollowUpQuestions);
            b.Property(x => x.InterviewerTips);
            b.Property(x => x.SkillsEvaluated);
            b.Property(x => x.TokensUsed).IsRequired();
            b.Property(x => x.GenerationCost).HasColumnType("decimal(18,4)").IsRequired();
            b.Property(x => x.ModelUsed).HasMaxLength(50);
            b.Property(x => x.GeneratedAt).IsRequired();
            b.Property(x => x.QualityScore).IsRequired();
            b.Property(x => x.IsApproved).IsRequired();
            b.Property(x => x.UsageCount).IsRequired();
            b.Property(x => x.EffectivenessRating).HasColumnType("decimal(18,2)").IsRequired();
            
            // Indexes for performance
            b.HasIndex(x => x.JobDescriptionId);
            b.HasIndex(x => x.Category);
            b.HasIndex(x => x.Difficulty);
            b.HasIndex(x => x.IsApproved);
            b.HasIndex(x => new { x.JobDescriptionId, x.Order });
            b.HasIndex(x => new { x.Category, x.Difficulty });
            
            // Relationships are already configured in JobDescription
        });
    }
}
