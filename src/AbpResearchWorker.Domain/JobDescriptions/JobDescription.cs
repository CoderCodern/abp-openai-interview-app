using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpResearchWorker.JobDescriptions;

/// <summary>
/// JobDescription aggregate root representing a job posting with AI analysis
/// </summary>
public class JobDescription : FullAuditedAggregateRoot<Guid>
{
    #region Basic Job Information

    /// <summary>
    /// Job title/position name
    /// </summary>
    [Required]
    [StringLength(200)]
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// Company name
    /// </summary>
    [Required]
    [StringLength(100)]
    public string Company { get; private set; } = string.Empty;

    /// <summary>
    /// Full job description content
    /// </summary>
    [Required]
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// Extracted requirements and qualifications
    /// </summary>
    public string? Requirements { get; private set; }

    /// <summary>
    /// Job location (optional)
    /// </summary>
    [StringLength(100)]
    public string? Location { get; private set; }

    /// <summary>
    /// Salary range if provided (optional)
    /// </summary>
    [StringLength(50)]
    public string? SalaryRange { get; private set; }

    #endregion

    #region URL Extraction & Source Information

    /// <summary>
    /// Original job posting URL if extracted from link
    /// </summary>
    [StringLength(500)]
    public string? OriginalUrl { get; private set; }

    /// <summary>
    /// Job board/source name (LinkedIn, Indeed, TopCV, VietnamWorks, etc.)
    /// </summary>
    [StringLength(100)]
    public string? JobBoardName { get; private set; }

    /// <summary>
    /// Domain of the source website
    /// </summary>
    [StringLength(100)]
    public string? SourceDomain { get; private set; }

    /// <summary>
    /// How the job description was obtained
    /// </summary>
    public JobDescriptionSource InputType { get; private set; }

    /// <summary>
    /// Job posting ID from the source site
    /// </summary>
    [StringLength(100)]
    public string? ExternalJobId { get; private set; }

    /// <summary>
    /// When the job posting was extracted/scraped
    /// </summary>
    public DateTime? ExtractedAt { get; private set; }

    /// <summary>
    /// Country/region code (VN, US, SG, etc.) for localization
    /// </summary>
    [StringLength(10)]
    public string? CountryCode { get; private set; }

    /// <summary>
    /// Primary language of the original posting (vi, en, zh, etc.)
    /// </summary>
    [StringLength(10)]
    public string? Language { get; private set; }

    /// <summary>
    /// Currency used for salary (VND, USD, SGD, etc.)
    /// </summary>
    [StringLength(10)]
    public string? Currency { get; private set; }

    /// <summary>
    /// Job type (full-time, part-time, contract, internship, etc.)
    /// </summary>
    [StringLength(50)]
    public string? JobType { get; private set; }

    /// <summary>
    /// Application deadline if specified
    /// </summary>
    public DateTime? ApplicationDeadline { get; private set; }

    /// <summary>
    /// When the job was originally posted on the source site
    /// </summary>
    public DateTime? OriginalPostDate { get; private set; }

    /// <summary>
    /// Company logo URL from the job posting
    /// </summary>
    [StringLength(500)]
    public string? CompanyLogoUrl { get; private set; }

    /// <summary>
    /// Raw extracted content before AI processing (for debugging/reprocessing)
    /// </summary>
    public string? RawContent { get; private set; }

    #endregion

    #region AI Analysis Results

    /// <summary>
    /// Current status of AI analysis
    /// </summary>
    public JobAnalysisStatus Status { get; private set; }

    /// <summary>
    /// AI-extracted key skills (JSON array of strings)
    /// </summary>
    public string? KeySkills { get; private set; }

    /// <summary>
    /// Detected experience level requirement
    /// </summary>
    public ExperienceLevel ExperienceLevel { get; private set; }

    /// <summary>
    /// Job complexity assessment
    /// </summary>
    public JobComplexity Complexity { get; private set; }

    /// <summary>
    /// Industry/domain classification
    /// </summary>
    [StringLength(50)]
    public string? Industry { get; private set; }

    /// <summary>
    /// Estimated company size
    /// </summary>
    public CompanySize CompanySize { get; private set; }

    /// <summary>
    /// AI-generated company insights (JSON)
    /// </summary>
    public string? CompanyInsights { get; private set; }

    /// <summary>
    /// When analysis was completed
    /// </summary>
    public DateTime? AnalyzedAt { get; private set; }

    /// <summary>
    /// Analysis error message if failed
    /// </summary>
    [StringLength(500)]
    public string? ErrorMessage { get; private set; }

    #endregion

    #region AI Usage Tracking

    /// <summary>
    /// Number of OpenAI tokens used for analysis
    /// </summary>
    public int TokensUsed { get; private set; }

    /// <summary>
    /// Cost of AI analysis in USD
    /// </summary>
    public decimal AnalysisCost { get; private set; }

    /// <summary>
    /// OpenAI model used for analysis
    /// </summary>
    [StringLength(50)]
    public string? ModelUsed { get; private set; }

    #endregion

