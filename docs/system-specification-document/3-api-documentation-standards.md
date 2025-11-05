# API Documentation Standards

## **Requirements**

### **🔴 MUST HAVE:**
- XML `<summary>` for every controller method and DTO
- `<param>` and `<returns>` documentation
- `<response>` tags for all HTTP status codes  
- `<example>` values for DTOs
- `[ProducesResponseType]` attributes
- `[Authorize]` attribute documentation with specific permissions

### **🟡 RECOMMENDED:**
- `[Tags]` for endpoint grouping by business module
- `[SwaggerOperation]` for complex AI operations
- Realistic example values for interview data
- Rate limiting documentation for AI endpoints
- Multi-tenancy considerations in examples

## **Controller Documentation Patterns**

### **Job Description Processing Controller**

```csharp
/// <summary>
/// Analyzes job descriptions and extracts key requirements using AI
/// </summary>
/// <param name="input">Job description content and metadata for AI analysis</param>
/// <returns>Parsed job requirements with company insights and skill analysis</returns>
/// <response code="200">Job description successfully analyzed</response>
/// <response code="400">Invalid job description content</response>
/// <response code="401">Unauthorized access</response>
/// <response code="429">AI API rate limit exceeded</response>
[HttpPost("analyze")]
[Authorize(AbpResearchWorkerPermissions.JobDescriptions.Create)]
[ProducesResponseType(typeof(JobDescriptionDto), 200)]
[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
[ProducesResponseType(typeof(UnauthorizedResult), 401)]
[ProducesResponseType(typeof(ErrorResponse), 429)]
public async Task<JobDescriptionDto> AnalyzeJobDescriptionAsync([FromBody] CreateJobDescriptionDto input)
{
    return await _jdProcessorService.AnalyzeJobDescriptionAsync(input);
}
```

### **Interview Engine Controller**

```csharp
/// <summary>
/// Starts a new AI-powered mock interview session
/// </summary>
/// <param name="input">Interview configuration and job description context</param>
/// <returns>Interview session with initial questions and session details</returns>
/// <response code="200">Interview session successfully started</response>
/// <response code="400">Invalid interview configuration</response>
/// <response code="401">Unauthorized access</response>
/// <response code="409">User already has an active interview session</response>
[HttpPost("start")]
[Authorize(AbpResearchWorkerPermissions.Interviews.Create)]
[ProducesResponseType(typeof(InterviewSessionDto), 200)]
[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
[ProducesResponseType(typeof(UnauthorizedResult), 401)]
[ProducesResponseType(typeof(ConflictResult), 409)]
public async Task<InterviewSessionDto> StartInterviewAsync([FromBody] StartInterviewDto input)
{
    return await _interviewEngineService.StartInterviewAsync(input);
}

/// <summary>
/// Submits an answer to an interview question and gets AI feedback
/// </summary>
/// <param name="sessionId">Unique identifier of the interview session</param>
/// <param name="input">User's answer content and metadata</param>
/// <returns>AI-generated feedback with next question if available</returns>
/// <response code="200">Answer submitted and feedback generated</response>
/// <response code="400">Invalid answer format</response>
/// <response code="404">Interview session not found</response>
/// <response code="410">Interview session expired or completed</response>
[HttpPost("{sessionId:guid}/answer")]
[Authorize(AbpResearchWorkerPermissions.Interviews.Edit)]
[ProducesResponseType(typeof(AnswerFeedbackDto), 200)]
[ProducesResponseType(typeof(ValidationProblemDetails), 400)]
[ProducesResponseType(typeof(NotFoundResult), 404)]
[ProducesResponseType(typeof(ErrorResponse), 410)]
public async Task<AnswerFeedbackDto> SubmitAnswerAsync(
    [FromRoute] Guid sessionId, 
    [FromBody] SubmitAnswerDto input)
{
    return await _interviewEngineService.SubmitAnswerAsync(sessionId, input);
}
```

## **DTO Documentation Patterns**

