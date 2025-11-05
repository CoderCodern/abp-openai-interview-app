# AI Interview Preparation System - Project Structure & Overview

**Project:** AI Interview Preparation System  
**Service:** ABP Research Worker (Core Platform)  
**Developer:** Coder Codern  
**Architecture:** ABP.io Multi-Layer + Clean Architecture  
**Technology:** .NET 9 + ABP.io Framework v9.3.5 + Angular 20  
**Version:** 1.0  
**Date:** November 2, 2025  

---

## Service Overview

The **AI Interview Preparation System** is an intelligent platform that helps job candidates prepare for interviews through AI-powered analysis, mock interviews, and personalized feedback. Built on ABP.io framework, this system processes job descriptions, generates company insights, conducts realistic interview simulations, and creates tailored CVs using OpenAI's advanced AI capabilities.

### **Core Responsibilities**

- **Job Description Analysis**: Parse and analyze JD content to extract key requirements and skills
- **Company Intelligence**: Research and generate comprehensive company insights using AI
- **Interview Simulation**: Conduct realistic mock interviews with AI-powered questions and feedback
- **CV Generation**: Create tailored CVs optimized for specific job requirements
- **Chat Assistant**: Provide intelligent guidance and answer interview preparation questions
- **Performance Analytics**: Track progress, analyze performance, and generate improvement recommendations
- **Multi-tenancy Support**: Enable SaaS deployment with tenant isolation and customization

---

## Project Structure & Component Explanations

### 📁 **ABP.io Multi-Layer Architecture**

