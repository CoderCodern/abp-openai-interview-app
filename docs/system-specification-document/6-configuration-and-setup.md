# Configuration & Setup

## **Overview**

The AI Interview Preparation System uses configuration files, environment variables, and secrets management for different deployment environments. All sensitive data is externalized and properly secured.

## **Configuration Files Structure**

```
AbpResearchWorker.HttpApi.Host/
├── appsettings.json                    // Base configuration
├── appsettings.Development.json        // Development overrides
├── appsettings.Staging.json           // Staging overrides
├── appsettings.Production.json        // Production overrides
└── secrets.json                       // Local development secrets
```

## **Base Configuration (appsettings.json)**

```json
{
  "App": {
    "Name": "AI Interview Preparation System",
    "Version": "1.0.0",
    "Environment": "Production",
    "SelfUrl": "https://localhost:44397",
    "AngularUrl": "http://localhost:4200"
  },
  "ConnectionStrings": {
    "Default": "Server=(LocalDb)\\MSSQLLocalDB;Database=AbpResearchWorker;Trusted_Connection=True;MultipleActiveResultSets=true",
    "Redis": "localhost:6379"
  },
  "OpenAI": {
    "BaseUrl": "https://api.openai.com/v1",
    "Model": "gpt-4",
    "MaxTokens": 4000,
    "Temperature": 0.3,
    "TopP": 1.0,
    "FrequencyPenalty": 0.0,
    "PresencePenalty": 0.0,
    "RequestTimeout": 45000,
    "RetryAttempts": 3,
    "RetryDelay": 2000
  },
  "Interview": {
    "MaxSessionDuration": 3600,
    "DefaultQuestionCount": 15,
    "MaxConcurrentSessions": 5,
    "SessionTimeoutMinutes": 90,
    "AutoSaveInterval": 30,
    "MaxAnswerLength": 5000
  },
  "CV": {
    "MaxGenerationTime": 120,
    "SupportedFormats": ["PDF", "DOCX"],
    "MaxFileSize": 5242880,
    "Templates": ["ATS-Optimized", "Creative", "Executive", "Technical"]
  },
  "RateLimiting": {
    "RequestsPerMinute": 60,
    "RequestsPerHour": 500,
    "RequestsPerDay": 2000,
    "CostLimitPerDay": 25.00,
    "TokenLimitPerRequest": 8000,
    "SessionLimitPerUser": 10
  },
  "WebRTC": {
    "Enabled": true,
    "IceServers": [
      {
        "Urls": ["stun:stun.l.google.com:19302"]
      }
    ],
    "MaxRecordingDuration": 1800,
    "AudioCodec": "opus",
    "VideoCodec": "vp9"
  },
  "Caching": {
    "Provider": "Redis",
    "DefaultExpirationMinutes": 30,
    "JobAnalysisExpirationHours": 12,
    "SessionExpirationMinutes": 120,
    "PerformanceStatsExpirationMinutes": 10
  },
  "BackgroundJobs": {
    "Enabled": true,
    "BatchSize": 5,
    "RetryAttempts": 3,
    "CleanupIntervalHours": 24,
    "JobTimeoutMinutes": 60,
    "MaxConcurrentJobs": 10
  },
  "Monitoring": {
    "Enabled": true,
    "MetricsInterval": 60,
    "HealthCheckInterval": 30,
    "AlertThresholds": {
      "ErrorRatePercent": 3,
      "ResponseTimeMs": 8000,
      "CostPerHour": 5.00,
      "ActiveSessionsLimit": 50
    }
  },
  "Security": {
    "RequireHttps": true,
    "EnableCors": true,
    "AllowedOrigins": ["http://localhost:4200"],
    "JwtExpiration": 1440,
    "RefreshTokenExpiration": 10080
  }
}
```