### **Job Description DTOs**

```csharp
/// <summary>
/// Request data for job description analysis
/// </summary>
public class CreateJobDescriptionDto
{
    /// <summary>
    /// Job title from the posting
    /// </summary>
    /// <example>Senior Software Engineer - Full Stack</example>
    [Required]
    [StringLength(200, MinimumLength = 5)]
    public string Title { get; set; }

    /// <summary>
    /// Complete job description content to be analyzed
    /// </summary>
    /// <example>We are seeking a talented Senior Software Engineer to join our team. The ideal candidate will have experience with .NET, React, and cloud platforms...</example>
    [Required]
    [StringLength(50000, MinimumLength = 100)]
    public string Content { get; set; }

    /// <summary>
    /// Company name (optional, will be extracted from content if not provided)
    /// </summary>
    /// <example>Microsoft Corporation</example>
    [StringLength(100)]
    public string? CompanyName { get; set; }

    /// <summary>
    /// Source URL where the job was found (optional)
    /// </summary>
    /// <example>https://careers.microsoft.com/job/12345</example>
    [Url]
    public string? SourceUrl { get; set; }
}

/// <summary>
/// Job description analysis results with AI-extracted insights
/// </summary>
public class JobDescriptionDto : AuditedEntityDto<Guid>
{
    /// <summary>
    /// Job title
    /// </summary>
    /// <example>Senior Software Engineer - Full Stack</example>
    public string Title { get; set; }

    /// <summary>
    /// Original job description content
    /// </summary>
    /// <example>We are seeking a talented Senior Software Engineer...</example>
    public string Content { get; set; }

    /// <summary>
    /// AI-extracted company information and insights
    /// </summary>
    public CompanyInsightDto CompanyInsight { get; set; }

    /// <summary>
    /// List of extracted job requirements and skills
    /// </summary>
    public List<JobRequirementDto> Requirements { get; set; }

    /// <summary>
    /// Analysis status
    /// </summary>
    /// <example>Completed</example>
    public JobDescriptionStatus Status { get; set; }

    /// <summary>
    /// Number of AI tokens used for analysis
    /// </summary>
    /// <example>1250</example>
    public int TokensUsed { get; set; }
}
```

### **Interview DTOs**

```csharp
/// <summary>
/// Request to start a new interview session
/// </summary>
public class StartInterviewDto
{
    /// <summary>
    /// Job description ID to base the interview on
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid JobDescriptionId { get; set; }

    /// <summary>
    /// Type of interview to conduct
    /// </summary>
    /// <example>Technical</example>
    [Required]
    public InterviewType InterviewType { get; set; }

    /// <summary>
    /// Interview mode (Text, Voice, Video)
    /// </summary>
    /// <example>Text</example>
    [Required]
    public InterviewMode Mode { get; set; }

    /// <summary>
    /// Desired difficulty level
    /// </summary>
    /// <example>Medium</example>
    [Required]
    public QuestionDifficulty Difficulty { get; set; }

    /// <summary>
    /// Expected duration in minutes
    /// </summary>
    /// <example>30</example>
    [Range(15, 120)]
    public int DurationMinutes { get; set; } = 30;
}

/// <summary>
/// Interview session information with current state
/// </summary>
public class InterviewSessionDto : AuditedEntityDto<Guid>
{
    /// <summary>
    /// Associated job description ID
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid JobDescriptionId { get; set; }

    /// <summary>
    /// Current interview status
    /// </summary>
    /// <example>InProgress</example>
    public InterviewStatus Status { get; set; }

    /// <summary>
    /// Interview type and configuration
    /// </summary>
    /// <example>Technical</example>
    public InterviewType InterviewType { get; set; }

    /// <summary>
    /// Communication mode
    /// </summary>
    /// <example>Text</example>
    public InterviewMode Mode { get; set; }

    /// <summary>
    /// Current question being asked
    /// </summary>
    public QuestionDto? CurrentQuestion { get; set; }

    /// <summary>
    /// List of all questions in the session
    /// </summary>
    public List<QuestionDto> Questions { get; set; }

    /// <summary>
    /// Session start time
    /// </summary>
    /// <example>2025-11-03T10:30:00Z</example>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// Expected end time
    /// </summary>
    /// <example>2025-11-03T11:00:00Z</example>
    public DateTime? ExpectedEndTime { get; set; }

    /// <summary>
    /// Current progress (0-100)
    /// </summary>
    /// <example>65</example>
    public int ProgressPercentage { get; set; }
}
```

