# Job Description System - Entity Fields & Logic Review

**Date:** November 5, 2025  
**Status:** Entities Added to DbContext - Ready for Migration  

---

## 📋 **Entity Overview**

### **1. JobDescriptionEnums.cs** 
*Location: `Domain.Shared/JobDescriptions/`*

**Business Enumerations:**
- `JobAnalysisStatus`: Pending → InProgress → Completed/Failed/Expired
- `JobComplexity`: Entry → Intermediate → Advanced → Expert → Executive  
- `QuestionCategory`: Technical, Behavioral, Situational, Cultural, Industry, Leadership, CaseStudy
- `QuestionDifficulty`: Easy → Medium → Hard → Expert
- `ExperienceLevel`: EntryLevel → Junior → MidLevel → Senior → Lead → Executive
- `CompanySize`: Startup → Small → Medium → Large → Enterprise → Unknown

---

## 📊 **JobDescription Entity**
*Location: `Domain/JobDescriptions/JobDescription.cs`*  
*Table: `AppJobDescriptions`*

### **Core Job Information**
```csharp
// Basic job posting data
string Title               // Job title (Required, Max 200 chars)
string Company            // Company name (Required, Max 100 chars)  
string Description        // Full job description (Required, Unlimited)
string? Requirements      // Extracted requirements (Optional)
string? Location          // Job location (Optional, Max 100 chars)
string? SalaryRange       // Salary info (Optional, Max 50 chars)
```

### **AI Analysis Results**
```csharp
// Analysis workflow tracking
JobAnalysisStatus Status           // Pending/InProgress/Completed/Failed/Expired
string? KeySkills                  // JSON array of extracted skills
ExperienceLevel ExperienceLevel    // Entry to Executive level
JobComplexity Complexity           // Entry to Expert complexity
string? Industry                   // Industry classification (Max 50 chars)
CompanySize CompanySize            // Startup to Enterprise
string? CompanyInsights            // AI-generated company data (JSON)
DateTime? AnalyzedAt               // When analysis completed
string? ErrorMessage               // Failure reason (Max 500 chars)
```

### **AI Cost Tracking**
```csharp
// OpenAI usage monitoring
int TokensUsed                     // Number of tokens consumed
decimal AnalysisCost               // Cost in USD (18,4 precision)
string? ModelUsed                  // OpenAI model name (Max 50 chars)
```

### **Business Logic Methods**
```csharp
// Core operations
void UpdateJobInfo(title, company, description, ...)      // Update job details
void StartAnalysis()                                       // Mark analysis started
void CompleteAnalysis(keySkills, experienceLevel, ...)    // Set analysis results  
void FailAnalysis(errorMessage)                           // Mark analysis failed
void ExpireAnalysis()                                      // Mark for refresh
void ResetAnalysis()                                       // Clear all analysis data

// Question management
void AddQuestion(InterviewQuestion question)              // Associate question
decimal GetTotalCost()                                     // Total cost including questions
int GetTotalTokens()                                       // Total tokens including questions
bool IsReadyForQuestions()                                 // Check if analysis ready
```

### **Relationships**
```csharp
// Navigation properties
ICollection<InterviewQuestion> Questions                   // Associated questions (Cascade delete)
```

### **Database Indexes**
- `Status` (Query by analysis status)
- `Company` (Search by company)
- `CreationTime` (Order by creation)
- `Status + CreationTime` (Composite for pagination)

---

## 🤔 **InterviewQuestion Entity**
*Location: `Domain/JobDescriptions/InterviewQuestion.cs`*  
*Table: `AppInterviewQuestions`*

### **Core Question Data**
```csharp
// Question basics
Guid JobDescriptionId             // FK to JobDescription (Required)
string QuestionText               // Actual question (Required)
QuestionCategory Category         // Technical/Behavioral/etc
QuestionDifficulty Difficulty     // Easy/Medium/Hard/Expert
int Order                         // Display sequence
```