## **Development Environment (appsettings.Development.json)**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "AbpResearchWorker": "Debug",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  },
  "App": {
    "Environment": "Development",
    "SelfUrl": "https://localhost:44397",
    "AngularUrl": "http://localhost:4200"
  },
  "ConnectionStrings": {
    "Default": "Server=(LocalDb)\\MSSQLLocalDB;Database=AbpResearchWorker_Dev;Trusted_Connection=True;MultipleActiveResultSets=true",
    "Redis": "localhost:6379"
  },
  "OpenAI": {
    "Model": "gpt-3.5-turbo",
    "MaxTokens": 2000,
    "Temperature": 0.5,
    "RequestTimeout": 60000
  },
  "Interview": {
    "MaxSessionDuration": 7200,
    "DefaultQuestionCount": 5,
    "SessionTimeoutMinutes": 30
  },
  "RateLimiting": {
    "RequestsPerMinute": 200,
    "RequestsPerHour": 2000,
    "CostLimitPerDay": 5.00,
    "SessionLimitPerUser": 20
  },
  "Caching": {
    "DefaultExpirationMinutes": 5,
    "JobAnalysisExpirationHours": 1
  },
  "BackgroundJobs": {
    "BatchSize": 2,
    "MaxConcurrentJobs": 3
  },
  "Monitoring": {
    "Enabled": false
  },
  "Security": {
    "RequireHttps": false,
    "JwtExpiration": 60
  }
}
```

## **Staging Environment (appsettings.Staging.json)**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "AbpResearchWorker": "Debug",
      "Microsoft": "Warning"
    }
  },
  "App": {
    "Environment": "Staging"
  },
  "OpenAI": {
    "Model": "gpt-4",
    "MaxTokens": 4000,
    "Temperature": 0.3
  },
  "Interview": {
    "MaxConcurrentSessions": 10
  },
  "RateLimiting": {
    "RequestsPerMinute": 80,
    "RequestsPerHour": 800,
    "CostLimitPerDay": 15.00
  },
  "WebRTC": {
    "Enabled": true
  },
  "Monitoring": {
    "Enabled": true,
    "AlertThresholds": {
      "ErrorRatePercent": 5,
      "ResponseTimeMs": 12000,
      "CostPerHour": 3.00
    }
  }
}
```

## **Production Environment (appsettings.Production.json)**

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  },
  "App": {
    "Environment": "Production"
  },
  "OpenAI": {
    "Model": "gpt-4",
    "MaxTokens": 4000,
    "RequestTimeout": 30000
  },
  "Interview": {
    "MaxConcurrentSessions": 20,
    "SessionTimeoutMinutes": 60
  },
  "RateLimiting": {
    "RequestsPerMinute": 60,
    "RequestsPerHour": 500,
    "CostLimitPerDay": 50.00,
    "SessionLimitPerUser": 15
  },
  "WebRTC": {
    "Enabled": true
  },
  "Monitoring": {
    "Enabled": true,
    "AlertThresholds": {
      "ErrorRatePercent": 2,
      "ResponseTimeMs": 5000,
      "CostPerHour": 8.00,
      "ActiveSessionsLimit": 100
    }
  },
  "Security": {
    "RequireHttps": true,
    "EnableCors": true
  }
}
```

## **Environment Variables**

### **Required Variables:**
```bash
# OpenAI Configuration
OPENAI_API_KEY=sk-your-openai-api-key-here
OPENAI_ORGANIZATION_ID=org-your-organization-id

# Database
DATABASE_CONNECTION_STRING="Server=prod-db;Database=AbpResearchWorker;Uid=app_user;Pwd=secure_password;"

# Redis Cache  
REDIS_CONNECTION_STRING="prod-redis:6379,password=redis_password"

# Application Secrets
JWT_SECRET_KEY=your-super-secure-jwt-secret-key-here
ENCRYPTION_KEY=your-encryption-key-for-sensitive-data

# ABP Framework
ABP_LICENSE_CODE=your-abp-license-code-here

# Monitoring & Alerting
SEQ_API_KEY=your-seq-logging-api-key
APPLICATIONINSIGHTS_CONNECTION_STRING=InstrumentationKey=your-app-insights-key

