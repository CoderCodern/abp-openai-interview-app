# Enhanced JobDescription Entity - URL Extraction Support

**Date:** November 6, 2025  
**Status:** Updated for Vietnamese & International Job Board Support  

---

## 🌏 **New Features Added**

### **Multi-Regional Job Board Support**
- **Vietnamese Job Boards:** TopCV.vn, VietnamWorks, ITViec, CareerBuilder.vn, etc.
- **International Boards:** LinkedIn, Indeed, Glassdoor, Monster, etc.
- **Local/Regional Sites:** Any website with configurable scraping rules
- **API Integration:** Support for official job board APIs

### **Localization & Currency Support**
- **Country Codes:** VN, US, SG, TH, MY, etc.
- **Languages:** Vietnamese (vi), English (en), Chinese (zh), etc.
- **Currencies:** VND, USD, SGD, THB, MYR with proper formatting
- **Date Formats:** Regional date parsing (dd/MM/yyyy vs MM/dd/yyyy)

---

## 📊 **Updated JobDescription Entity**

### **🔗 NEW: URL Extraction & Source Information**
```csharp
// Source tracking
string? OriginalUrl              // Original job posting URL (Max 500 chars)
string? JobBoardName             // "TopCV", "VietnamWorks", "LinkedIn" (Max 100 chars)
string? SourceDomain             // "topcv.vn", "vietnamworks.com" (Max 100 chars)
JobDescriptionSource InputType   // ManualText/UrlScraping/JobBoardApi/etc.
string? ExternalJobId            // Job ID from source site (Max 100 chars)
DateTime? ExtractedAt            // When job was scraped

// Localization
string? CountryCode              // "VN", "US", "SG" (Max 10 chars)
string? Language                 // "vi", "en", "zh" (Max 10 chars)
string? Currency                 // "VND", "USD", "SGD" (Max 10 chars)

// Additional metadata
string? JobType                  // "Full-time", "Part-time", "Contract" (Max 50 chars)
DateTime? ApplicationDeadline    // When applications close
DateTime? OriginalPostDate       // When job was first posted
string? CompanyLogoUrl           // Company logo from job board (Max 500 chars)
string? RawContent               // Original scraped content (debugging)
```

### **📋 Enhanced JobDescriptionSource Enum**
```csharp
public enum JobDescriptionSource
{
    ManualText = 0,          // User pasted text
    UrlScraping = 1,         // Web scraping extraction
    JobBoardApi = 2,         // Official API integration
    FileUpload = 3,          // PDF/Word file upload
    BrowserAutomation = 4,   // Selenium/Playwright extraction
    RssFeed = 5             // RSS/XML feed parsing
}
```

---

## 🔧 **New Business Methods**

### **URL Extraction Management**
```csharp
// Set extraction information
void SetExtractionInfo(originalUrl, jobBoardName, inputType, ...)

// Set job metadata
void SetJobMetadata(jobType, deadline, postDate, logoUrl)

// Check extraction status
bool IsExtractedFromUrl()
bool IsVietnameseJobBoard()
string GetJobSourceDisplayName()

// Job status checks
bool IsJobActive()                    // Check if past deadline
string GetFormattedSalary()           // Format with currency
void MarkForReExtraction()            // Re-scrape from URL
```

### **Vietnamese Job Board Detection**
```csharp
public bool IsVietnameseJobBoard()
{
    var vietnameseDomains = new[]
    {
        "topcv.vn", "vietnamworks.com", "itviec.com", 
        "careerbuilder.vn", "jobsgo.vn", "mywork.com.vn",
        "vieclam24h.vn", "timviec365.vn"
    };
    
    return vietnameseDomains.Any(domain => 
        SourceDomain.Contains(domain, StringComparison.OrdinalIgnoreCase));
}
```

---

## 🗄️ **Database Schema Updates**

### **New Indexes for Performance**
```sql
-- Source & region queries
CREATE INDEX IX_JobDescriptions_InputType ON AppJobDescriptions (InputType);
CREATE INDEX IX_JobDescriptions_SourceDomain ON AppJobDescriptions (SourceDomain);
CREATE INDEX IX_JobDescriptions_CountryCode ON AppJobDescriptions (CountryCode);

-- Job lifecycle
CREATE INDEX IX_JobDescriptions_ApplicationDeadline ON AppJobDescriptions (ApplicationDeadline);
CREATE INDEX IX_JobDescriptions_OriginalUrl ON AppJobDescriptions (OriginalUrl);
CREATE INDEX IX_JobDescriptions_ExternalJobId ON AppJobDescriptions (ExternalJobId);

-- Composite indexes
CREATE INDEX IX_JobDescriptions_InputType_SourceDomain ON AppJobDescriptions (InputType, SourceDomain);
CREATE INDEX IX_JobDescriptions_CountryCode_Language ON AppJobDescriptions (CountryCode, Language);
```

### **Sample Data Scenarios**

**Vietnamese Job from TopCV:**
```csharp
var job = new JobDescription(id, "Senior Developer", "FPT Software", description);
job.SetExtractionInfo(
    originalUrl: "https://www.topcv.vn/viec-lam/senior-developer-12345",
    jobBoardName: "TopCV",
    inputType: JobDescriptionSource.UrlScraping,
    sourceDomain: "topcv.vn",
    countryCode: "VN",
    language: "vi",
    currency: "VND"
);
```

**International Job from LinkedIn:**
```csharp
var job = new JobDescription(id, "Software Engineer", "Google", description);
job.SetExtractionInfo(
    originalUrl: "https://www.linkedin.com/jobs/view/3456789",
    jobBoardName: "LinkedIn",
    inputType: JobDescriptionSource.JobBoardApi,
    sourceDomain: "linkedin.com",
    countryCode: "US",
    language: "en",
    currency: "USD"
);
```

---

## 🎯 **Benefits for Vietnamese/International Support**

### **1. Flexible Job Source Handling**
- ✅ **Any Vietnamese job board** can be supported with configuration
- ✅ **Multiple input methods** (URL, API, manual, file)
- ✅ **Source tracking** for audit and re-extraction

### **2. Regional Localization**
- ✅ **Currency formatting** (VND vs USD display)
- ✅ **Language detection** for better AI processing
- ✅ **Date parsing** (dd/MM/yyyy vs MM/dd/yyyy)

### **3. Vietnamese Job Market Features**
- ✅ **Local job board detection** (TopCV, VietnamWorks, etc.)
- ✅ **VND salary formatting** with proper currency symbols
- ✅ **Vietnamese language support** for AI analysis

### **4. Scalable Architecture**
- ✅ **Easy to add new job boards** via configuration
- ✅ **Multiple extraction methods** for different sites
- ✅ **Fallback support** (API → Scraping → Manual)

---

## 🚀 **Ready for Implementation**

**✅ Entity Structure:** Complete with URL extraction fields  
**✅ Business Logic:** Methods for Vietnamese and international job boards  
**✅ Database Schema:** Optimized indexes for multi-regional queries  
**✅ Build Status:** Compiles successfully with no errors  

**Next Steps:**
1. Generate EF Core migrations for new schema
2. Build flexible job scraping service
3. Create Vietnamese job board configurations (TopCV, VietnamWorks, etc.)
4. Implement multi-language AI analysis support

The entity is now ready to handle job postings from **any Vietnamese or international job board** with proper localization and currency support! 🌏🎯