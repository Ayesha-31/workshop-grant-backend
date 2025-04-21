# 🧑‍🎓 Workshop Grant Verification System

A full-stack web application that verifies student grant eligibility based on workshop attendance. Designed to simplify the manual verification process with an automated, Excel-integrated backend and a sleek, real-time eligibility checker UI.

## 🌐 Live Demo

- 🔗 **Frontend**: [https://ayesha-31.github.io/workshop-grant-frontend/](https://ayesha-31.github.io/workshop-grant-frontend/)
- 🔗 **Backend**: [https://workshop-grant-backend.onrender.com](https://workshop-grant-backend.onrender.com)

## 🚀 Features

- ✅ Real-time grant eligibility checking
- ✅ Upload `.xlsx` files to auto-import students and attendance
- ✅ Display attended workshops with names + dates
- ✅ SQLite-powered database for simple deployment
- ✅ Responsive, minimal frontend hosted on GitHub Pages
- ✅ Dockerized .NET 9 backend deployed via Render

## 🛠 Tech Stack

| Layer       | Technology                          |
|-------------|--------------------------------------|
| Frontend    | HTML, CSS, JavaScript               |
| Backend     | ASP.NET Core Web API (.NET 9)       |
| Database    | SQLite + Entity Framework Core      |
| Excel Import| EPPlus                              |
| Hosting     | GitHub Pages (frontend), Render (API) |

## 📦 Local Setup Instructions

### 🔧 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download)
- Git

### 🛠 Backend Setup

1. Clone the repo:

---bash
git clone https://github.com/Ayesha-31/workshop-grant-backend.git
cd workshop-grant-backend/WorkshopGrantSystem

Run the project:

dotnet restore
dotnet run


Navigate to:
https://localhost:5136

📊 Excel Upload Format
Each sheet represents one workshop

First column: Student Name
Second column: Student ID
Sheet name = Workshop title


✅ Example Sheet (Title: AI Bootcamp):
Name	ID
Ayesha Tabassum	12345678
John Doe	87654321


📤 Upload Data
Go to:
🔗 upload.html
Upload your formatted .xlsx file to import student + attendance data.

🔍 Eligibility API
Endpoint:
GET /eligibility/student/{id}


Example Response:
json
{
  "studentId": 12345678,
  "name": "Ayesha Tabassum",
  "workshopCount": 4,
  "isEligible": true,
  "workshopsAttended": [
    { "title": "AI Bootcamp", "date": "2025-02-01" },
    { "title": "Data Science Basics", "date": "2025-02-08" }
  ]
}

🧠 Deployment
📦 Backend
Dockerized .NET 9 app

Hosted on Render
Automatically redeploys on git push

🌐 Frontend
Static HTML/JS

Hosted via GitHub Pages

📸 Screenshot
<img width="1466" alt="image" src="https://github.com/user-attachments/assets/534823ed-70eb-4b9d-8611-8b2439ba8991" />


Author
Ayesha Tabassum
Graduate Student | Data Analyst | Full-Stack Explorer
GitHub: Ayesha-31

⭐ Give it a Star!
If you found this helpful or cool, don’t forget to ⭐ star the repo and share it!