```
AbpResearchWorker/
├── src/
│   ├── AbpResearchWorker.Domain.Shared/                  // 📋 Shared Domain Concepts
│   │   ├── Enums/
│   │   │   ├── InterviewStatus.cs                        // Pending/InProgress/Completed/Cancelled
│   │   │   ├── InterviewType.cs                          // Technical/Behavioral/Mixed/Case Study
│   │   │   ├── QuestionDifficulty.cs                     // Easy/Medium/Hard/Expert levels
│   │   │   ├── CVStatus.cs                               // Draft/Generated/Approved/Published
│   │   │   ├── CompanySize.cs                            // Startup/SmallBusiness/Enterprise/Corporation
│   │   │   ├── JobLevel.cs                               // Entry/Mid/Senior/Executive/C-Level
│   │   │   ├── InterviewMode.cs                          // Text/Voice/Video/Hybrid modes
│   │   │   ├── FeedbackType.cs                           // Technical/Communication/Overall
│   │   │   └── BookType.cs                               // Legacy: Adventure/Biography/ScienceFiction
│   │   ├── Consts/
│   │   │   ├── AbpResearchWorkerConsts.cs                // Global constants and database config
│   │   │   ├── InterviewConsts.cs                        // Interview session limits and rules
│   │   │   ├── AIConsts.cs                               // OpenAI token limits and model configs
│   │   │   ├── CVConsts.cs                               // CV generation limits and templates
│   │   │   └── CompanyConsts.cs                          // Company research parameters
│   │   ├── MultiTenancy/
│   │   │   └── MultiTenancyConsts.cs                     // Multi-tenancy configuration (enabled)
│   │   ├── Localization/
│   │   │   └── AbpResearchWorkerResource.cs              // Localization resource (21 languages)
│   │   └── AbpResearchWorkerDomainSharedModule.cs        // ABP shared module configuration
│   │
│   ├── AbpResearchWorker.Domain/                         // 🏛️ Business Rules & Core Logic
│   │   ├── Entities/
│   │   │   ├── JobDescriptions/
│   │   │   │   ├── JobDescription.cs                     // Main JD aggregate: content + analysis
│   │   │   │   ├── JobRequirement.cs                     // Extracted skills and qualifications
│   │   │   │   └── CompanyInfo.cs                        // Company details and insights
│   │   │   ├── Interviews/
│   │   │   │   ├── InterviewSession.cs                   // Interview aggregate: session data
│   │   │   │   ├── Question.cs                           // Individual interview questions
│   │   │   │   ├── Answer.cs                             // User responses with analysis
│   │   │   │   └── InterviewFeedback.cs                  // Performance scores and recommendations
│   │   │   ├── CVs/
│   │   │   │   ├── CVDocument.cs                         // Generated CV aggregate
│   │   │   │   ├── CVSection.cs                          // CV content sections (experience, education)
│   │   │   │   ├── CVTemplate.cs                         // CV layout templates
│   │   │   │   └── CVVersion.cs                          // Version control for CV iterations
│   │   │   ├── Chat/
│   │   │   │   ├── ChatSession.cs                        // Assistant conversation aggregate
│   │   │   │   ├── ChatMessage.cs                        // Individual chat messages
│   │   │   │   └── ChatContext.cs                        // Conversation context and memory
│   │   │   ├── Analytics/
│   │   │   │   ├── UserProgress.cs                       // Performance tracking aggregate
│   │   │   │   ├── InterviewMetrics.cs                   // Interview performance data
│   │   │   │   └── SystemUsage.cs                        // Platform usage analytics
│   │   │   └── Legacy/
│   │   │       └── Book.cs                               // Current implementation (to be evolved)
│   │   ├── Services/
│   │   │   ├── Interview/
│   │   │   │   ├── IInterviewDomainService.cs            // Interview business logic interface
│   │   │   │   ├── InterviewDomainService.cs             // Interview orchestration and scoring
│   │   │   │   ├── IQuestionGenerationService.cs        // Question creation business rules
│   │   │   │   └── QuestionGenerationService.cs         // AI question generation logic
│   │   │   ├── JD/
│   │   │   │   ├── IJDAnalysisDomainService.cs           // JD parsing business rules
│   │   │   │   ├── JDAnalysisDomainService.cs            // JD content extraction logic
│   │   │   │   ├── ICompanyResearchService.cs           // Company intelligence interface
│   │   │   │   └── CompanyResearchService.cs             // Company analysis business rules
│   │   │   ├── CV/
│   │   │   │   ├── ICVGenerationDomainService.cs         // CV creation business rules
│   │   │   │   ├── CVGenerationDomainService.cs          // CV tailoring and optimization logic
│   │   │   │   ├── ICVScoringService.cs                  // CV quality assessment
│   │   │   │   └── CVScoringService.cs                   // ATS compatibility scoring
│   │   │   └── Analytics/
│   │   │       ├── IPerformanceAnalysisService.cs       // Progress tracking interface
│   │   │       └── PerformanceAnalysisService.cs        // Performance calculation logic
│   │   ├── Repositories/                                 // Data access contracts
│   │   │   ├── IJobDescriptionRepository.cs              // JD data access interface
│   │   │   ├── IInterviewSessionRepository.cs           // Interview data repository
│   │   │   ├── ICVDocumentRepository.cs                  // CV data access interface
│   │   │   ├── IChatSessionRepository.cs                 // Chat data repository
│   │   │   ├── IUserProgressRepository.cs               // Analytics repository
│   │   │   └── IBookRepository.cs                        // Legacy: Book repository (inherited)
│   │   ├── ValueObjects/                                 // Immutable data structures
│   │   │   ├── SkillMatch.cs                             // JD-CV skill alignment
│   │   │   ├── InterviewScore.cs                         // Performance scoring structure
│   │   │   ├── CompanyInsight.cs                         // AI-generated company data
│   │   │   ├── ActionItem.cs                             // Extracted tasks and recommendations
│   │   │   ├── ContactInfo.cs                            // User contact details
│   │   │   └── DateRange.cs                              // Experience and project periods
│   │   ├── Specifications/                               // Business rule validation
│   │   │   ├── InterviewReadinessSpecification.cs       // Interview prerequisite validation
│   │   │   ├── CVCompletenessSpecification.cs           // CV quality requirements
│   │   │   ├── JDValiditySpecification.cs               // Job description validation rules
│   │   │   └── TokenUsageSpecification.cs               // AI budget limit specifications
│   │   ├── Events/                                       // Domain events
│   │   │   ├── InterviewCompletedEvent.cs               // After interview session completion
│   │   │   ├── CVGeneratedEvent.cs                      // After successful CV creation
│   │   │   ├── JDAnalyzedEvent.cs                       // After JD processing completion
│   │   │   ├── FeedbackGeneratedEvent.cs                // After performance analysis
│   │   │   └── UserProgressUpdatedEvent.cs              // After analytics update
│   │   ├── Data/
│   │   │   └── AbpResearchWorkerDbMigrationService.cs   // Database migration orchestrator
│   │   └── AbpResearchWorkerDomainModule.cs             // ABP domain module registration
│   │
│   ├── AbpResearchWorker.Application.Contracts/          // 📋 API Contracts & DTOs
│   │   ├── Dtos/
│   │   │   ├── JobDescriptions/
│   │   │   │   ├── CreateJobDescriptionDto.cs           // Input: JD content submission
│   │   │   │   ├── JobDescriptionDto.cs                 // Output: JD with analysis
│   │   │   │   ├── UpdateJobDescriptionDto.cs           // Input: JD modifications
│   │   │   │   ├── JobRequirementDto.cs                 // Output: Extracted requirements
│   │   │   │   └── CompanyInsightDto.cs                 // Output: AI company research
│   │   │   ├── Interviews/
│   │   │   │   ├── StartInterviewDto.cs                 // Input: Interview configuration
│   │   │   │   ├── InterviewSessionDto.cs               // Output: Session data and status
│   │   │   │   ├── QuestionDto.cs                       // Output: Interview questions
│   │   │   │   ├── SubmitAnswerDto.cs                   // Input: User responses
│   │   │   │   ├── AnswerDto.cs                         // Output: Answer with analysis
│   │   │   │   └── InterviewFeedbackDto.cs              // Output: Performance results
│   │   │   ├── CVs/
│   │   │   │   ├── GenerateCVDto.cs                     // Input: CV generation request
│   │   │   │   ├── CVDocumentDto.cs                     // Output: Generated CV data
│   │   │   │   ├── UpdateCVDto.cs                       // Input: CV modifications
│   │   │   │   ├── CVSectionDto.cs                      // Output: CV content sections
│   │   │   │   └── CVTemplateDto.cs                     // Output: Available templates
│   │   │   ├── Chat/
│   │   │   │   ├── StartChatDto.cs                      // Input: Chat session initiation
│   │   │   │   ├── ChatMessageDto.cs                    // Input/Output: Chat messages
│   │   │   │   ├── ChatSessionDto.cs                    // Output: Session information
│   │   │   │   └── ChatContextDto.cs                    // Output: Conversation context
│   │   │   ├── Analytics/
│   │   │   │   ├── UserProgressDto.cs                   // Output: Progress tracking
│   │   │   │   ├── InterviewMetricsDto.cs               // Output: Performance analytics
│   │   │   │   ├── DashboardStatsDto.cs                 // Output: Dashboard data
│   │   │   │   └── RecommendationDto.cs                 // Output: Improvement suggestions
│   │   │   └── Legacy/
│   │   │       ├── BookDto.cs                           // Legacy: Book data transfer
│   │   │       └── CreateUpdateBookDto.cs               // Legacy: Book operations
│   │   ├── Services/                                      // Application service interfaces
│   │   │   ├── IJDProcessorAppService.cs                 // JD analysis operations
│   │   │   ├── ICompanyInsightAppService.cs              // Company research operations
│   │   │   ├── IInterviewEngineAppService.cs             // Interview simulation operations
│   │   │   ├── ICVBuilderAppService.cs                   // CV generation operations
│   │   │   ├── IChatAssistantAppService.cs               // Chat assistant operations
│   │   │   ├── IAnalyticsAppService.cs                   // Performance tracking operations
│   │   │   ├── IUserManagementAppService.cs              // User profile management
│   │   │   └── IBookAppService.cs                        // Legacy: Book management (CRUD)
│   │   ├── Permissions/
│   │   │   └── AbpResearchWorkerPermissions.cs           // Authorization permissions
│   │   └── AbpResearchWorkerApplicationContractsModule.cs // ABP contracts module
│   │
│   ├── AbpResearchWorker.Application/                    // 🎯 Business Operations & Orchestration
│   │   ├── Services/                                      // Application service implementations
│   │   │   ├── JDProcessorAppService.cs                  // Main: JD parsing and analysis
│   │   │   ├── CompanyInsightAppService.cs               // Company research coordinator
│   │   │   ├── InterviewEngineAppService.cs              // Interview simulation orchestrator
│   │   │   ├── CVBuilderAppService.cs                    // CV generation coordinator
│   │   │   ├── ChatAssistantAppService.cs                // Chat assistant coordinator
│   │   │   ├── AnalyticsAppService.cs                    // Performance analytics service
│   │   │   ├── UserManagementAppService.cs               // User profile management
│   │   │   └── BookAppService.cs                         // Legacy: Current CRUD implementation
│   │   ├── AIStrategies/                                 // Smart AI processing strategies
│   │   │   ├── IInterviewStrategy.cs                     // Strategy pattern interface
│   │   │   ├── TechnicalInterviewStrategy.cs             // Handles coding interviews
│   │   │   ├── BehavioralInterviewStrategy.cs            // Handles soft skill interviews
│   │   │   ├── CaseStudyInterviewStrategy.cs             // Handles business case interviews
│   │   │   ├── ExecutiveInterviewStrategy.cs             // Handles leadership interviews
│   │   │   └── DefaultInterviewStrategy.cs               // Fallback for mixed interviews
│   │   ├── PromptEngines/                                // AI prompt management
│   │   │   ├── IJDPromptEngine.cs                        // JD analysis prompts
│   │   │   ├── JDPromptEngine.cs                         // JD processing prompt logic
│   │   │   ├── IInterviewPromptEngine.cs                 // Interview question prompts
│   │   │   ├── InterviewPromptEngine.cs                  // Question generation logic
│   │   │   ├── ICVPromptEngine.cs                        // CV generation prompts
│   │   │   ├── CVPromptEngine.cs                         // CV optimization logic
│   │   │   ├── ICompanyPromptEngine.cs                   // Company research prompts
│   │   │   └── CompanyPromptEngine.cs                    // Company analysis logic
│   │   ├── Handlers/                                     // Event & message handlers
│   │   │   ├── InterviewEventHandler.cs                  // Interview completion processing
│   │   │   ├── CVGenerationEventHandler.cs               // CV generation post-processing
│   │   │   ├── JDAnalysisEventHandler.cs                 // JD analysis completion handler
│   │   │   ├── ProgressUpdateHandler.cs                  // Analytics update coordinator
│   │   │   └── NotificationHandler.cs                    // User notification sender
│   │   ├── Mappers/                                      // Object mapping configuration
│   │   │   └── AbpResearchWorkerAutoMapperProfile.cs    // Entity ↔ DTO mapping rules
│   │   ├── Validators/                                   // Input validation rules
│   │   │   ├── JDValidationService.cs                    // Job description validation
│   │   │   ├── InterviewInputValidator.cs                // Interview session validation
│   │   │   ├── CVRequestValidator.cs                     // CV generation validation
│   │   │   └── ChatMessageValidator.cs                   // Chat input validation
│   │   ├── BackgroundJobs/                               // Async processing jobs
│   │   │   ├── JDAnalysisJob.cs                          // Background JD processing
│   │   │   ├── CompanyResearchJob.cs                     // Async company analysis
│   │   │   ├── CVGenerationJob.cs                        // Background CV creation
│   │   │   ├── InterviewAnalysisJob.cs                   // Post-interview processing
│   │   │   └── AnalyticsUpdateJob.cs                     // Performance metrics updates
│   │   └── AbpResearchWorkerApplicationModule.cs         // ABP application module
│   │
│   ├── AbpResearchWorker.EntityFrameworkCore/            // 🗄️ Data Persistence Layer
│   │   ├── EntityFrameworkCore/
│   │   │   ├── AbpResearchWorkerDbContext.cs             // Main EF Core database context
│   │   │   ├── IAbpResearchWorkerDbContext.cs            // Database context interface
│   │   │   ├── AbpResearchWorkerDbContextFactory.cs      // Design-time factory for migrations
│   │   │   └── EntityFrameworkCoreAbpResearchWorkerDbSchemaMigrator.cs // Schema migrator
│   │   ├── Configurations/                               // Entity configurations
│   │   │   ├── JobDescriptionConfiguration.cs           // JD entity EF configuration
│   │   │   ├── InterviewSessionConfiguration.cs         // Interview entity config
│   │   │   ├── CVDocumentConfiguration.cs               // CV entity configuration
│   │   │   ├── ChatSessionConfiguration.cs              // Chat entity configuration
│   │   │   ├── UserProgressConfiguration.cs             // Analytics entity config
│   │   │   └── BookConfiguration.cs                     // Legacy: Book entity config
│   │   ├── Repositories/                                 // Repository implementations
│   │   │   ├── EfCoreJobDescriptionRepository.cs        // EF Core JD repository
│   │   │   ├── EfCoreInterviewSessionRepository.cs      // EF Core interview repository
│   │   │   ├── EfCoreCVDocumentRepository.cs            // EF Core CV repository
│   │   │   ├── EfCoreChatSessionRepository.cs           // EF Core chat repository
│   │   │   ├── EfCoreUserProgressRepository.cs          // EF Core analytics repository
│   │   │   └── EfCoreBookRepository.cs                  // Legacy: EF Core book repository
│   │   ├── Migrations/                                   // Database migrations
│   │   │   ├── 20251028030947_Initial.cs                // Initial database schema
│   │   │   ├── 20251028030947_Initial.Designer.cs       // Migration designer file
│   │   │   └── AbpResearchWorkerDbContextModelSnapshot.cs // Current model snapshot
│   │   └── AbpResearchWorkerEntityFrameworkCoreModule.cs // ABP EF Core module
│   │
│   ├── AbpResearchWorker.Infrastructure/                 // 🔧 Business Infrastructure (To be created)
│   │   ├── AI/                                           // OpenAI integration services
│   │   │   ├── IOpenAIService.cs                         // OpenAI service contract
│   │   │   ├── OpenAIService.cs                          // OpenAI API implementation
│   │   │   ├── IPromptRenderer.cs                        // Dynamic prompt rendering
│   │   │   ├── PromptRenderer.cs                         // Template engine implementation
│   │   │   ├── OpenAIRetryPolicy.cs                     // Retry logic for API failures
│   │   │   ├── Models/                                   // OpenAI data models
│   │   │   │   ├── ChatRequest.cs                        // GPT chat request structure
│   │   │   │   ├── ChatResponse.cs                       // GPT chat response structure
│   │   │   │   ├── WhisperRequest.cs                     // Speech-to-text request
│   │   │   │   ├── TTSRequest.cs                         // Text-to-speech request
│   │   │   │   └── TokenUsageInfo.cs                     // Token consumption tracking
│   │   │   └── Configuration/
│   │   │       ├── OpenAIOptions.cs                      // Configuration options
│   │   │       └── AIModelSettings.cs                    // Model-specific settings
│   │   ├── WebRTC/                                       // Real-time communication
│   │   │   ├── IWebRTCService.cs                         // WebRTC service interface
│   │   │   ├── WebRTCService.cs                          // Video/audio streaming
│   │   │   ├── ISignalRHub.cs                            // SignalR hub interface
│   │   │   ├── InterviewHub.cs                           // Real-time interview hub
│   │   │   └── WebRTCConfiguration.cs                    // WebRTC settings
│   │   ├── FileProcessing/                               // File handling services
│   │   │   ├── ICVExportService.cs                       // CV export interface
│   │   │   ├── CVExportService.cs                        // PDF/DOCX CV generation
│   │   │   ├── IFileUploadService.cs                     // File upload interface
│   │   │   ├── FileUploadService.cs                      // Document upload handling
│   │   │   └── FileValidationService.cs                  // File type/size validation
│   │   ├── ExternalAPIs/                                 // Third-party integrations
│   │   │   ├── IJobBoardService.cs                       // Job board API interface
│   │   │   ├── LinkedInAPIService.cs                     // LinkedIn integration
│   │   │   ├── IndeedAPIService.cs                       // Indeed job search API
│   │   │   └── CompanyAPIService.cs                      // Company data providers
│   │   ├── Caching/                                      // Performance optimization
│   │   │   ├── IRedisCacheService.cs                     // Cache service interface
│   │   │   ├── RedisCacheService.cs                      // Redis implementation
│   │   │   ├── CacheKeyGenerator.cs                      // Consistent cache keys
│   │   │   └── CacheInvalidationService.cs               // Cache cleanup logic
│   │   ├── Monitoring/                                   // System observability
│   │   │   ├── IMetricsService.cs                        // Metrics collection interface
│   │   │   ├── MetricsService.cs                         // Performance monitoring
│   │   │   ├── HealthCheckService.cs                     // Service health checks
│   │   │   └── AIUsageMonitor.cs                         // OpenAI usage tracking
│   │   └── AbpResearchWorkerInfrastructureModule.cs      // ABP infrastructure module
│   │
│   ├── AbpResearchWorker.DbMigrator/                     // 🚀 Database Migration Tool
│   │   ├── Program.cs                                    // Migration console application
│   │   ├── appsettings.json                              // Database connection settings
│   │   ├── appsettings.Development.json                  // Development database config
│   │   ├── appsettings.Production.json                   // Production database config
│   │   └── AbpResearchWorkerDbMigratorModule.cs          // ABP migration module
│   │
│   ├── AbpResearchWorker.HttpApi/                        // 🌐 Web API Layer
│   │   ├── Controllers/                                  // REST API controllers
│   │   │   ├── JDProcessorController.cs                  // Job description processing API
│   │   │   ├── CompanyInsightController.cs               // Company research API
│   │   │   ├── InterviewEngineController.cs              // Interview simulation API
│   │   │   ├── CVBuilderController.cs                    // CV generation API
│   │   │   ├── ChatAssistantController.cs                // Chat assistant API
│   │   │   ├── AnalyticsController.cs                    // Performance analytics API
│   │   │   ├── UserManagementController.cs               // User profile API
│   │   │   └── BookController.cs                         // Legacy: Book CRUD API
│   │   ├── Filters/                                      // Cross-cutting API concerns
│   │   │   ├── GlobalExceptionFilter.cs                  // API exception handling
│   │   │   ├── ValidationFilter.cs                       // Input validation filter
│   │   │   ├── RateLimitFilter.cs                        // API rate limiting
│   │   │   └── AIUsageTrackingFilter.cs                  // OpenAI usage monitoring
│   │   ├── Middleware/                                   // Request pipeline components
│   │   │   ├── RequestLoggingMiddleware.cs               // Request/response logging
│   │   │   ├── CorrelationIdMiddleware.cs                // Request correlation tracking
│   │   │   └── AIThrottlingMiddleware.cs                 // AI request throttling
│   │   └── AbpResearchWorkerHttpApiModule.cs             // ABP HTTP API module
│   │
│   ├── AbpResearchWorker.HttpApi.Client/                 // 📡 HTTP Client Library
│   │   ├── ClientProxies/                                // Generated API clients
│   │   │   ├── JDProcessorClientProxy.cs                 // JD processing client
│   │   │   ├── InterviewEngineClientProxy.cs             // Interview API client
│   │   │   ├── CVBuilderClientProxy.cs                   // CV generation client
│   │   │   ├── ChatAssistantClientProxy.cs               // Chat API client
│   │   │   ├── AnalyticsClientProxy.cs                   // Analytics API client
│   │   │   └── BookClientProxy.cs                        // Legacy: Book API client
│   │   └── AbpResearchWorkerHttpApiClientModule.cs       // ABP HTTP client module
│   │
│   └── AbpResearchWorker.HttpApi.Host/                   // 🚀 Application Host
│       ├── Controllers/
│       │   └── HomeController.cs                         // Application entry controller
│       ├── Properties/
│       │   └── launchSettings.json                       // Development launch settings
│       ├── wwwroot/                                      // Static web assets
│       ├── appsettings.json                              // Main application configuration
│       ├── appsettings.Development.json                  // Development settings
│       ├── appsettings.Production.json                   // Production configuration
│       ├── Program.cs                                    // Application entry point
│       └── AbpResearchWorkerHttpApiHostModule.cs         // Main application module
│
├── angular/                                              // 🅰️ Angular Frontend Application
│   ├── src/
│   │   ├── app/
│   │   │   ├── components/
│   │   │   │   ├── job-description/                      // JD management UI
│   │   │   │   ├── company-insights/                     // Company research UI
│   │   │   │   ├── interview-simulator/                  // Interview simulation UI
│   │   │   │   ├── cv-builder/                           // CV generation UI
│   │   │   │   ├── chat-assistant/                       // Chat interface UI
│   │   │   │   ├── analytics-dashboard/                  // Performance dashboard UI
│   │   │   │   ├── user-profile/                         // User management UI
│   │   │   │   └── book/                                 // Legacy: Book management UI
│   │   │   ├── proxy/                                    // Generated API proxies
│   │   │   ├── shared/                                   // Shared components and services
│   │   │   ├── core/                                     // Core services and guards
│   │   │   ├── app.component.ts                          // Root application component
│   │   │   ├── app.config.ts                             // Application configuration
│   │   │   └── app.routes.ts                             // Routing configuration
│   │   ├── environments/                                 // Environment configurations
│   │   ├── assets/                                       // Static assets (images, fonts)
│   │   ├── styles.scss                                   // Global styles
│   │   └── index.html                                    // Application entry HTML
│   ├── package.json                                      // NPM dependencies (Angular 20)
│   ├── angular.json                                      // Angular CLI configuration
│   ├── tsconfig.json                                     // TypeScript configuration
│   └── README.md                                         // Angular setup instructions
│
├── test/                                                 // 🧪 Test Projects
│   ├── AbpResearchWorker.Domain.Tests/                   // Domain layer tests
│   ├── AbpResearchWorker.Application.Tests/              // Application layer tests
│   │   └── Books/
│   │       └── BookAppService_Tests .cs                  // Current book service tests
│   ├── AbpResearchWorker.EntityFrameworkCore.Tests/     // Data layer tests
│   ├── AbpResearchWorker.HttpApi.Tests/                 // API layer tests (to be created)
│   ├── AbpResearchWorker.TestBase/                      // Common test infrastructure
│   └── AbpResearchWorker.HttpApi.Client.ConsoleTestApp/ // Console test application
│
├── docs/                                                 // 📚 Documentation
│   ├── project-structure-and-logic.md                   // Current system documentation
│   ├── strategies/                                       // Project planning documents
│   │   ├── 1-project-planning-roadmap.md                // Strategic roadmap
│   │   ├── 2-functional-design.md                       // Functional requirements
│   │   └── 3-architecture-design.md                     // System architecture (C4 model)
│   └── system-specification-document/                    // Technical specifications
│       └── 1-project-structure-and-overview.md          // This document
│
├── etc/                                                  // 🛠️ Development Tools
│   ├── abp-studio/
│   │   └── run-profiles/                                 // ABP Studio run configurations
│   └── scripts/
│       ├── initialize-solution.ps1                      // Solution initialization script
│       └── migrate-database.ps1                         // Database migration script
│
├── AbpResearchWorker.sln                                // 📋 Visual Studio solution file
├── AbpResearchWorker.abpsln                             // ABP Studio solution file
├── common.props                                          // Common MSBuild properties
├── NuGet.Config                                          // NuGet package sources
└── README.md                                             // Project overview and setup guide
```

