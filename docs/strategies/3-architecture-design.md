# 🏗️ Step 3 — System Architecture Design (C4 Model)

Define **how** the AI Interview Preparation System is structured using the C4 model.

---

## 🎯 Goal
Map the functional design into concrete system architecture before API design and implementation.

---

## 📐 C4 Model Overview

The C4 model provides four levels of architectural abstraction:
- **C1**: System Context - High-level view of the system and its users
- **C2**: Container - Major technical building blocks  
- **C3**: Component - Internal structure of containers
- **C4**: Code - Class-level interactions (optional)

---

## 🌐 C1: System Context Diagram

```mermaid
graph TB
    User[👤 Job Candidate<br/>Uses system for interview prep]
    Recruiter[👤 HR/Recruiter<br/>May use insights & reports]
    
    InterviewSystem[🎯 AI Interview Prep System<br/>ABP + Angular + OpenAI]
    
    OpenAI[🤖 OpenAI Services<br/>GPT-4, Whisper, TTS]
    JobBoards[💼 Job Boards API<br/>LinkedIn, Indeed, etc.]
    WebRTC[📹 WebRTC Services<br/>Video/Audio streaming]
    EmailService[📧 Email Service<br/>SMTP/SendGrid]
    FileStorage[📁 File Storage<br/>Azure Blob/AWS S3]
    
    User --> InterviewSystem
    Recruiter --> InterviewSystem
    InterviewSystem --> OpenAI
    InterviewSystem --> JobBoards
    InterviewSystem --> WebRTC
    InterviewSystem --> EmailService
    InterviewSystem --> FileStorage
    
    classDef userStyle fill:#4fc3f7,stroke:#01579b,stroke-width:2px,color:#000
    classDef systemStyle fill:#ab47bc,stroke:#4a148c,stroke-width:3px,color:#fff
    classDef externalStyle fill:#ffb74d,stroke:#e65100,stroke-width:2px,color:#000
    
    class User,Recruiter userStyle
    class InterviewSystem systemStyle
    class OpenAI,JobBoards,WebRTC,EmailService,FileStorage externalStyle
```

### System Context Description
- **Primary Users**: Job candidates preparing for interviews
- **Secondary Users**: HR/Recruiters accessing analytics
- **Core System**: ABP-based interview preparation platform
- **External Dependencies**: AI services, job data, real-time communication

---

## 🏢 C2: Container Diagram

```mermaid
graph TB
    subgraph "User Devices"
        Browser[🌐 Web Browser<br/>Angular 20 SPA]
        Mobile[📱 Mobile Browser<br/>PWA Support]
    end
    
    subgraph "ABP Interview System"
        API[🔌 API Gateway<br/>ABP HttpApi.Host<br/>ASP.NET Core 9]
        App[⚙️ Application Layer<br/>Business Logic<br/>ABP Application Services]
        Domain[🏛️ Domain Layer<br/>Business Rules<br/>Domain Services]
        Infrastructure[🗃️ Infrastructure<br/>EF Core + Repositories]
        
        BackgroundJobs[⏱️ Background Services<br/>ABP Background Jobs<br/>AI Processing Queue]
        
        AIService[🤖 AI Integration Layer<br/>OpenAI SDK<br/>Custom AI Services]
        
        FileManager[📁 File Management<br/>CV Upload/Storage<br/>ABP Blob Storage]
        
        RealtimeHub[📡 SignalR Hub<br/>Real-time Communication<br/>Interview Sessions]
    end
    
    subgraph "Data Layer"
        MainDB[(🗄️ Main Database<br/>SQL Server<br/>Multi-tenant)]
        CacheDB[(⚡ Redis Cache<br/>Session Data<br/>Performance)]
        VectorDB[(🧠 Vector Database<br/>Pinecone/Qdrant<br/>Knowledge Base)]
    end
    
    subgraph "External Services"
        OpenAI[🤖 OpenAI API<br/>GPT-4 Turbo<br/>Whisper STT<br/>TTS]
        BlobStorage[☁️ Cloud Storage<br/>Azure Blob<br/>File Assets]
        EmailSMTP[📧 Email Provider<br/>SendGrid/SMTP<br/>Notifications]
        JobAPIs[💼 Job APIs<br/>LinkedIn API<br/>Indeed API]
    end
    
    Browser --> API
    Mobile --> API
    
    API --> App
    App --> Domain
    App --> Infrastructure
    App --> AIService
    App --> FileManager
    API --> RealtimeHub
    
    BackgroundJobs --> AIService
    BackgroundJobs --> Domain
    
    Infrastructure --> MainDB
    App --> CacheDB
    AIService --> VectorDB
    
    AIService --> OpenAI
    FileManager --> BlobStorage
    App --> EmailSMTP
    AIService --> JobAPIs
    
    classDef containerStyle fill:#66bb6a,stroke:#2e7d32,stroke-width:2px,color:#fff
    classDef dataStyle fill:#42a5f5,stroke:#1565c0,stroke-width:2px,color:#fff
    classDef externalStyle fill:#ffa726,stroke:#ef6c00,stroke-width:2px,color:#000
    
    class API,App,Domain,Infrastructure,BackgroundJobs,AIService,FileManager,RealtimeHub containerStyle
    class MainDB,CacheDB,VectorDB dataStyle
    class OpenAI,BlobStorage,EmailSMTP,JobAPIs externalStyle
```

