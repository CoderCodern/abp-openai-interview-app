# 🚀 AI Interview Preparation System — Planning Roadmap

A step-by-step plan to design and build the system before coding.

---

## 🧭 Overview
**Goal:** Build a GenAI-powered system that helps users prepare for job interviews  
**Tech Stack:** **ABP.io (.NET 9)** + **Angular** + **OpenAI** + **WebRTC (Camera & Audio)**

---

## 1️⃣ Product Vision
**Purpose:**  
Help users practice interviews realistically and understand target companies better.

**Core Features:**  
- Paste JD → analyze company info  
- AI generates company insights  
- AI mock interview (text or voice + camera)  
- Tailored CV generation  
- History & feedback  
- Interactive AI chat assistant

---

## 2️⃣ Functional Design
**Main Modules:**
- **User Management**  
- **JD Processor**  
- **Company Insights**  
- **CV Builder**  
- **Interview Simulator** (voice + camera)  
- **Chat Assistant**  
- **Analytics & History**

📘 Output: *Functional Design Document (FDD)*

---

## 3️⃣ Architecture (C4 Model)
Design system structure before coding.

| Level | Description |
|--------|--------------|
| **C1** | System Context (User + ABP Backend + OpenAI + DB + Angular FE + Camera/Mic APIs) |
| **C2** | Containers (Backend, Frontend, DB, External AI/ML APIs) |
| **C3** | Components (Modules, Domain Services, Repositories) |
| **C4** | Code/Class interactions (optional) |

📘 Output: *System Architecture Document*

---

## 4️⃣ API Design
Define endpoints for each module.

**Example:**
- `POST /api/jd/analyze`
- `POST /api/interview/start`
- `POST /api/interview/feedback`
- `GET /api/company/info`
- `POST /api/cv/generate`

📘 Output: *API Specification Document*

---

## 5️⃣ AI & Data Integration
- Integrate OpenAI for:
  - JD understanding  
  - Company insights  
  - Question generation  
  - Feedback & CV writing  
- Use **RAG** or **Vector DB** later for knowledge retrieval.

---

## 6️⃣ Interview Experience (New)
- Use **WebRTC** for real-time camera & voice interview  
- Integrate **Speech-to-Text (Whisper API)** for bot understanding  
- Use **Text-to-Speech** for AI voice responses  
- Provide both **mock interview (voice)** and **chat mode**

📘 Output: *Interview Interaction Flow Diagram*

---

## 7️⃣ Deployment & Scaling
- ABP backend hosted on Azure or AWS  
- Angular frontend on Vercel or Azure Static Web Apps  
- Database: PostgreSQL / SQL Server  
- Containerization with Docker for microservices

📘 Output: *DevOps & Deployment Plan*
