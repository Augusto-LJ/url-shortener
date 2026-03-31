# 🔗 URL Shortener
A full-stack URL Shortener application built to simulate real-world backend engineering scenarios, focusing on **API design, software architecture, and production-ready practices**.

This project goes beyond basic CRUD operations by addressing concerns such as **unique slug generation, environment configuration, containerization, and system scalability**.

---

## 🧠 Architecture Overview

The application follows a **layered architecture**, with clear separation of responsibilities:

- **Controllers** → Handle HTTP requests and responses  
- **Services** → Contain business logic and application rules  
- **Data Layer** → Responsible for persistence and database access  

Key architectural principles applied:

- Separation of concerns  
- Dependency Injection  
- Clean Code practices  
- SOLID principles  

---

## ⚙️ Technical Decisions

### 🔹 Slug Generation Strategy
- Implemented using a **Base62 encoding approach**
- Ensures short, unique, and URL-friendly identifiers
- Designed to scale efficiently with increasing data volume

### 🔹 Backend & Frontend Decoupling
- Backend exposed via REST API
- Frontend consumes API independently
- Enables scalability and independent deployment

### 🔹 Containerization
- Entire application runs via **Docker Compose**
- Ensures consistency between development and production environments

### 🔹 Environment Configuration
- Uses **environment-based settings** (`appsettings` + `.env`)
- Supports different configurations for development and production

---

## 🚀 Technologies Used

- **Backend**:
  - ASP.NET Core 8
  - C#
  - Entity Framework Core
  - PostgreSQL (Npgsql)
  - RESTful API
  - Swagger
    
- **Frontend**:
  - Vue.js 3
  - Vite
  - TypeScript
  - Axios
  - HTML5 / CSS3
 
- **Infrastructure & Tooling**:
  - Docker / Docker Compose
  - CORS policy configuration
  - EF Core Migrations
  - Git & GitHub

## 🎯 Features

- Shorten long URLs into unique, compact links
- Redirect shortened URLs to the original destination
- Server-side URL validation  
- Persistent storage with PostgreSQL  
- Environment-specific configuration  
- Fully decoupled frontend and backend 

## 💻 Demo

<img width="1764" height="830" alt="image" src="https://github.com/user-attachments/assets/391f929f-ff31-45ad-a962-676ca51c31d4" />


## 🔄 How It Works
1. User submits a URL through the frontend
2. Frontend sends a request to the backend API
3. Backend:
    - Validates the URL
    - Generates a unique slug
    - Persists the mapping in the database
4. Returns the shortened URL
5. Accessing the short URL triggers a redirect to the original URL

## 🧪 Local Development Setup
### **Prerequisites**
- Docker Desktop
- Git

### Run the application
```bash
git clone https://github.com/Augusto-LJ/url-shortener.git
cd url-shortener
docker compose up --build
```

### Access the services:
- Frontend: http://localhost:3000
- Backend API: http://localhost:5000

## ⚡ Engineering Considerations
- Designed for extensibility and maintainability
- Prepared for future improvements such as:
  - Caching (Redis)
  - Rate limiting
  - Observability (logs & metrics)
  - Authentication & user management

## 🔜 Future improvements
- CI/CD pipeline
- Caching layer (e.g. Redis)
- Observability (logging, metrics)
- Rate limiting and abuse protection
- URL expiration
- Improved error handling and API responses
- Authentication and user accounts
- Favorites and URL management
- Option to delete or deactivate URLs
- Click analytics per URL


**⭐ Feel free to explore the repository and provide feedback.</h2>**
