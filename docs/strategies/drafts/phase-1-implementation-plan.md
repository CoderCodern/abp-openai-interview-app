# Phase 1: Job Description Analysis & Question Generation

## 🎯 **Phase Overview**

**Goal:** Build the core functionality for analyzing job descriptions and generating AI-powered interview questions.

**Duration:** 6-7 hours for complete working MVP  
**Priority:** High - Foundation for entire interview system  
**Dependencies:** Domain entities (JobDescription, InterviewQuestion) already created

---

## 📋 **Current Status**

✅ **Completed:**
- JobDescription & InterviewQuestion domain entities created
- Book entities completely removed from codebase
- Clean foundation established with proper ABP patterns
- Database migration applied to remove legacy data

🔄 **In Progress:**
- Domain foundation setup (entities not yet in DbContext)

⏳ **Pending:**
- OpenAI integration service
- Application services layer
- API controllers
- Angular frontend components

---

## 🏗️ **Detailed Implementation Plan**

### **Step 1: Complete Domain Foundation** 
**Time:** 30-45 minutes | **Risk:** Low

**Tasks:**
1. Add JobDescription & InterviewQuestion to AbpResearchWorkerDbContext
2. Create EF Core entity configurations with proper relationships
3. Generate and apply database migrations
4. Add custom repository methods if needed

**Deliverables:**
- Updated DbContext with new entities
- EF Core configurations for relationships and indexes
- Database schema created via migrations

---

### **Step 2: Build OpenAI Integration Service**
**Time:** 1-2 hours | **Risk:** Medium

**Components:**
```csharp
// Configuration
public class OpenAIOptions
{
    public string ApiKey { get; set; }
    public string Model { get; set; } = "gpt-3.5-turbo";
    public int MaxTokens { get; set; } = 4000;
    public decimal CostPerToken { get; set; } = 0.000002m;
}

// Service Interface
public interface IOpenAIService
{
    Task<JobAnalysisResult> AnalyzeJobDescriptionAsync(string jobDescription);
    Task<List<InterviewQuestionResult>> GenerateQuestionsAsync(string jobDescription, JobAnalysisResult analysis);
}
```

**Key Features:**
- Smart prompt engineering for job analysis
- Token counting and cost calculation
- Retry mechanism for transient failures
- Support for different OpenAI models (cost optimization)

**Prompt Engineering Strategy:**
```
Job Analysis Prompt:
"Analyze this job description and extract: company name, key skills, experience level, domain/industry. Return structured JSON."

Question Generation Prompt:
"Generate {count} interview questions for {role} at {company}. Include technical, behavioral, and situational questions. Format as JSON with categories and difficulty."
```

---

### **Step 3: Application Services Layer**
**Time:** 1-1.5 hours | **Risk:** Low-Medium

**Core Service:**
```csharp
public class JobAnalysisAppService : ApplicationService
{
    Task<JobAnalysisResultDto> AnalyzeJobDescriptionAsync(AnalyzeJobDescriptionDto input);
    Task<List<InterviewQuestionDto>> GenerateQuestionsAsync(Guid jobDescriptionId);
    Task<JobAnalysisResultDto> GetAnalysisAsync(Guid jobDescriptionId);
    Task RegenerateQuestionsAsync(Guid jobDescriptionId);
}
```

**Business Logic:**
- Parse and validate job description input
- Extract company name and key requirements
- Determine appropriate question count based on role level
- Apply cost budgeting and rate limiting
- Cache results to avoid duplicate processing

**DTOs:**
- `AnalyzeJobDescriptionDto` - Input with job description text
- `JobAnalysisResultDto` - Analysis results with questions
- `InterviewQuestionDto` - Individual question with metadata

---

### **Step 4: API Controllers**
**Time:** 45 minutes | **Risk:** Low

**RESTful Endpoints:**
```
POST /api/app/job-analysis/analyze
  Body: { jobTitle, company, description, requirements }
  Response: JobAnalysisResultDto with questions

GET /api/app/job-analysis/{id}
  Response: JobAnalysisResultDto

GET /api/app/job-analysis/{id}/questions
  Response: List<InterviewQuestionDto>

POST /api/app/job-analysis/{id}/regenerate-questions
  Response: List<InterviewQuestionDto>
```

**Features:**
- Swagger/OpenAPI documentation
- Input validation with detailed error responses
- ABP authorization integration
- Proper HTTP status codes and error handling

---

### **Step 5: Angular Frontend (MVP)**
**Time:** 1-1.5 hours | **Risk:** Medium