### Container Responsibilities

| Container | Technology | Purpose |
|-----------|------------|---------|
| **Angular SPA** | Angular 20, ABP NG | User interface, real-time UI updates |
| **API Gateway** | ASP.NET Core, ABP HttpApi | REST endpoints, authentication, CORS |
| **Application Layer** | ABP Application Services | Business use cases, orchestration |
| **Domain Layer** | .NET 9, ABP Domain | Core business logic, entities, rules |
| **Infrastructure** | EF Core, ABP Repositories | Data persistence, external integrations |
| **AI Integration** | OpenAI SDK, Custom Services | AI processing, GPT interactions |
| **Background Jobs** | ABP Background Jobs | Async processing, AI tasks |
| **SignalR Hub** | ASP.NET SignalR | Real-time interview sessions |
| **File Manager** | ABP Blob Storage | CV upload, file management |

---

## 🧩 C3: Component Diagram - Application Layer

```mermaid
graph TB
    subgraph "ABP Application Layer Components"
        
        subgraph "User Management"
            UserAppService[👤 UserAppService<br/>Profile management]
            AuthService[🔐 AuthService<br/>JWT & OAuth]
        end
        
        subgraph "Job Description Module"
            JDProcessor[📋 JDProcessorService<br/>Parse & analyze JDs]
            JDAppService[📋 JDAppService<br/>JD CRUD operations]
        end
        
        subgraph "Company Intelligence"
            CompanyService[🏢 CompanyInsightService<br/>Company research & analysis]
            CompanyAppService[🏢 CompanyAppService<br/>Company data management]
        end
        
        subgraph "CV Builder Module"
            CVGenerator[📄 CVGeneratorService<br/>AI-powered CV creation]
            CVAppService[📄 CVAppService<br/>CV management & export]
        end
        
        subgraph "Interview Simulation"
            InterviewEngine[🎭 InterviewEngineService<br/>Conduct AI interviews]
            InterviewAppService[🎭 InterviewAppService<br/>Session management]
            FeedbackService[📊 FeedbackService<br/>Performance analysis]
        end
        
        subgraph "Chat Assistant"
            ChatService[💬 ChatAssistantService<br/>AI-powered guidance]
            ChatAppService[💬 ChatAppService<br/>Chat session management]
        end
        
        subgraph "Analytics & Reports"
            AnalyticsService[📈 AnalyticsService<br/>Performance tracking]
            ReportService[📋 ReportService<br/>Generate insights]
        end
        
        subgraph "Legacy (Current)"
            BookService[📚 BookAppService<br/>Current implementation<br/>To be replaced/extended]
        end
    end
    
    subgraph "Cross-Cutting Services"
        AIOrchestrator[🤖 AIOrchestrationService<br/>Coordinate AI operations]
        NotificationService[🔔 NotificationService<br/>Email/Push notifications]
        AuditService[📝 AuditService<br/>Activity logging]
    end
    
    subgraph "External Integration"
        OpenAIConnector[🤖 OpenAIConnector<br/>GPT, Whisper, TTS APIs]
        JobBoardConnector[💼 JobBoardConnector<br/>External job data]
        FileStorageConnector[📁 FileStorageConnector<br/>Cloud file operations]
    end
    
    %% Dependencies
    JDProcessor --> AIOrchestrator
    CompanyService --> AIOrchestrator
    CVGenerator --> AIOrchestrator
    InterviewEngine --> AIOrchestrator
    ChatService --> AIOrchestrator
    
    AIOrchestrator --> OpenAIConnector
    CompanyService --> JobBoardConnector
    CVAppService --> FileStorageConnector
    
    InterviewAppService --> NotificationService
    UserAppService --> AuditService
    
    classDef moduleStyle fill:#66bb6a,stroke:#2e7d32,stroke-width:2px,color:#fff
    classDef crossCuttingStyle fill:#ffca28,stroke:#f57f17,stroke-width:2px,color:#000
    classDef externalStyle fill:#ef5350,stroke:#c62828,stroke-width:2px,color:#fff
    classDef legacyStyle fill:#9e9e9e,stroke:#424242,stroke-width:2px,color:#fff
    
    class UserAppService,AuthService,JDProcessor,JDAppService,CompanyService,CompanyAppService,CVGenerator,CVAppService,InterviewEngine,InterviewAppService,FeedbackService,ChatService,ChatAppService,AnalyticsService,ReportService moduleStyle
    class AIOrchestrator,NotificationService,AuditService crossCuttingStyle
    class OpenAIConnector,JobBoardConnector,FileStorageConnector externalStyle
    class BookService legacyStyle
```