### 🎯 **ABP.io Layer Explanations**

#### **Domain.Shared Layer (Common Domain Concepts)**
- **Purpose**: Contains shared enums, constants, and basic types used across all layers
- **Enums**: Business categorizations (InterviewStatus, InterviewType, QuestionDifficulty, CVStatus, JobLevel)  
- **Constants**: Global limits, configuration values, AI model settings, and business rules
- **Multi-tenancy**: Enabled for SaaS deployment with tenant isolation
- **Localization**: Supports 21 languages with extensible resource system
- **Benefits**: Prevents circular dependencies, allows DTOs to reference domain enums without heavy dependencies

#### **Domain Layer (Business Rules & Core Logic)**
- **Entities**: Core business aggregates with identity, behavior, and business rules enforcement
  - **JobDescription**: Job posting analysis and company research aggregate
  - **InterviewSession**: Interview simulation with questions, answers, and feedback
  - **CVDocument**: AI-generated resume with sections, templates, and versioning
  - **ChatSession**: AI assistant conversations with context memory
  - **UserProgress**: Performance analytics and improvement tracking
- **Domain Services**: Complex business operations spanning multiple entities
- **Value Objects**: Immutable business concepts (SkillMatch, InterviewScore, CompanyInsight)
- **Repository Interfaces**: Data persistence contracts (no implementations)
- **Specifications**: Business rule validation for complex queries and validations
- **Domain Events**: Business events triggering workflows and integrations

