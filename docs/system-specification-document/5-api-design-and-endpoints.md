# API Design & Endpoints

## **API Overview**

The AI Interview Preparation System provides RESTful endpoints for job description analysis, interview sessions, CV generation, and performance tracking. All endpoints follow consistent patterns for requests, responses, and error handling.

**Base URL**: `/api/interview-prep`  
**Authentication**: JWT Bearer token required  
**Content-Type**: `application/json`  

## **Core Endpoints**

### **Job Description Analysis**

#### **POST /api/interview-prep/job-description/analyze**
Analyze job description and generate tailored interview questions.

**Request:**
```json
{
  "jobId": "job-12345-tech",
  "title": "Senior Full Stack Developer",
  "description": "We are seeking a Senior Full Stack Developer with 5+ years experience...",
  "company": "TechCorp Inc",
  "requirements": [
    "React/Angular", "Node.js", "AWS", "Database design"
  ]
}
```

**Response (200):**
```json
{
  "jobId": "job-12345-tech",
  "analysis": {
    "keySkills": ["React", "Node.js", "AWS", "PostgreSQL"],
    "experience": "Senior (5+ years)",
    "difficulty": "Advanced",
    "domain": "Web Development"
  },
  "interviewQuestions": [
    {
      "id": "q-001",
      "category": "Technical",
      "difficulty": "Medium",
      "question": "Explain React hooks and their lifecycle",
      "expectedPoints": ["useState", "useEffect", "custom hooks"]
    }
  ],
  "preparationAreas": [
    "System design patterns",
    "AWS services overview",
    "React performance optimization"
  ],
  "tokensUsed": 1850,
  "cost": 0.0092,
  "processingTimeMs": 2150
}
```

#### **GET /api/interview-prep/job-description/{jobId}**
Retrieve analyzed job description details.

**Response (200):**
```json
{
  "jobId": "job-12345-tech",
  "title": "Senior Full Stack Developer",
  "company": "TechCorp Inc",
  "analysisDate": "2025-11-03T10:30:00Z",
  "status": "Analyzed",
  "questionCount": 25,
  "estimatedInterviewDuration": "45-60 minutes"
}
```

### **Interview Sessions**

#### **POST /api/interview-prep/sessions/start**
Start a new interview session.

**Request:**
```json
{
  "jobId": "job-12345-tech",
  "sessionType": "Technical",
  "duration": 30,
  "questionCount": 10,
  "difficulty": "Advanced"
}
```

**Response (201):**
```json
{
  "sessionId": "session-abc123-def",
  "status": "Started",
  "startTime": "2025-11-03T14:00:00Z",
  "estimatedEndTime": "2025-11-03T14:30:00Z",
  "firstQuestion": {
    "id": "q-001",
    "question": "Describe your experience with microservices architecture",
    "timeLimit": 180,
    "category": "Architecture"
  }
}
```

#### **POST /api/interview-prep/sessions/{sessionId}/answer**
Submit answer for current question.

**Request:**
```json
{
  "questionId": "q-001",
  "answer": "I have worked with microservices for 3 years, implementing service discovery...",
  "recordingUrl": "blob://audio-recording-001",
  "answerDuration": 165
}
```

**Response (200):**
```json
{
  "questionId": "q-001",
  "feedback": {
    "score": 8.5,
    "strengths": ["Clear explanation", "Practical examples"],
    "improvements": ["Mention monitoring strategies", "Discuss failure handling"],
    "detailedFeedback": "Good understanding of microservices. Consider elaborating on..."
  },
  "nextQuestion": {
    "id": "q-002",
    "question": "How do you handle state management in React applications?",
    "timeLimit": 180,
    "category": "Frontend"
  },
  "sessionProgress": {
    "currentQuestion": 2,
    "totalQuestions": 10,
    "averageScore": 8.5,
    "timeRemaining": 1635
  }
}
```

#### **POST /api/interview-prep/sessions/{sessionId}/complete**
Complete the interview session.

**Response (200):**
```json
{
  "sessionId": "session-abc123-def",
  "status": "Completed",
  "completedAt": "2025-11-03T14:28:15Z",
  "summary": {
    "totalQuestions": 10,
    "answeredQuestions": 10,
    "averageScore": 7.8,
    "totalDuration": 1685,
    "strongAreas": ["System Design", "Backend Development"],
    "improvementAreas": ["Frontend Optimization", "Testing Strategies"]
  },
  "reportUrl": "/api/interview-prep/sessions/session-abc123-def/report"
}
```

### **CV Generation**

#### **POST /api/interview-prep/cv/generate**
Generate AI-optimized CV for specific job.