### Component Interactions

#### Core Workflow Example: JD to Interview Flow
```mermaid
sequenceDiagram
    participant User
    participant JDAppService
    participant JDProcessor
    participant CompanyService
    participant InterviewEngine
    participant AIOrchestrator
    participant OpenAI
    
    User->>JDAppService: Upload Job Description
    JDAppService->>JDProcessor: Process JD Text
    JDProcessor->>AIOrchestrator: Extract Skills & Requirements
    AIOrchestrator->>OpenAI: Analyze JD Content
    OpenAI-->>AIOrchestrator: Parsed Data
    AIOrchestrator-->>JDProcessor: Structured JD Info
    
    JDProcessor->>CompanyService: Get Company Insights
    CompanyService->>AIOrchestrator: Research Company
    AIOrchestrator->>OpenAI: Generate Company Profile
    OpenAI-->>CompanyService: Company Insights
    
    User->>InterviewEngine: Start Interview Session
    InterviewEngine->>AIOrchestrator: Generate Questions
    AIOrchestrator->>OpenAI: Create Interview Questions
    OpenAI-->>InterviewEngine: Tailored Questions
    InterviewEngine-->>User: Begin Interview
```

---

## 🗄️ C3: Component Diagram - Domain Layer

```mermaid
graph TB
    subgraph "Domain Entities (Aggregates)"
        
        subgraph "User Context"
            User[👤 User<br/>Identity + Profile]
            UserProfile[👤 UserProfile<br/>Skills, Experience]
        end
        
        subgraph "Job Context"
            JobDescription[📋 JobDescription<br/>JD content & analysis]
            Company[🏢 Company<br/>Company information]
            JobRequirement[📋 JobRequirement<br/>Skills & qualifications]
        end
        
        subgraph "CV Context"
            CVDocument[📄 CVDocument<br/>Generated CVs]
            CVTemplate[📄 CVTemplate<br/>CV layouts & styles]
            CVSection[📄 CVSection<br/>CV content sections]
        end
        
        subgraph "Interview Context"
            InterviewSession[🎭 InterviewSession<br/>Interview data & state]
            Question[❓ Question<br/>Interview questions]
            Answer[💬 Answer<br/>User responses]
            Feedback[📊 Feedback<br/>Performance analysis]
        end
        
        subgraph "Chat Context"
            ChatSession[💬 ChatSession<br/>Assistant conversations]
            ChatMessage[💬 ChatMessage<br/>Individual messages]
        end
        
        subgraph "Legacy Context"
            Book[📚 Book<br/>Current entity<br/>To be evolved]
        end
    end
    
    subgraph "Domain Services"
        InterviewDomainService[🎭 InterviewDomainService<br/>Interview business rules]
        CVGenerationDomainService[📄 CVGenerationDomainService<br/>CV creation logic]
        CompanyResearchDomainService[🏢 CompanyResearchDomainService<br/>Company analysis logic]
        ScoringDomainService[📊 ScoringDomainService<br/>Performance calculation]
    end
    
    subgraph "Value Objects"
        Score[📊 Score<br/>Interview performance]
        Skill[🔧 Skill<br/>Technical/soft skills]
        ContactInfo[📧 ContactInfo<br/>User contact details]
        DateRange[📅 DateRange<br/>Experience periods]
    end
    
    subgraph "Domain Events"
        InterviewCompleted[📢 InterviewCompletedEvent]
        CVGenerated[📢 CVGeneratedEvent]
        JDAnalyzed[📢 JDAnalyzedEvent]
        FeedbackGenerated[📢 FeedbackGeneratedEvent]
    end
    
    %% Relationships
    User --> UserProfile
    JobDescription --> Company
    JobDescription --> JobRequirement
    InterviewSession --> Question
    InterviewSession --> Answer
    InterviewSession --> User
    Answer --> Feedback
    CVDocument --> User
    CVDocument --> CVSection
    ChatSession --> User
    ChatSession --> ChatMessage
    
    InterviewSession --> InterviewDomainService
    CVDocument --> CVGenerationDomainService
    Company --> CompanyResearchDomainService
    Feedback --> ScoringDomainService
    
    InterviewSession -.-> InterviewCompleted
    CVDocument -.-> CVGenerated
    JobDescription -.-> JDAnalyzed
    Feedback -.-> FeedbackGenerated
    
    classDef entityStyle fill:#66bb6a,stroke:#2e7d32,stroke-width:2px,color:#fff
    classDef serviceStyle fill:#ffca28,stroke:#f57f17,stroke-width:2px,color:#000
    classDef valueStyle fill:#42a5f5,stroke:#1565c0,stroke-width:2px,color:#fff
    classDef eventStyle fill:#ef5350,stroke:#c62828,stroke-width:2px,color:#fff
    classDef legacyStyle fill:#9e9e9e,stroke:#424242,stroke-width:2px,color:#fff
    
    class User,UserProfile,JobDescription,Company,JobRequirement,CVDocument,CVTemplate,CVSection,InterviewSession,Question,Answer,Feedback,ChatSession,ChatMessage entityStyle
    class InterviewDomainService,CVGenerationDomainService,CompanyResearchDomainService,ScoringDomainService serviceStyle
    class Score,Skill,ContactInfo,DateRange valueStyle
    class InterviewCompleted,CVGenerated,JDAnalyzed,FeedbackGenerated eventStyle
    class Book legacyStyle
```

