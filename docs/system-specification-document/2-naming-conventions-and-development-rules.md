# Naming Conventions & Development Rules

## 📏 Naming Conventions & Development Rules

### **ABP.io Standard Conventions (MUST Follow)**

| Component | Pattern | Example | Purpose |
|-----------|---------|---------|---------|
| **Entity** | `{BusinessConcept}` | `JobDescription`, `InterviewSession` | Domain objects with identity |
| **Domain Service** | `{BusinessConcept}DomainService` | `InterviewDomainService` | Complex business logic |
| **Application Service** | `{BusinessConcept}AppService` | `JDProcessorAppService` | Use case orchestration |
| **Controller** | `{BusinessConcept}Controller` | `InterviewEngineController` | API endpoint exposure |
| **Repository Interface** | `I{Entity}Repository` | `IJobDescriptionRepository` | Data access contract |
| **Repository Implementation** | `EfCore{Entity}Repository` | `EfCoreInterviewSessionRepository` | EF Core implementation |
| **DTO** | `{Action}{Entity}Dto` | `CreateJobDescriptionDto` | Data transfer object |
| **ABP Module** | `{ProjectName}Module` | `AbpResearchWorkerDomainModule` | ABP module registration |

### **Our Project-Specific Conventions (Team Standards)**

| Component | Pattern | Example | Rule |
|-----------|---------|---------|------|
| **AI Services** | `{AI}{Purpose}Service` | `AIPromptEngine`, `AIInterviewService` | Always prefix AI-related services |
| **Background Jobs** | `{Purpose}Job` | `JDAnalysisJob`, `CVGenerationJob` | Hangfire job naming |
| **Validators** | `{Dto}Validator` | `CreateJobDescriptionDtoValidator` | FluentValidation naming |
| **API Routes** | `/api/interview-prep/{module}/{action}` | `/api/interview-prep/jd/analyze` | Kebab-case URLs |
| **Configuration Options** | `{Service}Options` | `OpenAIOptions`, `WebRTCOptions` | Options pattern |
| **Custom Exceptions** | `{Context}Exception` | `InterviewEngineException` | Custom exception naming |
| **Event Handlers** | `{Event}Handler` | `InterviewCompletedEventHandler` | Domain event handlers |
| **Specifications** | `{Entity}{Purpose}Specification` | `JDValiditySpecification` | Business rule specs |
| **Prompt Engines** | `{Purpose}PromptEngine` | `InterviewPromptEngine` | AI prompt management |
| **Strategies** | `{Context}Strategy` | `TechnicalInterviewStrategy` | Strategy pattern implementation |

### **File and Folder Naming Rules**

#### **Folder Structure Rules:**
- **PascalCase**: All folder names use PascalCase (`Services`, `Controllers`, `Repositories`)
- **Descriptive Names**: Folders clearly describe their contents (`AIStrategies`, `PromptEngines`, `WebRTC`)
- **Pluralized**: Use plural forms for collections (`Entities`, `Services`, `Dtos`)
- **Categorized**: Group related files together (`AI/Models`, `Interview/Strategies`, `CV/Templates`)
- **Business-Focused**: Organize by business domain (`JobDescriptions/`, `Interviews/`, `CVs/`, `Chat/`)

#### **File Naming Rules:**
- **PascalCase**: All C# files use PascalCase (`JobDescription.cs`, `InterviewEngineAppService.cs`)
- **Descriptive**: File names clearly indicate their purpose and type
- **Consistent Extensions**: `.cs` for classes, `.json` for config files, `.ts` for TypeScript
- **No Abbreviations**: Use full names (`JobDescriptionAppService` not `JDAppSvc`)
- **Module Prefixes**: For shared components (`AbpResearchWorker{ComponentName}`)

### **Clean Architecture & Development Rules**

#### 🔴 **MUST FOLLOW (Breaking = Code Review Rejection)**

##### **Layer Dependency Rules:**
- ✅ **Controllers**: ONLY inject Application Services, never Domain Services or Repositories
- ✅ **Application Services**: Use `[Authorize]` and `[UnitOfWork]` attributes for data operations
- ✅ **Domain Logic**: Business rules ONLY in Entities and Domain Services
- ✅ **Repository Interfaces**: Must be in Domain layer, implementations in EntityFrameworkCore layer
- ✅ **Dependencies Flow**: Domain ← Application ← Infrastructure/EntityFrameworkCore ← HttpApi ← Host