**Request:**
```json
{
  "jobId": "job-12345-tech",
  "template": "ATS-Optimized",
  "personalInfo": {
    "name": "John Smith",
    "email": "john.smith@email.com",
    "phone": "+1-555-0123"
  },
  "experience": [
    {
      "company": "Previous Corp",
      "role": "Software Developer",
      "duration": "2020-2023",
      "achievements": ["Led team of 4", "Reduced load time by 40%"]
    }
  ]
}
```

**Response (202):**
```json
{
  "cvId": "cv-789xyz-abc",
  "status": "Processing",
  "estimatedCompletion": "2025-11-03T14:35:00Z",
  "statusEndpoint": "/api/interview-prep/cv/cv-789xyz-abc/status"
}
```

#### **GET /api/interview-prep/cv/{cvId}/status**
Check CV generation status.

**Response (200):**
```json
{
  "cvId": "cv-789xyz-abc",
  "status": "Completed",
  "completedAt": "2025-11-03T14:33:20Z",
  "optimization": {
    "atsScore": 92,
    "keywordMatch": 85,
    "readabilityScore": 88
  },
  "downloadUrls": {
    "pdf": "/api/interview-prep/cv/cv-789xyz-abc/download/pdf",
    "docx": "/api/interview-prep/cv/cv-789xyz-abc/download/docx"
  },
  "tokensUsed": 2100,
  "cost": 0.0105
}
```

### **Performance Analytics**

#### **GET /api/interview-prep/analytics/performance**
Get user performance analytics across sessions.

**Response (200):**
```json
{
  "period": {
    "start": "2025-10-01T00:00:00Z",
    "end": "2025-11-03T23:59:59Z"
  },
  "performance": {
    "totalSessions": 15,
    "averageScore": 7.6,
    "improvementRate": 12.5,
    "strongCategories": ["Backend", "Databases"],
    "weakCategories": ["System Design", "Leadership"]
  },
  "trends": {
    "scoreProgression": [6.2, 6.8, 7.1, 7.6, 7.8],
    "categoryBreakdown": {
      "Technical": { "sessions": 8, "avgScore": 8.1 },
      "Behavioral": { "sessions": 5, "avgScore": 7.2 },
      "System Design": { "sessions": 2, "avgScore": 6.5 }
    }
  }
}
```

#### **GET /api/interview-prep/analytics/usage**
Get usage statistics and cost breakdown.

**Response (200):**
```json
{
  "period": {
    "start": "2025-10-01T00:00:00Z",
    "end": "2025-11-03T23:59:59Z"
  },
  "usage": {
    "totalSessions": 15,
    "totalQuestions": 180,
    "totalTokens": 45600,
    "totalCost": 22.80,
    "averageCostPerSession": 1.52
  },
  "breakdown": {
    "byFeature": {
      "JobAnalysis": { "count": 8, "cost": 7.36 },
      "InterviewSessions": { "count": 15, "cost": 12.60 },
      "CVGeneration": { "count": 3, "cost": 2.84 }
    },
    "byModel": {
      "gpt-4": { "count": 50, "cost": 15.20 },
      "gpt-3.5-turbo": { "count": 130, "cost": 7.60 }
    }
  }
}
```

### **Health & Monitoring**

#### **GET /api/interview-prep/health**
Service health check endpoint.

**Response (200):**
```json
{
  "status": "Healthy",
  "timestamp": "2025-11-03T15:30:00Z",
  "services": {
    "openai": {
      "status": "Healthy",
      "responseTime": 285,
      "lastCheck": "2025-11-03T15:29:45Z"
    },
    "database": {
      "status": "Healthy",
      "responseTime": 15,
      "lastCheck": "2025-11-03T15:29:58Z"
    },
    "redis": {
      "status": "Healthy", 
      "responseTime": 4,
      "lastCheck": "2025-11-03T15:29:59Z"
    },
    "webrtc": {
      "status": "Healthy",
      "activeConnections": 12,
      "lastCheck": "2025-11-03T15:29:57Z"
    }
  },
  "version": "1.0.0"
}
```

## **Response Patterns**

### **Standard Success Response Structure**
```json
{
  "data": { /* Response payload */ },
  "metadata": {
    "timestamp": "2025-11-03T15:30:00Z",
    "version": "1.0.0",
    "requestId": "req-interview-abc123"
  }
}
```

### **Pagination Pattern**
```json
{
  "data": [ /* Array of items */ ],
  "pagination": {
    "page": 1,
    "pageSize": 20,
    "totalItems": 85,
    "totalPages": 5,
    "hasNextPage": true,
    "hasPreviousPage": false
  }
}
```