---

## 🔌 C3: Component Diagram - Infrastructure Layer

```mermaid
graph TB
    subgraph "Infrastructure Layer"
        
        subgraph "Data Persistence"
            EFDbContext[🗄️ AbpResearchWorkerDbContext<br/>Entity Framework Core]
            Repositories[📦 Generic Repositories<br/>ABP Repository Pattern]
            UnitOfWork[⚡ Unit of Work<br/>Transaction management]
        end
        
        subgraph "AI Integration Infrastructure"
            OpenAIClient[🤖 OpenAI Client<br/>HTTP client wrapper]
            WhisperService[🎤 Whisper Integration<br/>Speech-to-Text]
            TTSService[🔊 TTS Integration<br/>Text-to-Speech]
            EmbeddingService[🧠 Embedding Service<br/>Vector generation]
        end
        
        subgraph "External APIs"
            LinkedInConnector[💼 LinkedIn API Client<br/>Job data integration]
            IndeedConnector[💼 Indeed API Client<br/>Job search API]
            EmailProvider[📧 Email Service Provider<br/>SendGrid/SMTP]
        end
        
        subgraph "File & Media"
            BlobStorageProvider[☁️ Blob Storage Provider<br/>Azure/AWS integration]
            FileUploadHandler[📁 File Upload Handler<br/>CV & document processing]
            MediaProcessor[🎬 Media Processor<br/>Audio/Video handling]
        end
        
        subgraph "Caching & Performance"
            RedisCacheProvider[⚡ Redis Cache Provider<br/>Session & performance cache]
            MemoryCacheProvider[💾 Memory Cache Provider<br/>Local caching]
        end
        
        subgraph "Background Processing"
            BackgroundJobManager[⏱️ Background Job Manager<br/>ABP Hangfire integration]
            QueueProcessor[📬 Queue Processor<br/>AI task processing]
        end
        
        subgraph "Real-time Communication"
            SignalRProvider[📡 SignalR Provider<br/>Real-time updates]
            WebRTCHandler[📹 WebRTC Handler<br/>Video interview streaming]
        end
        
        subgraph "Security & Auth"
            JWTProvider[🔐 JWT Token Provider<br/>Authentication tokens]
            PermissionChecker[🛡️ Permission Checker<br/>Authorization logic]
            EncryptionService[🔒 Encryption Service<br/>Data protection]
        end
    end
    
    %% Dependencies
    Repositories --> EFDbContext
    UnitOfWork --> EFDbContext
    
    OpenAIClient --> WhisperService
    OpenAIClient --> TTSService
    OpenAIClient --> EmbeddingService
    
    BackgroundJobManager --> QueueProcessor
    QueueProcessor --> OpenAIClient
    
    SignalRProvider --> WebRTCHandler
    
    classDef dataStyle fill:#42a5f5,stroke:#1565c0,stroke-width:2px,color:#fff
    classDef aiStyle fill:#ab47bc,stroke:#4a148c,stroke-width:2px,color:#fff
    classDef externalStyle fill:#ffa726,stroke:#ef6c00,stroke-width:2px,color:#000
    classDef fileStyle fill:#66bb6a,stroke:#2e7d32,stroke-width:2px,color:#fff
    classDef cacheStyle fill:#ffca28,stroke:#f57f17,stroke-width:2px,color:#000
    classDef backgroundStyle fill:#ec407a,stroke:#ad1457,stroke-width:2px,color:#fff
    classDef realtimeStyle fill:#26a69a,stroke:#00695c,stroke-width:2px,color:#fff
    classDef securityStyle fill:#ef5350,stroke:#c62828,stroke-width:2px,color:#fff
    
    class EFDbContext,Repositories,UnitOfWork dataStyle
    class OpenAIClient,WhisperService,TTSService,EmbeddingService aiStyle
    class LinkedInConnector,IndeedConnector,EmailProvider externalStyle
    class BlobStorageProvider,FileUploadHandler,MediaProcessor fileStyle
    class RedisCacheProvider,MemoryCacheProvider cacheStyle
    class BackgroundJobManager,QueueProcessor backgroundStyle
    class SignalRProvider,WebRTCHandler realtimeStyle
    class JWTProvider,PermissionChecker,EncryptionService securityStyle
```