##### **Method and Class Rules:**
- ✅ **Async Methods**: All methods must be async with `Async` suffix (`AnalyzeJobDescriptionAsync`)
- ✅ **Return Types**: Use `Task<T>` for all async operations, never `void`
- ✅ **Cancellation Tokens**: Accept `CancellationToken` for long-running operations (AI calls)
- ✅ **Interface Segregation**: Keep interfaces focused and specific to their purpose

##### **Error Handling Rules:**
- ✅ **ABP Exceptions**: Use `BusinessException`, `UserFriendlyException` for user-facing errors
- ✅ **Infrastructure Exceptions**: Use `AbpException` for technical errors
- ✅ **Validation**: Use ABP validation attributes or FluentValidation, never manual validation
- ✅ **Logging**: Structured logging with proper context (UserId, InterviewId, CorrelationId)

##### **Security Rules:**
- ✅ **Authorization**: Every Application Service method must have `[Authorize]` attribute
- ✅ **Input Sanitization**: Always validate and sanitize input, especially for AI processing
- ✅ **API Keys**: Never hardcode API keys, use configuration and Key Vault
- ✅ **Rate Limiting**: Implement rate limiting for external API calls (OpenAI, WebRTC)
- ✅ **Multi-tenancy**: Always consider tenant isolation in data access and caching

#### 🟡 **SHOULD FOLLOW (Discussed in PR Reviews)**

##### **Performance Guidelines:**
- ⚠️ **Caching Strategy**: Cache expensive operations (OpenAI API calls, company research results)
- ⚠️ **Database Queries**: Use projection and optimize EF Core queries
- ⚠️ **Background Processing**: Use Hangfire for time-consuming operations (AI analysis, CV generation)
- ⚠️ **Memory Management**: Dispose resources properly, avoid memory leaks in long-running operations

##### **Code Quality Guidelines:**
- ⚠️ **SOLID Principles**: Follow Single Responsibility, Open/Closed, Liskov Substitution, etc.
- ⚠️ **DRY Principle**: Extract common logic into reusable services
- ⚠️ **Clean Code**: Meaningful names, small methods (max 20 lines), clear intent
- ⚠️ **API Documentation**: XML summary comments for all controllers and public methods (Swagger integration)
- ⚠️ **Code Documentation**: XML comments for complex business logic and domain services

##### **Testing Guidelines:**
- ⚠️ **Test Coverage**: Aim for 80%+ coverage on Domain and Application layers
- ⚠️ **Unit Tests**: Domain Services and Application Services must have unit tests
- ⚠️ **Integration Tests**: API endpoints should have integration tests
- ⚠️ **Mock External Dependencies**: Mock OpenAI, WebRTC, and other external services

### 🎯 **ABP.io Specific Development Rules**

#### **Module Configuration:**
```csharp
// ✅ Correct: Module dependencies in proper order
[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(AbpResearchWorkerDomainSharedModule)
)]
public class AbpResearchWorkerDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Module configuration here
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });
    }
}
```

#### **Application Service Pattern:**
```csharp
// ✅ Correct: Proper ABP Application Service
[Authorize(AbpResearchWorkerPermissions.JobDescriptions.Default)]
public class JDProcessorAppService : ApplicationService, IJDProcessorAppService
{
    private readonly IJobDescriptionRepository _jobDescriptionRepository;
    private readonly IJDAnalysisDomainService _jdAnalysisService;

    public JDProcessorAppService(
        IJobDescriptionRepository jobDescriptionRepository,
        IJDAnalysisDomainService jdAnalysisService)
    {
        _jobDescriptionRepository = jobDescriptionRepository;
        _jdAnalysisService = jdAnalysisService;
    }

    [Authorize(AbpResearchWorkerPermissions.JobDescriptions.Create)]
    [UnitOfWork]
    public async Task<JobDescriptionDto> AnalyzeJobDescriptionAsync(CreateJobDescriptionDto input)
    {
        // Validation is automatic via ABP
        // Business logic delegated to Domain Service
        var jobDescription = await _jdAnalysisService.AnalyzeAsync(input.Content);
        await _jobDescriptionRepository.InsertAsync(jobDescription);
        
        // Response mapping via AutoMapper
        return ObjectMapper.Map<JobDescription, JobDescriptionDto>(jobDescription);
    }
}
```