### **CV Generation DTOs**

```csharp
/// <summary>
/// Request to generate a tailored CV
/// </summary>
public class GenerateCVDto
{
    /// <summary>
    /// Job description to tailor the CV for
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    [Required]
    public Guid JobDescriptionId { get; set; }

    /// <summary>
    /// CV template to use
    /// </summary>
    /// <example>Modern Professional</example>
    [Required]
    [StringLength(50)]
    public string TemplateName { get; set; }

    /// <summary>
    /// Additional instructions for CV customization
    /// </summary>
    /// <example>Emphasize cloud architecture experience and highlight leadership roles</example>
    [StringLength(1000)]
    public string? CustomInstructions { get; set; }

    /// <summary>
    /// Include personal projects section
    /// </summary>
    /// <example>true</example>
    public bool IncludePersonalProjects { get; set; } = true;
}
```

## **Controller Template**

```csharp
/// <summary>
/// Job Description Processing API - Analyzes job postings and extracts key requirements
/// </summary>
[ApiController]
[Route("api/interview-prep/jd")]
[Tags("Job Description Processing")]
[Authorize]
public class JDProcessorController : AbpControllerBase
{
    private readonly IJDProcessorAppService _jdProcessorService;

    public JDProcessorController(IJDProcessorAppService jdProcessorService)
    {
        _jdProcessorService = jdProcessorService;
    }

    /// <summary>
    /// Analyzes a job description and extracts requirements, skills, and company insights
    /// </summary>
    /// <param name="input">Job description content and metadata for AI analysis</param>
    /// <returns>Structured job requirements with company insights and skill analysis</returns>
    /// <remarks>
    /// This endpoint uses OpenAI GPT to analyze job descriptions and extract:
    /// - Required technical skills and experience levels
    /// - Company information and culture insights
    /// - Job responsibilities and expectations
    /// - Salary range and benefits (if mentioned)
    /// 
    /// Rate limits: 10 requests per minute per user
    /// Average processing time: 5-15 seconds
    /// </remarks>
    /// <response code="200">Job description successfully analyzed with AI insights</response>
    /// <response code="400">Invalid job description content or format</response>
    /// <response code="401">User not authenticated</response>
    /// <response code="403">User lacks permission to analyze job descriptions</response>
    /// <response code="429">AI API rate limit exceeded, try again later</response>
    /// <response code="500">Internal server error during AI processing</response>
    [HttpPost("analyze")]
    [Authorize(AbpResearchWorkerPermissions.JobDescriptions.Create)]
    [ProducesResponseType(typeof(JobDescriptionDto), 200)]
    [ProducesResponseType(typeof(ValidationProblemDetails), 400)]
    [ProducesResponseType(typeof(UnauthorizedResult), 401)]
    [ProducesResponseType(typeof(ForbidResult), 403)]
    [ProducesResponseType(typeof(ErrorResponse), 429)]
    [ProducesResponseType(typeof(ErrorResponse), 500)]
    public async Task<JobDescriptionDto> AnalyzeJobDescriptionAsync([FromBody] CreateJobDescriptionDto input)
    {
        return await _jdProcessorService.AnalyzeJobDescriptionAsync(input);
    }

    /// <summary>
    /// Retrieves a previously analyzed job description by ID
    /// </summary>
    /// <param name="id">Unique identifier of the job description</param>
    /// <returns>Job description with analysis results</returns>
    /// <response code="200">Job description found and returned</response>
    /// <response code="404">Job description not found or not accessible to current user</response>
    [HttpGet("{id:guid}")]
    [Authorize(AbpResearchWorkerPermissions.JobDescriptions.Default)]
    [ProducesResponseType(typeof(JobDescriptionDto), 200)]
    [ProducesResponseType(typeof(NotFoundResult), 404)]
    public async Task<JobDescriptionDto> GetJobDescriptionAsync([FromRoute] Guid id)
    {
        return await _jdProcessorService.GetAsync(id);
    }
}
```