### **AI-Generated Content**
```csharp
// Rich question content
string? ExpectedAnswerPoints      // JSON array of key topics
string? SampleAnswer              // AI-provided good answer
string? FollowUpQuestions         // Additional questions to ask
string? InterviewerTips           // Guidance for interviewer
string? SkillsEvaluated          // JSON array of skills tested
```

### **AI Usage Tracking**
```csharp
// Generation metrics
int TokensUsed                    // Tokens for this question
decimal GenerationCost            // Cost for this question (18,4 precision)
string? ModelUsed                 // OpenAI model used (Max 50 chars)
DateTime GeneratedAt              // When question was created
```

### **Quality Metrics**
```csharp
// Question effectiveness
int QualityScore                  // Score 0-100
bool IsApproved                   // Manual approval flag
int UsageCount                    // Times used in interviews
decimal EffectivenessRating       // User feedback (18,2 precision)
```

### **Business Logic Methods**
```csharp
// Question management
void UpdateQuestion(questionText)                         // Change question text
void SetAIContent(expectedPoints, sampleAnswer, ...)      // Set AI-generated content
void UpdateClassification(category, difficulty)           // Change categorization
void SetOrder(order)                                       // Set display order

// Quality control
void Approve(qualityScore)                                // Approve for use
void Disapprove(reason)                                   // Reject question
void RecordUsage()                                        // Track usage
void UpdateEffectiveness(rating)                          // Update rating

// Utility methods
bool IsSuitableForExperience(experienceLevel)            // Check level match
string GetCategoryDisplayName()                           // UI-friendly category
string GetDifficultyDisplayName()                         // UI-friendly difficulty
```

### **Relationships**
```csharp
// Navigation properties
JobDescription JobDescription                             // Parent job (Required FK)
```

### **Database Indexes**
- `JobDescriptionId` (FK queries)
- `Category` (Filter by type)
- `Difficulty` (Filter by level)
- `IsApproved` (Show approved only)
- `JobDescriptionId + Order` (Ordered display)
- `Category + Difficulty` (Filtered lists)

---

## 🏗️ **Database Configuration**

### **Entity Framework Setup**
```csharp
// DbContext configuration in AbpResearchWorkerDbContext
public DbSet<JobDescription> JobDescriptions { get; set; }
public DbSet<InterviewQuestion> InterviewQuestions { get; set; }

// Table configuration
JobDescriptions → AppJobDescriptions (with proper constraints)
InterviewQuestions → AppInterviewQuestions (with FK cascade)

// Decimal precision
AnalysisCost: decimal(18,4)     // Supports $9,999,999,999.9999
GenerationCost: decimal(18,4)   // Same precision for consistency
EffectivenessRating: decimal(18,2)  // Up to 99999999999999.99
```

### **Audit Trail (Inherited from ABP)**
```csharp
// FullAuditedAggregateRoot provides:
Guid Id                          // Primary key
DateTime CreationTime            // When created
Guid? CreatorId                  // Who created
DateTime? LastModificationTime   // When updated  
Guid? LastModifierId            // Who updated
DateTime? DeletionTime          // When soft deleted
Guid? DeleterId                 // Who deleted
bool IsDeleted                  // Soft delete flag

// Multi-tenancy support
Guid? TenantId                  // Tenant isolation
```

---

## ✅ **Build Verification**

**Status:** ✅ **BUILD SUCCESSFUL**
- All entities compile without errors
- Entity configurations validated
- Database relationships properly configured
- ABP patterns correctly implemented
- Ready for EF Core migration generation

**Warnings:** 2 unrelated warnings in OpenIddict configuration (pre-existing)

---

## 🚀 **Next Steps**

1. **✅ Completed:** Entity creation and DbContext configuration
2. **🔄 Current:** Ready to generate EF Core migrations
3. **⏳ Pending:** Create migration for database schema
4. **⏳ Pending:** Apply migration to database
5. **⏳ Pending:** Build OpenAI integration service

**Ready for Migration Generation!** 🎯