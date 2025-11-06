using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities.Auditing;

namespace AbpResearchWorker.JobDescriptions;

/// <summary>
/// InterviewQuestion entity representing an AI-generated interview question
/// </summary>
public class InterviewQuestion : FullAuditedEntity<Guid>
{
    #region Basic Question Information

    /// <summary>
    /// Reference to the job description this question belongs to
    /// </summary>
    public Guid JobDescriptionId { get; private set; }

    /// <summary>
    /// The actual interview question text
    /// </summary>
    [Required]
    public string QuestionText { get; private set; } = string.Empty;

    /// <summary>
    /// Category of this question
    /// </summary>
    public QuestionCategory Category { get; private set; }

    /// <summary>
    /// Difficulty level of this question
    /// </summary>
    public QuestionDifficulty Difficulty { get; private set; }

    /// <summary>
    /// Order/sequence number for displaying questions
    /// </summary>
    public int Order { get; private set; }

    #endregion

    #region AI-Generated Content

    /// <summary>
    /// Expected answer points or key topics (JSON array)
    /// </summary>
    public string? ExpectedAnswerPoints { get; private set; }

    /// <summary>
    /// Sample good answer provided by AI
    /// </summary>
    public string? SampleAnswer { get; private set; }

    /// <summary>
    /// Follow-up questions that interviewer can ask
    /// </summary>
    public string? FollowUpQuestions { get; private set; }

    /// <summary>
    /// Interviewer tips and guidance
    /// </summary>
    public string? InterviewerTips { get; private set; }

    /// <summary>
    /// Skills this question evaluates (JSON array)
    /// </summary>
    public string? SkillsEvaluated { get; private set; }

    #endregion

    #region AI Usage Tracking

    /// <summary>
    /// Number of tokens used to generate this question
    /// </summary>
    public int TokensUsed { get; private set; }

    /// <summary>
    /// Cost to generate this question in USD
    /// </summary>
    public decimal GenerationCost { get; private set; }

    /// <summary>
    /// OpenAI model used for generation
    /// </summary>
    [StringLength(50)]
    public string? ModelUsed { get; private set; }

    /// <summary>
    /// When this question was generated
    /// </summary>
    public DateTime GeneratedAt { get; private set; }

    #endregion

    #region Quality Metrics

    /// <summary>
    /// Quality score of the question (0-100)
    /// </summary>
    public int QualityScore { get; private set; }

    /// <summary>
    /// Whether this question has been reviewed/approved
    /// </summary>
    public bool IsApproved { get; private set; }

    /// <summary>
    /// Number of times this question was used in interviews
    /// </summary>
    public int UsageCount { get; private set; }

    /// <summary>
    /// Average effectiveness rating from users
    /// </summary>
    public decimal EffectivenessRating { get; private set; }

    #endregion

    #region Relationships

    /// <summary>
    /// Navigation property to the job description
    /// </summary>
    public virtual JobDescription JobDescription { get; private set; } = null!;

    #endregion

    #region Constructors

    protected InterviewQuestion()
    {
        // For EF Core
    }