## **Response DTOs**

### **Success Response Pattern:**
```csharp
/// <summary>
/// Standard success response for AI interview operations
/// </summary>
public class InterviewOperationResultDto
{
    /// <summary>
    /// Operation success status
    /// </summary>
    /// <example>true</example>
    public bool Success { get; set; }

    /// <summary>
    /// Human-readable success message
    /// </summary>
    /// <example>Interview session started successfully</example>
    public string Message { get; set; }

    /// <summary>
    /// Unique identifier for the created/updated resource
    /// </summary>
    /// <example>3fa85f64-5717-4562-b3fc-2c963f66afa6</example>
    public Guid? ResourceId { get; set; }

    /// <summary>
    /// AI processing metrics
    /// </summary>
    public AIProcessingMetricsDto Metrics { get; set; }
}

/// <summary>
/// AI processing usage and performance metrics
/// </summary>
public class AIProcessingMetricsDto
{
    /// <summary>
    /// Number of OpenAI tokens consumed
    /// </summary>
    /// <example>1250</example>
    public int TokensUsed { get; set; }

    /// <summary>
    /// Cost in USD for the AI operation
    /// </summary>
    /// <example>0.025</example>
    public decimal CostUSD { get; set; }

    /// <summary>
    /// Processing time in milliseconds
    /// </summary>
    /// <example>3500</example>
    public int ProcessingTimeMs { get; set; }

    /// <summary>
    /// AI model used for processing
    /// </summary>
    /// <example>gpt-4-turbo</example>
    public string ModelUsed { get; set; }
}
```

### **Error Response Pattern:**
```csharp
/// <summary>
/// Standard error response for API operations
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// Unique error code for programmatic handling
    /// </summary>
    /// <example>AI_RATE_LIMIT_EXCEEDED</example>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Human-readable error message
    /// </summary>
    /// <example>OpenAI API rate limit exceeded. Please try again in 60 seconds.</example>
    public string Message { get; set; }

    /// <summary>
    /// HTTP status code
    /// </summary>
    /// <example>429</example>
    public int StatusCode { get; set; }

    /// <summary>
    /// Request correlation ID for tracking
    /// </summary>
    /// <example>abc123-def456-ghi789</example>
    public string CorrelationId { get; set; }

    /// <summary>
    /// Retry delay in seconds (for rate limiting errors)
    /// </summary>
    /// <example>60</example>
    public int? RetryAfterSeconds { get; set; }

    /// <summary>
    /// Additional error details (development only)
    /// </summary>
    public object? Details { get; set; }
}
```

## **Swagger Configuration**