# WebRTC Services
WEBRTC_TURN_SERVER_URL=turn:your-turn-server.com:3478
WEBRTC_TURN_USERNAME=your-turn-username
WEBRTC_TURN_CREDENTIAL=your-turn-password
```

### **Optional Variables:**
```bash
# Performance Tuning
MAX_CONCURRENT_INTERVIEWS=20
REQUEST_TIMEOUT_SECONDS=45
BACKGROUND_JOB_WORKERS=5

# Feature Flags
ENABLE_WEBRTC=true
ENABLE_CV_GENERATION=true
ENABLE_REAL_TIME_FEEDBACK=true
ENABLE_COST_ALERTS=true

# Development/Debugging
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=https://+:443;http://+:80
ENABLE_SWAGGER=false
ENABLE_AUDIT_LOGGING=true

# Multi-tenancy
DEFAULT_TENANT_NAME=Default
ENABLE_MULTI_TENANCY=true
```

## **Setup Instructions**

### **1. Development Setup**

```bash
# Clone and navigate to project
git clone https://github.com/company/abp-interview-prep-system.git
cd abp-interview-prep-system

# Install .NET dependencies
cd src/AbpResearchWorker.HttpApi.Host
dotnet restore

# Set up user secrets for development
dotnet user-secrets init
dotnet user-secrets set "OpenAI:ApiKey" "sk-your-dev-api-key"
dotnet user-secrets set "ConnectionStrings:Default" "Server=(LocalDb)\\MSSQLLocalDB;Database=AbpResearchWorker_Dev;Trusted_Connection=True"
dotnet user-secrets set "AbpLicenseCode" "your-abp-license-code"

# Run database migrations
cd ../AbpResearchWorker.DbMigrator
dotnet run

# Start the API
cd ../AbpResearchWorker.HttpApi.Host
dotnet run

# In separate terminal, start Angular app
cd ../../angular
npm install
npm start
```

### **2. Docker Setup**

```yaml
# docker-compose.yml
version: '3.8'
services:
  interview-prep-api:
    image: abp-interview-prep:latest
    ports:
      - "5000:80"
      - "5001:443"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - OPENAI_API_KEY=${OPENAI_API_KEY}
      - DATABASE_CONNECTION_STRING=${DATABASE_CONNECTION_STRING}
      - REDIS_CONNECTION_STRING=${REDIS_CONNECTION_STRING}
      - ABP_LICENSE_CODE=${ABP_LICENSE_CODE}
    depends_on:
      - database
      - redis
    
  interview-prep-ui:
    image: abp-interview-prep-angular:latest
    ports:
      - "4200:80"
    environment:
      - API_BASE_URL=http://interview-prep-api:5000
    depends_on:
      - interview-prep-api
    
  database:
    image: mcr.microsoft.com/mssql/server:2022-latest
    environment:
      - SA_PASSWORD=${SA_PASSWORD}
      - ACCEPT_EULA=Y
    ports:
      - "1433:1433"
    volumes:
      - mssql_data:/var/opt/mssql
    
  redis:
    image: redis:7-alpine
    ports:
      - "6379:6379"
    command: redis-server --requirepass ${REDIS_PASSWORD}
    volumes:
      - redis_data:/data

volumes:
  mssql_data:
  redis_data:
```

### **3. Production Deployment**

```bash
# Set environment variables
export ASPNETCORE_ENVIRONMENT=Production
export OPENAI_API_KEY="your-production-api-key"
export DATABASE_CONNECTION_STRING="your-production-db-connection"
export ABP_LICENSE_CODE="your-abp-license-code"

# Build API
cd src/AbpResearchWorker.HttpApi.Host
dotnet publish -c Release -o ./publish

# Build Angular app
cd ../../angular
npm run build:prod

# Run migrations
cd ../src/AbpResearchWorker.DbMigrator
dotnet run

# Start API service
cd ../AbpResearchWorker.HttpApi.Host/publish
dotnet AbpResearchWorker.HttpApi.Host.dll
```

## **Configuration Validation**

### **Startup Configuration Check:**
```csharp
// Program.cs - Configuration validation
public static void Main(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);
    
    // Validate required configuration
    ValidateConfiguration(builder.Configuration);
    
    // ... rest of ABP configuration
}