    public InterviewQuestion(
        Guid id,
        Guid jobDescriptionId,
        string questionText,
        QuestionCategory category,
        QuestionDifficulty difficulty,
        int order = 0
    ) : base(id)
    {
        JobDescriptionId = jobDescriptionId;
        SetQuestionText(questionText);
        Category = category;
        Difficulty = difficulty;
        Order = order;
        
        TokensUsed = 0;
        GenerationCost = 0;
        GeneratedAt = DateTime.UtcNow;
        QualityScore = 0;
        IsApproved = false;
        UsageCount = 0;
        EffectivenessRating = 0;
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Update the question text
    /// </summary>
    public void UpdateQuestion(string questionText)
    {
        SetQuestionText(questionText);
        
        // Reset approval when question changes
        IsApproved = false;
        QualityScore = 0;
    }

    /// <summary>
    /// Set AI-generated content for this question
    /// </summary>
    public void SetAIContent(
        string? expectedAnswerPoints = null,
        string? sampleAnswer = null,
        string? followUpQuestions = null,
        string? interviewerTips = null,
        string? skillsEvaluated = null,
        int tokensUsed = 0,
        decimal generationCost = 0,
        string? modelUsed = null)
    {
        ExpectedAnswerPoints = expectedAnswerPoints;
        SampleAnswer = sampleAnswer;
        FollowUpQuestions = followUpQuestions;
        InterviewerTips = interviewerTips;
        SkillsEvaluated = skillsEvaluated;
        TokensUsed = tokensUsed;
        GenerationCost = generationCost;
        ModelUsed = modelUsed;
        GeneratedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Update question category and difficulty
    /// </summary>
    public void UpdateClassification(QuestionCategory category, QuestionDifficulty difficulty)
    {
        Category = category;
        Difficulty = difficulty;
    }

    /// <summary>
    /// Set the display order
    /// </summary>
    public void SetOrder(int order)
    {
        Order = Math.Max(0, order);
    }

    /// <summary>
    /// Approve the question for use
    /// </summary>
    public void Approve(int qualityScore = 100)
    {
        IsApproved = true;
        QualityScore = Math.Max(0, Math.Min(100, qualityScore));
    }

    /// <summary>
    /// Reject/disapprove the question
    /// </summary>
    public void Disapprove(string? reason = null)
    {
        IsApproved = false;
        QualityScore = 0;
        // Could add rejection reason if needed
    }

    /// <summary>
    /// Record usage of this question in an interview
    /// </summary>
    public void RecordUsage()
    {
        UsageCount++;
    }

    /// <summary>
    /// Update effectiveness rating based on user feedback
    /// </summary>
    public void UpdateEffectiveness(decimal rating)
    {
        // Simple average for now, could be more sophisticated
        if (UsageCount > 0)
        {
            EffectivenessRating = ((EffectivenessRating * (UsageCount - 1)) + rating) / UsageCount;
        }
        else
        {
            EffectivenessRating = rating;
        }
    }

    /// <summary>
    /// Check if question is suitable for the given experience level
    /// </summary>
    public bool IsSuitableForExperience(ExperienceLevel experienceLevel)
    {
        return Difficulty switch
        {
            QuestionDifficulty.Easy => experienceLevel <= ExperienceLevel.Junior,
            QuestionDifficulty.Medium => experienceLevel >= ExperienceLevel.Junior && experienceLevel <= ExperienceLevel.Senior,
            QuestionDifficulty.Hard => experienceLevel >= ExperienceLevel.Senior,
            QuestionDifficulty.Expert => experienceLevel >= ExperienceLevel.Lead,
            _ => true
        };
    }

    /// <summary>
    /// Get display text for the question category
    /// </summary>
    public string GetCategoryDisplayName()
    {
        return Category switch
        {
            QuestionCategory.Technical => "Technical",
            QuestionCategory.Behavioral => "Behavioral",
            QuestionCategory.Situational => "Situational",
            QuestionCategory.Cultural => "Cultural Fit",
            QuestionCategory.Industry => "Industry Knowledge",
            QuestionCategory.Leadership => "Leadership",
            QuestionCategory.CaseStudy => "Case Study",
            _ => "General"
        };
    }

    /// <summary>
    /// Get display text for the question difficulty
    /// </summary>
    public string GetDifficultyDisplayName()
    {
        return Difficulty switch
        {
            QuestionDifficulty.Easy => "Entry Level",
            QuestionDifficulty.Medium => "Intermediate",
            QuestionDifficulty.Hard => "Advanced",
            QuestionDifficulty.Expert => "Expert",
            _ => "Unknown"
        };
    }

    #endregion

    #region Private Methods

    private void SetQuestionText(string questionText)
    {
        QuestionText = Check.NotNullOrWhiteSpace(questionText, nameof(questionText));
    }

    #endregion
}