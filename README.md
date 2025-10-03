# Disaster Alleviation Foundation


> A software platform to support disaster relief efforts — coordinating aid, volunteers, and resources.

---

## Table of Contents

- [About](#about)  
- [Features](#features)  
- [Tech Stack](#tech-stack)  
- [Getting Started](#getting-started)  
  - [Prerequisites](#prerequisites)  
  - [Installation](#installation)  
  - [Database setup](#database-setup)  
  - [Running](#running)  
- [Usage](#usage)  
- [Contributing](#contributing)  
- [License](#license)  
- [Contact](#contact)  

---

## About

The **Disaster Alleviation Foundation** is a project aimed at simplifying and enhancing disaster response. It provides tools to:

- Manage and distribute relief resources  
- Coordinate volunteers  
- Track affected areas and needs  
- Interface with donors, government, NGOs, and communities  

Its goal is to improve efficiency, transparency, and responsiveness during disasters.

---

## Features

- User authentication & roles (admin, volunteer, coordinator, donor)  
- Dashboard for incident overview and metrics  
- Resource request & dispatch system  
- Volunteer scheduling and tracking  
- Mapping / geolocation of affected zones  
- Notifications & alerts  
- Reporting & analytics  

---

## Tech Stack

| Layer                | Technology / Framework          |
|----------------------|----------------------------------|
| Backend / API        | ASP.NET Core Web API / C#        |
| Frontend / UI        | (Your frontend library, e.g. React, Blazor, etc.) |
| Database             | (e.g. SQL Server, PostgreSQL, etc.) |
| ORM / Data Access    | Entity Framework Core (if used)  |
| Hosting / Deployment | (Azure, AWS, Docker, etc.)       |

_You can modify this table to match your actual implementation._

---

## Getting Started

### Prerequisites

Make sure you have installed:

- [.NET SDK (version X.X or above)](https://dotnet.microsoft.com/)  
- A relational database (e.g. SQL Server, PostgreSQL)  
- (Optional) Node.js / npm if frontend needs it  

### Installation

### Installation & Setup
1.  **Clone the repository**
    ```bash
    git clone https://github.com/your-username/disaster-management-system.git
    cd disaster-management-system
    ```

2.  **Install dependencies**
    ```bash
    npm install
    ```

3.  **Set up environment variables**
    Create a `.env` file in the root directory and configure your database and other settings:
    ```env
    DATABASE_URL=your_postgresql_connection_string
    SESSION_SECRET=your_secret_key
    ```

4.  **Initialize the database**
    ```bash
    npm run db:reset
    ```

5.  **Start the development server**
    ```bash
    npm run dev
    ```
    The application will be running at `http://localhost:3000`.

## 👤 Usage

Once the application is running, you can perform the following actions:

### Authentication
- **Log in** using the provided demo accounts:
  - **Admin Account:** `admin@example.com` / `admin123`
  - **Test User Account:** `test@example.com` / `test123`

### Core Features
- **Create New Disaster Incidents:** Log events, set severity levels, and define affected areas.
- **Manage Resource Requests:** Add and track requests for critical supplies like water, food, and medical kits.
- **Volunteer Coordination:** Assign available volunteers to incidents and track their dispatch status.
- **Monitoring & Reporting:** View a real-time dashboard of all active incidents and generate summary reports.

### UI Walkthrough

#### Dashboard Overview
The main dashboard provides an at-a-glance view of all active incidents and key metrics.
![Dashboard Screenshot](./screenshots/dashboard.png)

#### Creating a New Incident
Easily log a new disaster event with details like location, type, and severity.
![Create Incident GIF](./screenshots/create-incident.gif)

#### Managing Resources
Add and fulfill resource requests from the incident management page.
![Resource Management Screenshot](./screenshots/resources.png)

---



## Author

Contact
-Author: Risima Gala

-GitHub: @RisimaGala

-Email: your-email@example.com