### **Filter & Search Pattern**
```json
{
  "data": [ /* Filtered results */ ],
  "filters": {
    "sessionType": "Technical",
    "dateRange": {
      "start": "2025-10-01",
      "end": "2025-11-03"
    },
    "minScore": 7.0,
    "difficulty": "Advanced"
  },
  "totalMatches": 28
}
```

## ⚠️ **Error Handling**

### **Error Response Structure**
All errors follow a consistent format for easy parsing and handling.

```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Request validation failed",
    "details": "Job description exceeds maximum length of 10,000 characters",
    "timestamp": "2025-11-03T15:30:00Z",
    "correlationId": "req-interview-abc123",
    "retryAfter": null
  }
}
```

### **HTTP Status Codes**

| Code | Status | Usage | Retry |
|------|---------|-------|-------|
| **200** | OK | Successful operation | - |
| **201** | Created | Session/resource created | - |
| **202** | Accepted | Async operation started | - |
| **400** | Bad Request | Invalid request data | No |
| **401** | Unauthorized | Missing/invalid authentication | No |
| **403** | Forbidden | Insufficient permissions | No |
| **404** | Not Found | Session/resource not found | No |
| **409** | Conflict | Session already active | No |
| **422** | Unprocessable Entity | Business rule violation | No |
| **429** | Too Many Requests | Rate limit exceeded | Yes |
| **500** | Internal Server Error | Unexpected server error | Yes |
| **502** | Bad Gateway | OpenAI service error | Yes |
| **503** | Service Unavailable | Service temporarily down | Yes |

### **Common Error Codes**

#### **Validation Errors (400)**
```json
{
  "error": {
    "code": "VALIDATION_ERROR",
    "message": "Request validation failed",
    "details": "Job description is required for interview session",
    "validationErrors": [
      {
        "field": "jobId",
        "message": "JobId is required",
        "rejectedValue": null
      }
    ]
  }
}
```

#### **Session Errors (409)**
```json
{
  "error": {
    "code": "SESSION_CONFLICT",
    "message": "Another interview session is already active",
    "details": "Complete current session before starting a new one",
    "activeSessionId": "session-xyz789",
    "suggestedAction": "Complete or abandon current session first"
  }
}
```

#### **Rate Limiting (429)**
```json
{
  "error": {
    "code": "RATE_LIMIT_EXCEEDED",
    "message": "Rate limit exceeded. Please retry after 60 seconds",
    "details": "Current limit: 50 requests per hour per user",
    "retryAfter": 60,
    "resetTime": "2025-11-03T16:30:00Z"
  }
}
```

#### **AI Service Errors (502)**
```json
{
  "error": {
    "code": "OPENAI_SERVICE_ERROR",
    "message": "OpenAI API request failed",
    "details": "Model capacity exceeded, try again later",
    "retryAfter": 120,
    "suggestedAction": "Reduce complexity or retry with different model"
  }
}
```

#### **Business Logic Errors (422)**
```json
{
  "error": {
    "code": "INSUFFICIENT_CREDITS",
    "message": "Insufficient credits to process interview session",
    "details": "Required: $1.50, Available: $0.75",
    "suggestedAction": "Add credits to your account or upgrade plan"
  }
}
```

## **Retry Strategy**

### **Exponential Backoff Pattern**
For retriable errors (429, 500, 502, 503):

```typescript
const retryDelays = [1, 2, 4, 8, 16]; // seconds
const maxRetries = 5;

async function callWithRetry(apiCall: () => Promise<any>, attempt = 0): Promise<any> {
  try {
    return await apiCall();
  } catch (error) {
    if (isRetriable(error) && attempt < maxRetries) {
      const delay = retryDelays[attempt] * 1000;
      await sleep(delay);
      return callWithRetry(apiCall, attempt + 1);
    }
    throw error;
  }
}
```

### **Session Timeout Handling**
```typescript
// Handle session timeouts gracefully
if (error.code === 'SESSION_TIMEOUT') {
  // Automatically save progress and restart
  await saveSessionProgress(sessionId);
  const newSession = await startSession(previousConfig);
  return continueFromLastQuestion(newSession);
}
```

## **Request/Response Headers**

### **Required Request Headers**
```
Authorization: Bearer {jwt-token}
Content-Type: application/json
X-Request-ID: {unique-request-id}
X-Session-ID: {session-id} // For session-related requests
```

### **Standard Response Headers**
```
Content-Type: application/json
X-Request-ID: {echoed-request-id}
X-RateLimit-Remaining: 45
X-RateLimit-Reset: 1699027200
X-Session-Expires: 1699025400 // For active sessions
```

### **WebRTC Headers** 
```
X-WebRTC-Session: {webrtc-session-id}
X-Recording-Status: active|paused|stopped
```

This API design ensures consistency, reliability, and seamless integration for AI-powered interview preparation! 🎯