#### **Application.Contracts Layer (API Definitions)**
- **Purpose**: Defines the application's public interface without implementation details
- **DTOs**: Structured data for API input/output, organized by business module
  - Job Description DTOs for JD processing and company insights
  - Interview DTOs for session management and feedback
  - CV DTOs for generation requests and document management
  - Chat DTOs for assistant interactions
  - Analytics DTOs for performance tracking
- **Service Interfaces**: Application service contracts defining available operations
- **Permissions**: Authorization rules for role-based access control
- **Benefits**: Can be shared with clients/frontend without exposing business logic

#### **Application Layer (Use Case Orchestration)**
- **Application Services**: Coordinate business workflows with cross-cutting concerns (auth, validation, transactions)
- **AI Strategies**: Interview-specific business logic using strategy pattern
  - Technical interviews (coding challenges, system design)
  - Behavioral interviews (leadership, teamwork, problem-solving)
  - Case study interviews (business analysis, consulting)
  - Executive interviews (strategic thinking, vision)
- **Prompt Engines**: AI prompt management for different business contexts
- **Event Handlers**: Process domain events and coordinate with external systems
- **Background Jobs**: Async processing for AI-intensive operations
- **Validators**: Input validation using FluentValidation and ABP attributes
- **Mappers**: AutoMapper profiles for Entity ↔ DTO conversions

