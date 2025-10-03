Disaster Alleviation Foundation - ASP.NET MVC Application
The Disaster Alleviation Foundation is a comprehensive web platform designed to coordinate disaster response efforts through efficient user management, real-time incident reporting, secure donation processing, and organized volunteer coordination. This ASP.NET Core MVC application provides a complete disaster management ecosystem.

🚀 Core Features
1. User Authentication & Security
Secure Registration & Login: Cookie-based authentication with BCrypt password hashing

Role Management: User, Volunteer, and Admin role support

Profile Management: Complete user profile system with statistics tracking

Session Management: 30-day persistent login with "Remember Me" option

2. Disaster Incident Management
Real-time Incident Reporting: Categorized disaster type reporting with severity levels

Location Tracking: Geographic location and disaster zone mapping

Status Monitoring: Incident status tracking from reported to resolved

Image Support: Visual documentation through image uploads

3. Donation Coordination
Impact Tracking: Real-time donation statistics and progress visualization

Transparent Reporting: Comprehensive donation history and impact assessment

User Engagement: Personalized donation tracking for registered users

4. Volunteer Management
Skills-Based Registration: Comprehensive skill catalog matching volunteers to needs

Opportunity Management: Categorized volunteer role assignments

Availability Coordination: Flexible scheduling and task management

Performance Tracking: Volunteer contribution statistics and history

🛠 Technology Stack
Backend Framework
ASP.NET Core MVC 8.0 - Modern web application framework

Entity Framework Core - Data access with SQLite database

BCrypt.Net-Next - Secure password hashing and verification

Cookie Authentication - Persistent and secure user sessions

Frontend Development
Bootstrap 5.3.0 - Responsive UI framework

Font Awesome 6.0 - Professional icon library

jQuery 3.6.0 - Client-side interactivity

Custom CSS3 - Branded styling and animations

Development & Deployment
Azure DevOps - CI/CD pipeline automation

Git Version Control - Source code management

SQLite Database - Lightweight, file-based data storage




📁 Application Architecture
text
DisasterAlleviationFoundation/
├── Controllers/
│   ├── AuthController.cs          # Authentication & user management
│   ├── HomeController.cs          # Dashboard & main views
│   ├── ProfileController.cs       # User profile management
│   └── IncidentController.cs      # Disaster incident handling
├── Models/
│   ├── User.cs                    # User entity with relationships
│   ├── DisasterIncident.cs        # Incident reporting structure
│   ├── Volunteer.cs               # Volunteer management
│   ├── Donation.cs                # Donation tracking
│   └── ViewModels/                # UI data transfer objects
├── Views/
│   ├── Auth/                      # Authentication interfaces
│   ├── Home/                      # Dashboard & landing pages
│   ├── Profile/                   # User profile management
│   └── Shared/                    # Layout components
├── Data/
│   └── ApplicationDbContext.cs    # Database context & configuration
└── Program.cs                     # Application startup & configuration
🗄 Database Schema
Core Entities
Users: Complete user profiles with authentication data

DisasterIncidents: Comprehensive incident reporting with status tracking

Donations: Financial and resource contribution records

Volunteers: Skill-based volunteer profiles and assignments

Skills: Categorized volunteer competency tracking

Data Relationships
One-to-Many: Users → IncidentReports (User reporting system)

One-to-Many: Users → Donations (Contribution tracking)

One-to-Many: Users → Volunteers (Participation history)

Many-to-Many: Volunteers ↔ Skills (Competency mapping)

🚀 Quick Start
Prerequisites
.NET 8.0 SDK

Visual Studio 2022+

Git version control

Installation & Setup
bash
# Clone repository
git clone [your-repository-url]
cd DisasterAlleviationFoundation

# Restore dependencies
dotnet restore

# Run database setup
dotnet run
Initial Configuration
Database: SQLite database auto-created on first run

Authentication: Cookie-based sessions with 30-day persistence

File Storage: Local file system for image uploads

Logging: Comprehensive debug and information logging

🔧 Configuration
Application Settings (appsettings.json)
json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=DisasterAlleviation.db"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
📊 Key Functionality
User Dashboard
Real-time statistics on incidents, volunteers, and donations

Personalized activity tracking and contribution history

Quick access to reporting and participation features

Incident Reporting
Multi-category disaster classification

Geographic location and severity assessment

Visual documentation through image uploads

Real-time status updates and tracking

Security Implementation
BCrypt password hashing with work factor 13

Enhanced hash verification for cross-platform compatibility

Secure session management with cookie authentication

Input validation and sanitization throughout

🛡 Security Features
Password Protection: BCrypt hashing with salt

Session Security: Cookie-based authentication with expiration

Data Validation: Comprehensive client and server-side validation

SQL Injection Prevention: Parameterized queries via Entity Framework

XSS Protection: Input encoding and output escaping

📈 Performance Optimization
Asynchronous Operations: Non-blocking database operations

Eager Loading: Optimized data retrieval with Include()

Client-Side Validation: Reduced server load through pre-validation

Static File Caching: Efficient asset delivery

🤝 Development Workflow
Branch Strategy
main - Production-ready releases

develop - Feature integration branch

feature/* - Individual feature development

hotfix/* - Emergency production fixes

Quality Assurance
Automated build validation on Azure DevOps

Comprehensive error handling and logging

User-friendly error messages and recovery flows

Cross-browser compatibility testing