**Components Structure:**
```
src/app/job-analysis/
├── job-input/
│   ├── job-input.component.ts
│   ├── job-input.component.html
│   └── job-input.component.scss
├── analysis-results/
│   ├── analysis-results.component.ts
│   ├── analysis-results.component.html
│   └── analysis-results.component.scss
├── question-list/
│   ├── question-list.component.ts
│   ├── question-list.component.html
│   └── question-list.component.scss
└── services/
    └── job-analysis.service.ts
```

**User Flow:**
1. **Job Input Page:** Large textarea for job description + company field
2. **Analysis Loading:** Progress indicator while AI processes
3. **Results Display:** Show extracted info + generated questions
4. **Question Management:** Category tabs, regenerate options

**UI Features:**
- Responsive design for mobile/desktop
- Real-time validation and character counts
- Loading states with progress indicators
- Error handling with user-friendly messages

---

### **Step 6: Performance & Caching**
**Time:** 1 hour | **Risk:** Low-Medium

**Caching Strategy:**
```csharp
// Redis Cache Keys
public static class CacheKeys
{
    public static string JobAnalysis(Guid jobId) => $"job:analysis:{jobId}";
    public static string Questions(Guid jobId) => $"job:questions:{jobId}";
    public static string CompanyInfo(string company) => $"company:{company}";
}
```

**Optimization Features:**
- Cache job analysis results (24 hours TTL)
- Cache generated questions (12 hours TTL)
- Token counting before OpenAI API calls
- Daily budget limits per user/tenant
- Smart prompt optimization to reduce costs

**Cost Control:**
- Track OpenAI usage per user/day
- Implement rate limiting (e.g., 10 analyses per hour)
- Alert when approaching budget limits
- Graceful degradation when limits exceeded

---

### **Step 7: Testing & Validation**
**Time:** 1-1.5 hours | **Risk:** Low

**Testing Strategy:**
```csharp
// Unit Tests
JobDescriptionTests.cs - Domain entity validation
OpenAIServiceTests.cs - Mock OpenAI responses
JobAnalysisAppServiceTests.cs - Business logic validation

// Integration Tests
JobAnalysisControllerTests.cs - API endpoint testing
JobAnalysisE2ETests.cs - Complete workflow testing
```

**Test Cases:**
- Various job description formats and lengths
- Different industries and role levels
- Edge cases (very short/long descriptions)
- Error scenarios (API failures, rate limits)
- Performance testing with concurrent requests

**Manual Testing Checklist:**
- [ ] Real job descriptions from Indeed/LinkedIn
- [ ] Different company sizes and industries
- [ ] Question quality and relevance
- [ ] UI responsiveness on different devices
- [ ] Error handling and recovery

---

## 🎯 **Success Criteria**

### **Functional Requirements:**
✅ User can paste any job description and receive:
- Extracted company information and key requirements
- 10-15 relevant interview questions categorized by type
- Questions with difficulty levels and interviewer tips

### **Technical Requirements:**
✅ System demonstrates:
- OpenAI cost tracking under $5/day during development
- Response times under 10 seconds for job analysis
- Proper caching to avoid duplicate API calls
- Graceful error handling for all failure scenarios
- Mobile-responsive UI with good UX

### **Quality Gates:**
✅ All automated tests passing (90%+ coverage)
✅ Manual testing with 10+ different job descriptions
✅ Performance testing with 20+ concurrent users
✅ Security review of API endpoints and data handling

---

## 🔧 **Technical Decisions Made**

1. **OpenAI Model:** Start with GPT-3.5-turbo for cost efficiency
2. **Caching:** Redis for distributed caching across instances  
3. **Frontend State:** Angular services with RxJS for reactive updates
4. **Validation:** FluentValidation on backend + Angular reactive forms
5. **Error Handling:** Structured error responses with user-friendly messages

---

## 📊 **Metrics to Track**

### **Business Metrics:**
- Job descriptions analyzed per day
- Questions generated per analysis
- User satisfaction with question quality
- Time spent by users in the application

### **Technical Metrics:**
- OpenAI API response times and costs
- Cache hit rates for analysis results
- API endpoint performance and error rates
- Database query performance

### **Cost Metrics:**
- OpenAI tokens consumed per analysis
- Daily/monthly AI service costs
- Cost per user per analysis
- ROI based on user engagement

---

## 🚀 **Next Phase Preview**

**Phase 2: Interactive Interview Sessions**
- Real-time interview simulation with WebRTC
- AI-powered answer evaluation and feedback
- Session recording and playback
- Performance analytics and improvement suggestions

**Phase 3: Advanced Features**
- CV generation based on job requirements
- Company research integration
- Interview scheduling and calendar integration
- Multi-language support for global job markets

---

**Created:** November 5, 2025  
**Status:** Ready for Implementation  
**Owner:** Development Team  
**Estimated Completion:** 1-2 weeks for MVP