```csharp
public static void ConfigureSwagger(ServiceConfigurationContext context, IConfiguration configuration)
{
    context.Services.AddAbpSwaggerGenWithOidc(
        configuration["AuthServer:Authority"]!,
        ["AbpResearchWorker"],
        [AbpSwaggerOidcFlows.AuthorizationCode],
        null,
        options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "AI Interview Preparation System API", 
                Version = "v1",
                Description = @"
# AI Interview Preparation System

## Overview
This API provides comprehensive interview preparation services powered by artificial intelligence. 
The system helps job seekers analyze job descriptions, practice mock interviews, generate tailored CVs, 
and receive personalized feedback to improve their interview performance.

## Features
- **Job Description Analysis**: AI-powered parsing and requirement extraction
- **Company Intelligence**: Automated company research and insights
- **Mock Interviews**: Real-time AI interviews with voice/video support  
- **CV Generation**: Tailored resume creation based on job requirements
- **Chat Assistant**: Interactive AI guidance for interview preparation
- **Performance Analytics**: Progress tracking and improvement recommendations

## Authentication
All endpoints require OAuth 2.0 authentication. Use the Authorize button below to authenticate 
with your credentials.

## Rate Limits
- Job Description Analysis: 10 requests/minute
- Interview Sessions: 5 concurrent sessions per user
- CV Generation: 3 requests/hour
- Chat Assistant: 50 messages/hour

## Support
For API support, contact: api-support@interviewprep.ai
",
                Contact = new OpenApiContact
                {
                    Name = "AI Interview Prep API Support",
                    Email = "api-support@interviewprep.ai",
                    Url = new Uri("https://docs.interviewprep.ai")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });

            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);

            // Include XML comments from all assemblies
            var assemblies = new[]
            {
                typeof(JDProcessorController).Assembly,        // HttpApi
                typeof(CreateJobDescriptionDto).Assembly,     // Application.Contracts
                typeof(JobDescription).Assembly               // Domain
            };

            foreach (var assembly in assemblies)
            {
                var xmlFile = $"{assembly.GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
                }
            }

            // Add custom operation filters for better documentation
            options.OperationFilter<AuthorizeOperationFilter>();
            options.OperationFilter<TenantHeaderOperationFilter>();
            options.SchemaFilter<EnumSchemaFilter>();

            // Group endpoints by business module
            options.TagActionsBy(api => new List<string> { GetTagFromController(api.ActionDescriptor) });
        });
}

private static string GetTagFromController(ActionDescriptor actionDescriptor)
{
    var controllerName = actionDescriptor.RouteValues["controller"];
    
    return controllerName switch
    {
        "JDProcessor" => "Job Description Analysis",
        "InterviewEngine" => "Interview Simulation",
        "CVBuilder" => "CV Generation", 
        "ChatAssistant" => "Chat Assistant",
        "Analytics" => "Performance Analytics",
        "CompanyInsight" => "Company Intelligence",
        _ => "General"
    };
}
```

## **XML Documentation Configuration**

### **Project File Setup:**
```xml
<PropertyGroup>
    <GenerateDocumentationFile>true</GenerateDocumentationFile>
    <DocumentationFile>bin\$(Configuration)\$(TargetFramework)\$(AssemblyName).xml</DocumentationFile>
    <NoWarn>$(NoWarn);1591</NoWarn> <!-- Suppress missing XML comment warnings -->
</PropertyGroup>
```

## **API Documentation Checklist**

### **🔴 Required for All Endpoints:**
- [ ] Controller class has XML `<summary>` with business purpose
- [ ] Each action method has detailed `<summary>`, `<param>`, `<returns>`
- [ ] All HTTP status codes documented with `<response>` tags
- [ ] `[ProducesResponseType]` attributes for all possible responses
- [ ] `[Authorize]` attributes with specific permission references
- [ ] Request/response DTOs have property documentation with examples

### **🟡 Recommended Enhancements:**
- [ ] `[Tags]` for logical endpoint grouping
- [ ] `[SwaggerOperation]` for complex AI operations
- [ ] Realistic example values in all DTO properties
- [ ] Rate limiting information in method documentation
- [ ] Processing time expectations for AI operations
- [ ] Multi-tenancy considerations documented
- [ ] Error handling patterns documented
- [ ] Integration examples in `<remarks>` sections

### **🔵 Advanced Documentation:**
- [ ] OpenAPI operation filters for custom headers
- [ ] Schema filters for enum documentation
- [ ] Custom example providers for complex DTOs
- [ ] Webhook documentation for real-time features
- [ ] SDK generation configuration
- [ ] Postman collection generation setup

This comprehensive API documentation standard ensures that all endpoints in the AI Interview Preparation System are thoroughly documented, making the API easy to understand and integrate with for both internal development and external consumers! 📚