#### **EntityFrameworkCore Layer (Data Persistence)**
- **Purpose**: **Dedicated layer for Entity Framework Core data access implementation**
- **DbContext**: Multi-tenant database context with entity configurations and DbSets
- **Entity Configurations**: Fluent API configurations for database schema and relationships
- **Repository Implementations**: Concrete EF Core implementations of domain repository interfaces
- **Migrations**: Database schema version control and deployment scripts
- **Current State**: Initial migration created (20251028030947_Initial.cs)
- **Benefits**: Separates data access technology from business infrastructure concerns

#### **Infrastructure Layer (Business Infrastructure) - To Be Created**
- **Purpose**: **Non-data external integrations and business infrastructure services**
- **AI Integration**: OpenAI GPT, Whisper (STT), and TTS API communication with retry policies
- **WebRTC Services**: Real-time video/audio communication for live interviews
- **File Processing**: CV export (PDF/DOCX), document upload, and validation services
- **External APIs**: Job board integrations (LinkedIn, Indeed), company data providers
- **Caching**: Redis-based performance optimization with intelligent key generation
- **Monitoring**: Metrics collection, health checks, AI usage tracking, and system observability

#### **DbMigrator (Database Management)**
- **Purpose**: **Standalone console application for database migrations and seeding**
- **Multi-tenant Support**: Handles both host and tenant database migrations
- **Environment Configuration**: Separate settings for development and production
- **Benefits**: Can be deployed separately, supports automated CI/CD pipelines

