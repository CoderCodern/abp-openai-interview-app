using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AbpResearchWorker.JobDescriptions;

/// <summary>
/// Configuration for job site scraping rules
/// </summary>
public class JobScrapingConfiguration
{
    /// <summary>
    /// Domain pattern to match (e.g., "topcv.vn", "*.vietnamworks.com")
    /// </summary>
    [Required]
    [StringLength(100)]
    public string DomainPattern { get; set; } = string.Empty;

    /// <summary>
    /// Display name for the job board
    /// </summary>
    [Required]
    [StringLength(100)]
    public string JobBoardName { get; set; } = string.Empty;

    /// <summary>
    /// Country/region code (VN, US, SG, etc.)
    /// </summary>
    [StringLength(10)]
    public string? CountryCode { get; set; }

    /// <summary>
    /// Primary language of the site (vi, en, zh, etc.)
    /// </summary>
    [StringLength(10)]
    public string? Language { get; set; }

    /// <summary>
    /// Default currency for salary parsing (VND, USD, SGD, etc.)
    /// </summary>
    [StringLength(10)]
    public string? DefaultCurrency { get; set; }

    /// <summary>
    /// CSS selectors for extracting job information
    /// </summary>
    public JobScrapingSelectors Selectors { get; set; } = new();

    /// <summary>
    /// Additional extraction rules and transformations
    /// </summary>
    public JobScrapingRules Rules { get; set; } = new();

    /// <summary>
    /// Whether this configuration is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Priority order (higher numbers processed first)
    /// </summary>
    public int Priority { get; set; } = 0;
}

/// <summary>
/// CSS selectors for extracting job data
/// </summary>
public class JobScrapingSelectors
{
    /// <summary>
    /// Job title selector
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    /// Company name selector
    /// </summary>
    public string? Company { get; set; }

    /// <summary>
    /// Job description/content selector
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Location selector
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Salary selector
    /// </summary>
    public string? Salary { get; set; }

    /// <summary>
    /// Requirements/qualifications selector
    /// </summary>
    public string? Requirements { get; set; }

    /// <summary>
    /// Job type (full-time, part-time, etc.)
    /// </summary>
    public string? JobType { get; set; }

    /// <summary>
    /// Experience level selector
    /// </summary>
    public string? ExperienceLevel { get; set; }

    /// <summary>
    /// Posted date selector
    /// </summary>
    public string? PostedDate { get; set; }

    /// <summary>
    /// Application deadline selector
    /// </summary>
    public string? Deadline { get; set; }

    /// <summary>
    /// Company logo selector
    /// </summary>
    public string? CompanyLogo { get; set; }
}

/// <summary>
/// Additional extraction and transformation rules
/// </summary>
public class JobScrapingRules
{
    /// <summary>
    /// Text patterns to remove from description
    /// </summary>
    public List<string> RemovePatterns { get; set; } = new();

    /// <summary>
    /// Text patterns to identify requirements section
    /// </summary>
    public List<string> RequirementKeywords { get; set; } = new();

    /// <summary>
    /// Salary parsing patterns for this region
    /// </summary>
    public List<string> SalaryPatterns { get; set; } = new();

    /// <summary>
    /// Date format patterns (dd/MM/yyyy, MM-dd-yyyy, etc.)
    /// </summary>
    public List<string> DateFormats { get; set; } = new();

    /// <summary>
    /// Experience level mapping (Junior -> Entry, etc.)
    /// </summary>
    public Dictionary<string, string> ExperienceLevelMapping { get; set; } = new();

    /// <summary>
    /// Location normalization rules
    /// </summary>
    public Dictionary<string, string> LocationMapping { get; set; } = new();

    /// <summary>
    /// Custom JavaScript to execute for complex extraction
    /// </summary>
    public string? CustomJavaScript { get; set; }

    /// <summary>
    /// Headers to send with requests
    /// </summary>
    public Dictionary<string, string> RequestHeaders { get; set; } = new();

    /// <summary>
    /// Whether to use browser automation for this site
    /// </summary>
    public bool RequiresBrowser { get; set; } = false;

    /// <summary>
    /// Delay between requests (milliseconds)
    /// </summary>
    public int RequestDelay { get; set; } = 1000;
}