#### **Repository Pattern:**
```csharp
// ✅ Correct: Repository interface in Domain
public interface IJobDescriptionRepository : IRepository<JobDescription, Guid>
{
    Task<JobDescription> FindByTitleAsync(string title);
    Task<List<JobDescription>> GetRecentAnalysesAsync(int count, CancellationToken cancellationToken = default);
    Task<List<JobDescription>> GetByCompanyAsync(Guid companyId, CancellationToken cancellationToken = default);
}

// ✅ Correct: Repository implementation in EntityFrameworkCore
public class EfCoreJobDescriptionRepository : EfCoreRepository<AbpResearchWorkerDbContext, JobDescription, Guid>, 
    IJobDescriptionRepository
{
    public EfCoreJobDescriptionRepository(IDbContextProvider<AbpResearchWorkerDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<JobDescription> FindByTitleAsync(string title)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet.FirstOrDefaultAsync(jd => jd.Title == title);
    }

    public async Task<List<JobDescription>> GetRecentAnalysesAsync(int count, CancellationToken cancellationToken = default)
    {
        var dbSet = await GetDbSetAsync();
        return await dbSet
            .OrderByDescending(jd => jd.CreationTime)
            .Take(count)
            .ToListAsync(cancellationToken);
    }
}
```

#### **Domain Event Pattern:**
```csharp
// ✅ Correct: Domain Event in Domain layer
public class InterviewCompletedEvent : DomainEvent
{
    public Guid InterviewSessionId { get; set; }
    public Guid UserId { get; set; }
    public int QuestionsAnswered { get; set; }
    public decimal OverallScore { get; set; }
    public int TokensUsed { get; set; }
    
    public InterviewCompletedEvent(Guid interviewSessionId, Guid userId, 
        int questionsAnswered, decimal overallScore, int tokensUsed)
    {
        InterviewSessionId = interviewSessionId;
        UserId = userId;
        QuestionsAnswered = questionsAnswered;
        OverallScore = overallScore;
        TokensUsed = tokensUsed;
    }
}

// ✅ Correct: Event Handler in Application layer
public class InterviewCompletedEventHandler : IDistributedEventHandler<InterviewCompletedEvent>
{
    private readonly IAnalyticsAppService _analyticsService;
    private readonly INotificationService _notificationService;

    public InterviewCompletedEventHandler(
        IAnalyticsAppService analyticsService,
        INotificationService notificationService)
    {
        _analyticsService = analyticsService;
        _notificationService = notificationService;
    }

    public async Task HandleEventAsync(InterviewCompletedEvent eventData)
    {
        // Update analytics
        await _analyticsService.UpdateUserProgressAsync(eventData.UserId, eventData.OverallScore);
        
        // Send completion notification
        await _notificationService.NotifyInterviewCompletedAsync(eventData.UserId, eventData.InterviewSessionId);
    }
}
```

#### **AI Service Integration Pattern:**
```csharp
// ✅ Correct: AI Service in Infrastructure layer
public class OpenAIService : IOpenAIService, ITransientDependency
{
    private readonly ILogger<OpenAIService> _logger;
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public OpenAIService(ILogger<OpenAIService> logger, 
        IConfiguration configuration, 
        HttpClient httpClient)
    {
        _logger = logger;
        _configuration = configuration;
        _httpClient = httpClient;
    }

    public async Task<string> GenerateResponseAsync(string prompt, CancellationToken cancellationToken = default)
    {
        try
        {
            // OpenAI API implementation with proper error handling and retry logic
            var request = new ChatRequest
            {
                Model = _configuration["OpenAI:Model"],
                Messages = new[] { new ChatMessage { Role = "user", Content = prompt } },
                MaxTokens = _configuration.GetValue<int>("OpenAI:MaxTokens")
            };

            var response = await _httpClient.PostAsJsonAsync("/v1/chat/completions", request, cancellationToken);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<ChatResponse>(cancellationToken: cancellationToken);
            return result?.Choices?.FirstOrDefault()?.Message?.Content ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate AI response for prompt: {Prompt}", prompt);
            throw new OpenAIServiceException("Failed to generate AI response", ex);
        }
    }
}
```