    #region Relationships

    /// <summary>
    /// Generated interview questions for this job
    /// </summary>
    public virtual ICollection<InterviewQuestion> Questions { get; private set; } = new List<InterviewQuestion>();

    #endregion

    #region Constructors

    protected JobDescription()
    {
        // For EF Core
    }

    public JobDescription(
        Guid id,
        string title,
        string company,
        string description,
        string? requirements = null,
        string? location = null,
        string? salaryRange = null
    ) : base(id)
    {
        SetTitle(title);
        SetCompany(company);
        SetDescription(description);
        SetRequirements(requirements);
        SetLocation(location);
        SetSalaryRange(salaryRange);
        
        // Initialize default values
        Status = JobAnalysisStatus.Pending;
        ExperienceLevel = ExperienceLevel.EntryLevel;
        Complexity = JobComplexity.Entry;
        CompanySize = CompanySize.Unknown;
        TokensUsed = 0;
        AnalysisCost = 0;
        InputType = JobDescriptionSource.ManualText;
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Update basic job information
    /// </summary>
    public void UpdateJobInfo(
        string title,
        string company,
        string description,
        string? requirements = null,
        string? location = null,
        string? salaryRange = null)
    {
        SetTitle(title);
        SetCompany(company);
        SetDescription(description);
        SetRequirements(requirements);
        SetLocation(location);
        SetSalaryRange(salaryRange);
        
        // Reset analysis when job info changes
        if (Status == JobAnalysisStatus.Completed)
        {
            Status = JobAnalysisStatus.Pending;
            AnalyzedAt = null;
        }
    }

    /// <summary>
    /// Mark analysis as started
    /// </summary>
    public void StartAnalysis()
    {
        if (Status != JobAnalysisStatus.Pending)
        {
            throw new BusinessException("Job description must be in Pending status to start analysis");
        }

        Status = JobAnalysisStatus.InProgress;
    }

    /// <summary>
    /// Complete analysis with results
    /// </summary>
    public void CompleteAnalysis(
        string keySkills,
        ExperienceLevel experienceLevel,
        JobComplexity complexity,
        string? industry = null,
        CompanySize companySize = CompanySize.Unknown,
        string? companyInsights = null,
        int tokensUsed = 0,
        decimal analysisCost = 0,
        string? modelUsed = null)
    {
        if (Status != JobAnalysisStatus.InProgress)
        {
            throw new BusinessException("Job description must be in InProgress status to complete analysis");
        }

        KeySkills = keySkills;
        ExperienceLevel = experienceLevel;
        Complexity = complexity;
        Industry = industry;
        CompanySize = companySize;
        CompanyInsights = companyInsights;
        TokensUsed = tokensUsed;
        AnalysisCost = analysisCost;
        ModelUsed = modelUsed;
        Status = JobAnalysisStatus.Completed;
        AnalyzedAt = DateTime.UtcNow;
        ErrorMessage = null;
    }

    /// <summary>
    /// Mark analysis as failed
    /// </summary>
    public void FailAnalysis(string errorMessage)
    {
        if (Status != JobAnalysisStatus.InProgress)
        {
            throw new BusinessException("Job description must be in InProgress status to fail analysis");
        }

        Status = JobAnalysisStatus.Failed;
        ErrorMessage = Check.Length(errorMessage, nameof(errorMessage), 500);
    }

    /// <summary>
    /// Mark analysis as expired (needs refresh)
    /// </summary>
    public void ExpireAnalysis()
    {
        if (Status == JobAnalysisStatus.Completed)
        {
            Status = JobAnalysisStatus.Expired;
        }
    }

    /// <summary>
    /// Reset analysis to allow re-processing
    /// </summary>
    public void ResetAnalysis()
    {
        Status = JobAnalysisStatus.Pending;
        KeySkills = null;
        ExperienceLevel = ExperienceLevel.EntryLevel;
        Complexity = JobComplexity.Entry;
        Industry = null;
        CompanySize = CompanySize.Unknown;
        CompanyInsights = null;
        AnalyzedAt = null;
        ErrorMessage = null;
        TokensUsed = 0;
        AnalysisCost = 0;
        ModelUsed = null;
    }

    /// <summary>
    /// Add an interview question to this job description
    /// </summary>
    public void AddQuestion(InterviewQuestion question)
    {
        Check.NotNull(question, nameof(question));
        Questions.Add(question);
    }

    /// <summary>
    /// Get total cost for this job including all questions
    /// </summary>
    public decimal GetTotalCost()
    {
        decimal questionsCost = 0;
        foreach (var question in Questions)
        {
            questionsCost += question.GenerationCost;
        }
        return AnalysisCost + questionsCost;
    }

    /// <summary>
    /// Get total tokens used for this job including all questions
    /// </summary>
    public int GetTotalTokens()
    {
        int questionsTokens = 0;
        foreach (var question in Questions)
        {
            questionsTokens += question.TokensUsed;
        }
        return TokensUsed + questionsTokens;
    }

    /// <summary>
    /// Check if analysis is ready for question generation
    /// </summary>
    public bool IsReadyForQuestions()
    {
        return Status == JobAnalysisStatus.Completed && !string.IsNullOrEmpty(KeySkills);
    }

    /// <summary>
    /// Set URL extraction information when job is scraped from a website
    /// </summary>
    public void SetExtractionInfo(
        string originalUrl,
        string jobBoardName,
        JobDescriptionSource inputType,
        string? externalJobId = null,
        string? sourceDomain = null,
        string? countryCode = null,
        string? language = null,
        string? currency = null,
        string? rawContent = null)
    {
        OriginalUrl = Check.Length(originalUrl, nameof(originalUrl), 500);
        JobBoardName = Check.Length(jobBoardName, nameof(jobBoardName), 100);
        InputType = inputType;
        ExternalJobId = Check.Length(externalJobId, nameof(externalJobId), 100);
        SourceDomain = Check.Length(sourceDomain, nameof(sourceDomain), 100);
        CountryCode = Check.Length(countryCode, nameof(countryCode), 10);
        Language = Check.Length(language, nameof(language), 10);
        Currency = Check.Length(currency, nameof(currency), 10);
        RawContent = rawContent;
        ExtractedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Set additional job metadata from extraction
    /// </summary>
    public void SetJobMetadata(
        string? jobType = null,
        DateTime? applicationDeadline = null,
        DateTime? originalPostDate = null,
        string? companyLogoUrl = null)
    {
        JobType = Check.Length(jobType, nameof(jobType), 50);
        ApplicationDeadline = applicationDeadline;
        OriginalPostDate = originalPostDate;
        CompanyLogoUrl = Check.Length(companyLogoUrl, nameof(companyLogoUrl), 500);
    }

    /// <summary>
    /// Check if this job was extracted from a URL
    /// </summary>
    public bool IsExtractedFromUrl()
    {
        return !string.IsNullOrEmpty(OriginalUrl) && 
               (InputType == JobDescriptionSource.UrlScraping || 
                InputType == JobDescriptionSource.JobBoardApi ||
                InputType == JobDescriptionSource.BrowserAutomation);
    }

    /// <summary>
    /// Check if this job is from a Vietnamese job board
    /// </summary>
    public bool IsVietnameseJobBoard()
    {
        if (string.IsNullOrEmpty(SourceDomain))
            return false;

        var vietnameseDomains = new[]
        {
            "topcv.vn", "vietnamworks.com", "itviec.com", "careerbuilder.vn",
            "jobsgo.vn", "mywork.com.vn", "vieclam24h.vn", "timviec365.vn"
        };

        return vietnameseDomains.Any(domain => 
            SourceDomain.Contains(domain, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Get display name for the job source
    /// </summary>
    public string GetJobSourceDisplayName()
    {
        return InputType switch
        {
            JobDescriptionSource.ManualText => "Manual Input",
            JobDescriptionSource.UrlScraping => $"Scraped from {JobBoardName ?? "Website"}",
            JobDescriptionSource.JobBoardApi => $"API from {JobBoardName ?? "Job Board"}",
            JobDescriptionSource.FileUpload => "File Upload",
            JobDescriptionSource.BrowserAutomation => $"Browser from {JobBoardName ?? "Website"}",
            JobDescriptionSource.RssFeed => $"RSS from {JobBoardName ?? "Feed"}",
            _ => "Unknown Source"
        };
    }

    /// <summary>
    /// Check if job posting is still active (not past deadline)
    /// </summary>
    public bool IsJobActive()
    {
        return ApplicationDeadline == null || ApplicationDeadline > DateTime.UtcNow;
    }

    /// <summary>
    /// Get formatted salary range with currency
    /// </summary>
    public string GetFormattedSalary()
    {
        if (string.IsNullOrEmpty(SalaryRange))
            return "Not specified";

        if (!string.IsNullOrEmpty(Currency))
        {
            return $"{SalaryRange} {Currency}";
        }

        return SalaryRange;
    }

    /// <summary>
    /// Re-extract job information from the original URL
    /// </summary>
    public void MarkForReExtraction()
    {
        if (IsExtractedFromUrl())
        {
            Status = JobAnalysisStatus.Pending;
            ExtractedAt = null;
            // Keep original URL for re-extraction
        }
    }

    #endregion

    #region Private Methods

    private void SetTitle(string title)
    {
        Title = Check.NotNullOrWhiteSpace(title, nameof(title), 200);
    }

    private void SetCompany(string company)
    {
        Company = Check.NotNullOrWhiteSpace(company, nameof(company), 100);
    }

    private void SetDescription(string description)
    {
        Description = Check.NotNullOrWhiteSpace(description, nameof(description));
    }

    private void SetRequirements(string? requirements)
    {
        Requirements = requirements;
    }

    private void SetLocation(string? location)
    {
        Location = Check.Length(location, nameof(location), 100);
    }

    private void SetSalaryRange(string? salaryRange)
    {
        SalaryRange = Check.Length(salaryRange, nameof(salaryRange), 50);
    }

    #endregion
}