private static void ValidateConfiguration(IConfiguration config)
{
    var requiredKeys = new[]
    {
        "OpenAI:ApiKey",
        "ConnectionStrings:Default", 
        "ConnectionStrings:Redis",
        "App:SelfUrl"
    };
    
    foreach (var key in requiredKeys)
    {
        if (string.IsNullOrEmpty(config[key]))
        {
            throw new InvalidOperationException($"Required configuration '{key}' is missing");
        }
    }
    
    // Validate OpenAI configuration
    var openAiKey = config["OpenAI:ApiKey"];
    if (!openAiKey?.StartsWith("sk-") == true)
    {
        throw new InvalidOperationException("Invalid OpenAI API key format");
    }
}
```

### **Health Check Configuration:**
```csharp
// Configure health checks in AbpResearchWorkerHttpApiHostModule.cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    var configuration = context.Services.GetConfiguration();
    
    context.Services.AddHealthChecks()
        .AddSqlServer(configuration.GetConnectionString("Default"), name: "database")
        .AddRedis(configuration.GetConnectionString("Redis"), name: "redis")
        .AddUrlGroup(new Uri($"{configuration["OpenAI:BaseUrl"]}/models"), name: "openai")
        .AddCheck<OpenAIHealthCheck>("openai-service")
        .AddCheck<InterviewSessionHealthCheck>("interview-sessions");
}
```

## **Configuration Best Practices**

### **✅ DO:**
- Use ABP's configuration system with IOptions pattern
- Store secrets in Azure Key Vault or AWS Secrets Manager
- Validate configuration at startup with clear error messages
- Use environment-specific overrides appropriately
- Set reasonable defaults for interview timeouts and limits
- Document all OpenAI and WebRTC configuration options

### **❌ DON'T:**
- Store OpenAI API keys in appsettings files
- Use same rate limits across all environments
- Hard-code interview session limits in code
- Ignore WebRTC configuration validation
- Use overly permissive CORS settings in production

## **Configuration Classes**

```csharp
// OpenAIOptions.cs
public class OpenAIOptions
{
    public const string SectionName = "OpenAI";
    
    public string ApiKey { get; set; } = string.Empty;
    public string BaseUrl { get; set; } = "https://api.openai.com/v1";
    public string Model { get; set; } = "gpt-4";
    public int MaxTokens { get; set; } = 4000;
    public double Temperature { get; set; } = 0.3;
    public int RequestTimeout { get; set; } = 45000;
    public int RetryAttempts { get; set; } = 3;
    public int RetryDelay { get; set; } = 2000;
}

// InterviewOptions.cs
public class InterviewOptions
{
    public const string SectionName = "Interview";
    
    public int MaxSessionDuration { get; set; } = 3600;
    public int DefaultQuestionCount { get; set; } = 15;
    public int MaxConcurrentSessions { get; set; } = 5;
    public int SessionTimeoutMinutes { get; set; } = 90;
    public int AutoSaveInterval { get; set; } = 30;
    public int MaxAnswerLength { get; set; } = 5000;
}

// WebRTCOptions.cs
public class WebRTCOptions
{
    public const string SectionName = "WebRTC";
    
    public bool Enabled { get; set; } = true;
    public List<IceServer> IceServers { get; set; } = new();
    public int MaxRecordingDuration { get; set; } = 1800;
    public string AudioCodec { get; set; } = "opus";
    public string VideoCodec { get; set; } = "vp9";
}

// Registration in AbpResearchWorkerHttpApiHostModule.cs
public override void ConfigureServices(ServiceConfigurationContext context)
{
    Configure<OpenAIOptions>(context.Services.GetConfiguration().GetSection(OpenAIOptions.SectionName));
    Configure<InterviewOptions>(context.Services.GetConfiguration().GetSection(InterviewOptions.SectionName));
    Configure<WebRTCOptions>(context.Services.GetConfiguration().GetSection(WebRTCOptions.SectionName));
}
```

This configuration setup ensures secure, maintainable, and environment-appropriate configuration management for the AI Interview Preparation System with ABP Framework! ⚙️