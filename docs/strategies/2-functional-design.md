# 🧩 Step 2 — Functional Design Document (FDD)

Define **what** the system does — each module’s purpose, responsibilities, and interactions.

---

## 🎯 Goal
To map all major modules and user interactions before developing architecture or APIs.

---

## 🧱 System Modules Overview
| Module | Description | Main Responsibilities |
|---------|--------------|-----------------------|
| **1. User Management** | Manage user accounts, authentication, and profiles. | Register / Login / JWT / Roles / Profile Update |
| **2. JD Processor** | Parse and analyze job descriptions. | Extract title, skills, requirements, and company name. |
| **3. Company Insights** | Gather company details via GenAI and web info. | Generate company summary, industry, products, and tips. |
| **4. CV Builder** | Generate tailored CVs based on JD and user profile. | Create, edit, and export AI-generated CVs. |
| **5. Interview Simulator** | Simulate AI mock interviews via text, voice, or video. | Conduct sessions, evaluate answers, give feedback. |
| **6. Chat Assistant** | Offer general interview help and system navigation. | Suggest improvements, explain results, or guide next steps. |
| **7. Analytics & History** | Store and visualize user performance. | Track interview scores, progress, and recommendations. |

---

## ⚙️ User Journeys

### 🧩 1. JD Analysis Flow
1. User pastes Job Description.  
2. System extracts key data (position, skills, tech).  
3. OpenAI summarizes and enhances JD info.  
4. Output: **Company insights + Skill analysis + Interview topics.**

### 🧩 2. Company Insights Flow
1. System identifies company from JD.  
2. Queries OpenAI / web for details.  
3. Generates report:  
   - Overview  
   - Market field & size  
   - Projects or culture  
   - Interview expectations  

### 🧩 3. CV Generation Flow
1. User provides base CV or personal data.  
2. System aligns it with JD tone & requirements.  
3. Generates a tailored CV in desired style.  
4. User reviews and downloads.

### 🧩 4. Mock Interview Flow
1. User selects “Start Interview”.  
2. System launches **text or voice (WebRTC)** interview.  
3. AI asks questions → listens or reads answers.  
4. AI gives instant or final feedback (score, advice).  
5. Session saved for later review.

### 🧩 5. Chat Assistant Flow
1. User asks any question (e.g. “How to answer salary questions?”).  
2. System uses OpenAI to respond conversationally.  
3. Assistant can also explain system features or give suggestions.

### 🧩 6. Analytics & History Flow
1. After each session, system logs data.  
2. User views past results, CV versions, and insights dashboard.

---

## 🧰 Input / Output Examples

| Module | Input | Output |
|--------|--------|--------|
| JD Processor | JD text | Extracted skills, job title, company |
| Company Insights | Company name | Company summary, tips, profile |
| CV Builder | JD + User info | Generated CV (PDF/Docx) |
| Interview Simulator | JD + Mode (voice/chat) | Feedback, score, transcript |
| Analytics | Session data | Dashboard & reports |

---

## 🧩 Inter-Module Interactions
- **JD Processor → Company Insights** → gives company name & job context.  
- **Company Insights → Interview Simulator** → provides context for interview questions.  
- **CV Builder** depends on **JD Processor** and **User Management**.  
- **Analytics** consumes results from **Interview Simulator** and **CV Builder**.  

---

## 📘 Deliverables
- Functional Design Document (this file)  
- Module flow diagrams (optional next)  
- JD-to-Interview pipeline overview  

---

## ✅ Next Step
Move to **Step 3: Architecture Design (C4 Model)** to map these modules into containers and components.
