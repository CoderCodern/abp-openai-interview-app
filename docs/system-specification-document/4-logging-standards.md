# Logging Standards

## 🎯 **Overview**

Use **Serilog** for structured logging with consistent context and proper log levels.

## **Log Levels**

| Level | Usage | Required Context |
|-------|-------|------------------|
| **Debug** | Development flow | `Method`, `Parameters` |
| **Information** | Normal operations | `UserId`, `SessionId`, `Duration` |
| **Warning** | Recoverable issues | `RetryAttempt`, `Reason` |
| **Error** | Failures | `UserId`, `Exception`, `CorrelationId` |
| **Critical** | System failures | `ServiceName`, `ErrorCode` |

## **Logging Patterns**

### **Job Description Analysis:**
```csharp
Logger.LogInformation("JD analysis started: {JobId} for user {UserId}", jobId, CurrentUser.Id);
Logger.LogInformation("JD analysis completed: {JobId} in {Duration}ms, {TokensUsed} tokens", jobId, duration, tokensUsed);
Logger.LogError(ex, "JD analysis failed: {JobId} for user {UserId}", jobId, CurrentUser.Id);
```

### **Interview Sessions:**
```csharp
Logger.LogInformation("Interview started: {SessionId}, type: {InterviewType}", sessionId, interviewType);
Logger.LogInformation("Answer processed: {SessionId}, score: {Score:F2}", sessionId, score);
Logger.LogWarning("Session expired: {SessionId}, user: {UserId}", sessionId, userId);
```

### **CV Generation:**
```csharp
Logger.LogInformation("CV generation started: {CVId}, template: {TemplateName}", cvId, templateName);
Logger.LogInformation("CV generated: {CVId} in {Duration}ms, {TokensUsed} tokens", cvId, duration, tokensUsed);
Logger.LogError(ex, "CV generation failed: {CVId} for user {UserId}", cvId, userId);
```

### **AI Service Calls:**
```csharp
Logger.LogDebug("OpenAI request: {RequestId}, model: {Model}", requestId, model);
Logger.LogInformation("OpenAI completed: {RequestId}, {TokensUsed} tokens, ${Cost:F4}", requestId, tokensUsed, cost);
Logger.LogWarning("OpenAI rate limit: {RequestId}, retry in {DelaySeconds}s", requestId, delaySeconds);
```

### **Background Jobs:**
```csharp
Logger.LogInformation("Job started: {JobId}, type: {JobType}", jobId, jobType);
Logger.LogInformation("Job completed: {JobId}, processed: {ProcessedCount}/{TotalCount}", jobId, processedCount, totalCount);
```

## **Required Context Fields**

### **Business Operations:**
- **Job Description**: `JobId`, `UserId`, `Duration`, `TokensUsed`, `Cost`
- **Interview**: `SessionId`, `UserId`, `InterviewType`, `Duration`, `Score`
- **CV Generation**: `CVId`, `UserId`, `TemplateName`, `Duration`, `TokensUsed`
- **Chat**: `SessionId`, `UserId`, `MessageLength`, `TokensUsed`

### **Technical Operations:**
- **API Requests**: `Method`, `Path`, `StatusCode`, `Duration`, `UserId`
- **AI Calls**: `RequestId`, `Model`, `TokensUsed`, `Cost`, `Duration`
- **Background Jobs**: `JobId`, `JobType`, `ProcessedCount`, `FailedCount`
- **Errors**: `ErrorCode`, `UserId`, `CorrelationId`, `Exception`

## ⚙️ **Configuration**

### **appsettings.json:**
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "AbpResearchWorker": "Debug"
      }
    },
    "WriteTo": [
      { "Name": "Console" },
      { "Name": "File", "Args": { "path": "Logs/interview-prep-.log", "rollingInterval": "Day" } },
      { "Name": "Seq", "Args": { "serverUrl": "http://localhost:5341" } }
    ],
    "Enrich": ["FromLogContext", "WithCorrelationId"],
    "Properties": {
      "ServiceName": "AI-Interview-Prep-System"
    }
  }
}
```

## 📋 **Best Practices**

### **✅ DO:**
- Use structured logging: `Logger.LogInformation("Processing {SessionId}", sessionId)`
- Include correlation IDs for request tracing
- Log operation start/completion with timing
- Use appropriate log levels
- Include business context (UserId, SessionId, TokensUsed, Cost)

### **❌ DON'T:**
- Log sensitive information (interview answers, API keys)
- Use string concatenation: `"Processing " + sessionId`
- Log excessively at Debug level in production
- Use generic error messages without context
- Log large payloads or AI prompts in production

### **🔒 Security & Privacy:**
```csharp
// ✅ Good: Log metadata without sensitive content
Logger.LogInformation("Answer submitted: {SessionId}, length: {AnswerLength}", sessionId, answerLength);

// ❌ Bad: Logging sensitive content
Logger.LogInformation("Answer submitted: {SessionId}, content: {Answer}", sessionId, answerContent);
```

## 🔍 **Monitoring**

### **Key Metrics:**
- Request duration & AI response times
- Sessions completed & tokens consumed
- Error rates & retry attempts
- Cache hit rates & background job success

### **Alerts:**
- Error rate > 5% in 5-minute window
- AI cost exceeds daily budget
- Average response time > 10 seconds
- Background job failure rate > 10%

### **Dashboard Queries:**
```kusto
// Daily AI costs
logs | where TimeGenerated > ago(1d) | summarize sum(Cost) by bin(TimeGenerated, 1h)

// Interview completion rates  
logs | where Message contains "Interview" | summarize Started = countif(Message contains "started"), Completed = countif(Message contains "completed")

// Performance by operation
logs | summarize avg(Duration) by Operation, bin(TimeGenerated, 1h)
```