### **AI Interview System Specific Conventions**

#### **Interview Module Naming:**
```csharp
// ✅ Correct naming for interview components
public class InterviewSession : AuditedAggregateRoot<Guid>
public interface IInterviewEngineAppService
public class TechnicalInterviewStrategy : IInterviewStrategy
public class InterviewPromptEngine : IInterviewPromptEngine
public class StartInterviewDto
public class InterviewFeedbackDto
```

#### **Job Description Module Naming:**
```csharp
// ✅ Correct naming for JD processing components
public class JobDescription : AuditedAggregateRoot<Guid>
public interface IJDProcessorAppService
public class JDAnalysisDomainService : IJDAnalysisDomainService
public class CreateJobDescriptionDto
public class JobRequirementDto
public class CompanyInsightDto
```

#### **CV Generation Module Naming:**
```csharp
// ✅ Correct naming for CV building components
public class CVDocument : AuditedAggregateRoot<Guid>
public interface ICVBuilderAppService
public class CVGenerationDomainService : ICVGenerationDomainService
public class GenerateCVDto
public class CVSectionDto
public class CVTemplateDto
```

#### **Chat Assistant Module Naming:**
```csharp
// ✅ Correct naming for chat components
public class ChatSession : AuditedAggregateRoot<Guid>
public interface IChatAssistantAppService
public class ChatMessageDto
public class StartChatDto
public class ChatContextDto
```

### **Common Anti-Patterns to Avoid**

#### **❌ NEVER DO:**
- **Controllers calling Domain Services directly** (bypass Application layer)
- **Domain layer referencing Infrastructure** (dependency inversion violation)
- **Hardcoded strings for permissions** (use constants from AbpResearchWorkerPermissions class)
- **Manual transaction management** (let ABP UnitOfWork handle it)
- **Returning entities from Application Services** (always use DTOs)
- **Business logic in Controllers** (keep controllers thin)
- **Synchronous methods** (everything should be async)
- **Empty catch blocks** (always log exceptions properly)
- **Mixing business logic with UI logic in Angular components**
- **Direct OpenAI API calls from Application layer** (use Infrastructure services)

#### **⚠️ Code Review Red Flags:**
- Missing `[Authorize]` attributes on Application Services
- Using `Task.Result` or `.Wait()` (causes deadlocks)
- Direct database queries in Application layer (use repositories)
- Business logic scattered across multiple layers
- Missing validation on user inputs (especially AI prompts)
- Unclear variable or method names
- Large methods (>20 lines typically indicates need for refactoring)
- Hardcoded AI prompts (use PromptEngine services)
- Missing cancellation token support for long-running AI operations
- Not handling multi-tenancy in data access and caching

### **Angular Frontend Conventions**

#### **Component Naming:**
```typescript
// ✅ Correct Angular component naming
export class JobDescriptionComponent implements OnInit
export class InterviewSimulatorComponent implements OnInit
export class CvBuilderComponent implements OnInit
export class ChatAssistantComponent implements OnInit
export class AnalyticsDashboardComponent implements OnInit
```

#### **Service Naming:**
```typescript
// ✅ Correct Angular service naming
export class JobDescriptionService
export class InterviewEngineService
export class CVBuilderService
export class ChatAssistantService
export class AnalyticsService
```

#### **File Structure:**
```
src/app/
├── components/
│   ├── job-description/
│   │   ├── job-description.component.ts
│   │   ├── job-description.component.html
│   │   └── job-description.component.scss
│   ├── interview-simulator/
│   └── cv-builder/
├── services/
│   ├── job-description.service.ts
│   ├── interview-engine.service.ts
│   └── cv-builder.service.ts
└── models/
    ├── job-description.model.ts
    ├── interview-session.model.ts
    └── cv-document.model.ts
```

This comprehensive set of naming conventions and development rules ensures consistency, maintainability, and adherence to ABP.io best practices across the entire AI Interview Preparation System! 📏