---

## 📊 Data Architecture

### Database Schema Design

```mermaid
erDiagram
    %% User Management
    Users {
        guid Id PK
        string UserName
        string Email
        datetime CreationTime
        guid TenantId FK
    }
    
    UserProfiles {
        guid Id PK
        guid UserId FK
        string FullName
        string Phone
        string Summary
        json Skills
        json Experience
    }
    
    %% Job & Company Context
    Companies {
        guid Id PK
        string Name
        string Industry
        string Description
        string Website
        json InsightData
    }
    
    JobDescriptions {
        guid Id PK
        guid CompanyId FK
        guid UserId FK
        string Title
        text Content
        json ParsedData
        json Requirements
        datetime CreatedAt
    }
    
    %% Interview System
    InterviewSessions {
        guid Id PK
        guid UserId FK
        guid JobDescriptionId FK
        string SessionType
        string Status
        datetime StartTime
        datetime EndTime
        json Configuration
    }
    
    Questions {
        guid Id PK
        guid InterviewSessionId FK
        string QuestionText
        string QuestionType
        int OrderIndex
        json Metadata
    }
    
    Answers {
        guid Id PK
        guid QuestionId FK
        text AnswerText
        string AudioUrl
        datetime AnsweredAt
        json Analysis
    }
    
    InterviewFeedback {
        guid Id PK
        guid InterviewSessionId FK
        decimal OverallScore
        json DetailedScores
        text Recommendations
        datetime GeneratedAt
    }
    
    %% CV System
    CVDocuments {
        guid Id PK
        guid UserId FK
        guid JobDescriptionId FK
        string TemplateName
        string Status
        string FileUrl
        json Content
        datetime GeneratedAt
    }
    
    %% Chat System
    ChatSessions {
        guid Id PK
        guid UserId FK
        string SessionType
        datetime StartTime
        datetime LastActivity
    }
    
    ChatMessages {
        guid Id PK
        guid ChatSessionId FK
        string Role
        text Content
        datetime Timestamp
        json Metadata
    }
    
    %% Legacy (Current)
    Books {
        guid Id PK
        string Name
        int Type
        datetime PublishDate
        float Price
        datetime CreationTime
        guid CreatorId
    }
    
    %% Relationships
    Users ||--|| UserProfiles : "has"
    Users ||--o{ JobDescriptions : "creates"
    Companies ||--o{ JobDescriptions : "for"
    Users ||--o{ InterviewSessions : "participates"
    JobDescriptions ||--o{ InterviewSessions : "based on"
    InterviewSessions ||--o{ Questions : "contains"
    Questions ||--|| Answers : "has answer"
    InterviewSessions ||--|| InterviewFeedback : "generates"
    Users ||--o{ CVDocuments : "owns"
    JobDescriptions ||--o{ CVDocuments : "tailored for"
    Users ||--o{ ChatSessions : "initiates"
    ChatSessions ||--o{ ChatMessages : "contains"
```