#### **HttpApi Layer (Web Interface)**
- **Controllers**: RESTful API endpoints following ABP conventions and OpenAPI standards
- **Organized by Business Module**: Separate controllers for JD processing, interviews, CV building, chat, analytics
- **Filters**: Cross-cutting concerns including AI usage tracking and rate limiting
- **Middleware**: Request/response pipeline with correlation IDs and AI throttling

#### **HttpApi.Client Layer (Client SDK)**
- **Purpose**: **HTTP client library for consuming the API from other applications**
- **Client Proxies**: Strongly-typed HTTP clients for each business module
- **Benefits**: Other services can reference this for type-safe API calls
- **Auto-Generated**: ABP can auto-generate client proxies from API definitions

#### **HttpApi.Host Layer (Application Composition Root)**
- **Purpose**: Application entry point composing all layers and configuring runtime
- **Current Configuration**: 
  - HTTPS endpoint: https://localhost:44350
  - CORS enabled for Angular frontend (http://localhost:4200)
  - OpenIddict authentication with JWT tokens
  - Multi-tenancy middleware enabled
  - Swagger UI with OAuth integration
- **Module Dependencies**: Orchestrates all ABP modules and custom services

#### **Angular Frontend (Single Page Application)**
- **Technology**: Angular 20.0 with ABP NG packages v9.3.5
- **UI Theme**: LeptonX Lite with responsive design
- **Current Implementation**: Complete Books management UI as foundation
- **Planned Components**: 
  - Job Description analyzer and company insights viewer
  - Real-time interview simulator with WebRTC integration
  - AI-powered CV builder with template selection
  - Interactive chat assistant interface
  - Performance analytics dashboard
- **Authentication**: OAuth 2.0 integration with backend APIs

#### **Test Projects (Quality Assurance)**
- **Layered Testing**: Separate test projects for each application layer
- **Current Coverage**: Basic CRUD tests for Books functionality
- **Planned Expansion**: 
  - Unit tests for AI service integrations
  - Integration tests for interview workflows
  - End-to-end tests for complete user journeys
- **Test Infrastructure**: ABP test base classes with dependency injection

### 🔄 **Migration Strategy: From Books to AI Interview System**

#### **Phase 1: Foundation Enhancement (Current)**
```csharp
// Keep existing Book system as reference implementation
public class Book : AuditedAggregateRoot<Guid>
{
    public string Name { get; set; }
    public BookType Type { get; set; }
    public DateTime PublishDate { get; set; }
    public float Price { get; set; }
}

// Add new AI interview entities alongside
public class JobDescription : AuditedAggregateRoot<Guid>
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Guid? CompanyId { get; set; }
    public JobDescriptionStatus Status { get; set; }
    public string ParsedData { get; set; } // JSON
}
```

#### **Phase 2: AI Infrastructure Setup**
- Implement OpenAI service integration layer
- Add background job processing for AI operations
- Create prompt engineering framework
- Set up WebRTC for real-time interviews

#### **Phase 3: Business Module Implementation**
- Build JD processing and company intelligence modules
- Develop interview simulation engine
- Create AI-powered CV generation system
- Implement chat assistant with context memory

#### **Phase 4: Frontend Integration**
- Replace/extend Angular components from Books to AI features
- Add real-time interview interface with WebRTC
- Implement responsive analytics dashboard
- Create mobile-friendly PWA experience

#### **Phase 5: Production Readiness**
- Complete testing coverage for all modules
- Implement comprehensive monitoring and observability
- Optimize performance with caching and background processing
- Deploy with CI/CD pipeline and infrastructure as code

This multi-layer architecture ensures proper separation of concerns, maintainability, and testability while following ABP.io best practices and clean architecture principles. The system is designed to scale from the current Books foundation to a comprehensive AI interview preparation platform! 🚀