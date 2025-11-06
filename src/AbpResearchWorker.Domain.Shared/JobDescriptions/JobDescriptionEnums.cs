namespace AbpResearchWorker.JobDescriptions;

/// <summary>
/// Status of job description analysis process
/// </summary>
public enum JobAnalysisStatus
{
    /// <summary>
    /// Job description submitted but not yet processed
    /// </summary>
    Pending = 0,
    
    /// <summary>
    /// AI is currently analyzing the job description
    /// </summary>
    InProgress = 1,
    
    /// <summary>
    /// Analysis completed successfully with results
    /// </summary>
    Completed = 2,
    
    /// <summary>
    /// Analysis failed due to error (invalid content, API failure, etc.)
    /// </summary>
    Failed = 3,
    
    /// <summary>
    /// Analysis expired and needs to be refreshed
    /// </summary>
    Expired = 4
}

/// <summary>
/// Complexity level of the job position
/// </summary>
public enum JobComplexity
{
    /// <summary>
    /// Entry-level position requiring basic skills
    /// </summary>
    Entry = 0,
    
    /// <summary>
    /// Mid-level position requiring some experience
    /// </summary>
    Intermediate = 1,
    
    /// <summary>
    /// Senior position requiring advanced skills
    /// </summary>
    Advanced = 2,
    
    /// <summary>
    /// Expert/Lead position requiring extensive experience
    /// </summary>
    Expert = 3,
    
    /// <summary>
    /// Executive/C-level position requiring leadership experience
    /// </summary>
    Executive = 4
}

/// <summary>
/// Category of interview question
/// </summary>
public enum QuestionCategory
{
    /// <summary>
    /// Technical skills and knowledge questions
    /// </summary>
    Technical = 0,
    
    /// <summary>
    /// Behavioral and soft skills questions
    /// </summary>
    Behavioral = 1,
    
    /// <summary>
    /// Situational and problem-solving questions
    /// </summary>
    Situational = 2,
    
    /// <summary>
    /// Company culture and fit questions
    /// </summary>
    Cultural = 3,
    
    /// <summary>
    /// Industry-specific knowledge questions
    /// </summary>
    Industry = 4,
    
    /// <summary>
    /// Leadership and management questions (for senior roles)
    /// </summary>
    Leadership = 5,
    
    /// <summary>
    /// Case study and analytical questions
    /// </summary>
    CaseStudy = 6
}

/// <summary>
/// Difficulty level of interview question
/// </summary>
public enum QuestionDifficulty
{
    /// <summary>
    /// Basic level suitable for entry positions
    /// </summary>
    Easy = 0,
    
    /// <summary>
    /// Intermediate level for mid-level positions
    /// </summary>
    Medium = 1,
    
    /// <summary>
    /// Advanced level for senior positions
    /// </summary>
    Hard = 2,
    
    /// <summary>
    /// Expert level for lead/principal positions
    /// </summary>
    Expert = 3
}

/// <summary>
/// Experience level extracted from job description
/// </summary>
public enum ExperienceLevel
{
    /// <summary>
    /// No experience required (0-1 years)
    /// </summary>
    EntryLevel = 0,
    
    /// <summary>
    /// Some experience required (1-3 years)
    /// </summary>
    Junior = 1,
    
    /// <summary>
    /// Mid-level experience (3-5 years)
    /// </summary>
    MidLevel = 2,
    
    /// <summary>
    /// Senior experience (5-8 years)
    /// </summary>
    Senior = 3,
    
    /// <summary>
    /// Lead/Principal level (8+ years)
    /// </summary>
    Lead = 4,
    
    /// <summary>
    /// Executive/Director level (10+ years)
    /// </summary>
    Executive = 5
}

/// <summary>
/// Company size categories for better context
/// </summary>
public enum CompanySize
{
    /// <summary>
    /// Startup (1-10 employees)
    /// </summary>
    Startup = 0,
    
    /// <summary>
    /// Small business (11-50 employees)
    /// </summary>
    Small = 1,
    
    /// <summary>
    /// Medium business (51-200 employees)
    /// </summary>
    Medium = 2,
    
    /// <summary>
    /// Large business (201-1000 employees)
    /// </summary>
    Large = 3,
    
    /// <summary>
    /// Enterprise (1000+ employees)
    /// </summary>
    Enterprise = 4,
    
    /// <summary>
    /// Unknown or not specified
    /// </summary>
    Unknown = 5
}

/// <summary>
/// How the job description was obtained
/// </summary>
public enum JobDescriptionSource
{
    /// <summary>
    /// User pasted job description text manually
    /// </summary>
    ManualText = 0,
    
    /// <summary>
    /// Extracted from URL via web scraping
    /// </summary>
    UrlScraping = 1,
    
    /// <summary>
    /// Retrieved via job board API (LinkedIn API, Indeed API, etc.)
    /// </summary>
    JobBoardApi = 2,
    
    /// <summary>
    /// Imported from file upload (PDF, Word, etc.)
    /// </summary>
    FileUpload = 3,
    
    /// <summary>
    /// Extracted via browser automation (Selenium/Playwright)
    /// </summary>
    BrowserAutomation = 4,
    
    /// <summary>
    /// Fetched via RSS/XML feed
    /// </summary>
    RssFeed = 5
}