---

## 🔄 Migration Strategy from Books to Interview System

### Phase 1: Extend Current System
```csharp
// Keep existing Book system as reference
public class Book : AuditedAggregateRoot<Guid>
{
    // Current implementation remains
}

// Add new entities alongside
public class JobDescription : AuditedAggregateRoot<Guid>
{
    public string Title { get; set; }
    public string Content { get; set; }
    // New AI-powered functionality
}
```

### Phase 2: Gradual Replacement
- Implement new modules (JD, Company, Interview)
- Maintain Books as a reference implementation
- Migrate UI components progressively

### Phase 3: Complete Transformation
- Remove or repurpose Books system
- Full AI interview system operational

---

## 🛡️ Security Architecture

```mermaid
graph TD
    subgraph "Security Layers"
        A[🌐 API Gateway Security<br/>Rate Limiting, CORS, HTTPS]
        B[🔐 Authentication<br/>OpenIddict, JWT, OAuth 2.0]
        C[🛡️ Authorization<br/>ABP Permissions, Role-based]
        D[🔒 Data Protection<br/>Encryption at Rest & Transit]
        E[📝 Audit & Compliance<br/>ABP Audit Logging]
        F[🤖 AI Security<br/>Input Sanitization, Output Filtering]
    end
    
    A --> B
    B --> C
    C --> D
    D --> E
    E --> F
```

---

## 📈 Scalability Considerations

### Horizontal Scaling Points
1. **API Layer**: Load-balanced ABP HttpApi.Host instances
2. **Background Jobs**: Distributed job processing with Redis
3. **AI Services**: Queue-based AI request processing
4. **Database**: Read replicas for analytics queries
5. **File Storage**: CDN for CV and media files

### Performance Optimizations
1. **Caching Strategy**: Redis for session data, Memory cache for static data
2. **Database Indexing**: Optimized queries for interview search
3. **AI Response Caching**: Cache common AI responses
4. **CDN Integration**: Fast file delivery

---

## ✅ Implementation Priorities

### Priority 1 (Foundation)
- [ ] Complete Domain Models for Interview entities
- [ ] Implement AI Integration Infrastructure
- [ ] Set up Background Job Processing

### Priority 2 (Core Features)
- [ ] Job Description Processing Service
- [ ] Company Intelligence Service
- [ ] Basic Interview Engine

### Priority 3 (Advanced Features)
- [ ] Real-time Interview Sessions (WebRTC)
- [ ] Advanced CV Generation
- [ ] Analytics Dashboard

---

## 🔄 Next Steps

1. **Complete this Architecture Document** ✅
2. **Design API Specifications** (Step 4)
3. **Implement Core Domain Models**
4. **Set up AI Integration Layer**
5. **Begin Interview Engine Development**

---

**Architecture Version**: 1.0  
**Last Updated**: October 31, 2025  
**Next Review**: After API design completion