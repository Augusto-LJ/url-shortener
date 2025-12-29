# 🔗 URL Shortener

A full-stack URL Shortener application designed to demonstrate real-world backend and frontend practices, focusing on clean architecture, environment separation, containerization, and production-ready patterns.

This project goes beyond a simple CRUD by addressing concerns such as unique slug generation, database migrations, CORS configuration, environment-based settings, and local development parity with production.

🧭 Software Development Life Cycle ([SDLC](https://aws.amazon.com/pt/what-is/sdlc/)) of the application:
- ✅ Planning
- ✅ Design
- ✅ Implementation
- 🛠️ **Testing**
- 🔜 Deployment
- 🔜 Maintenance

## 🚀 Technologies Used

- **Backend**:
  - ASP.NET Core 8
  - C#
  - Entity Framework Core
  - PostgreSQL
  - Npgsql
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
  - Environment-based configuration (appsettings, .env)
  - CORS policy configuration
  - Database migrations with EF Core
  - Git & GitHub
- **Architecture & Patterns**:
  - Layered architecture:
    - Controllers
    - Services
    - Data / Context
  - Dependency Injection
  - Async/await throughout the data layer
  - Clear separation of concerns between backend and frontend

## 🎯 Current Features

- Generate a shortened URL from a valid HTTP/HTTPS URL
- Unique slug generation using a Base62 strategy
- Server-side validation of URLs
- Redirect from shortened URL to the original URL
- Persistent storage using PostgreSQL
- Database schema managed via EF Core migrations
- Fully decoupled frontend and backend
- Environment-specific configuration (Development vs Production-ready setup)

## 💻 Demo

<img width="1874" height="935" alt="image" src="https://github.com/user-attachments/assets/a44a9593-4be9-4dd8-aeab-25e24e2c2a97" />

## 🛠️ How the Application Works
1. The user enters a long URL in the frontend
2. The frontend sends a request to the backend API
3. The backend:
    - Validates the URL
    - Generates a unique slug
    - Persists the mapping in the database
4. The backend returns the shortened URL
5. When the shortened URL is accessed, the API redirects to the original URL

# 🧪 Local Development Setup
**Prerequisites**
- Docker
- Docker Compose
- Git

## Steps to Run Locally
1. Clone the repository:
```bash
git clone https://github.com/Augusto-LJ/url-shortener.git
cd url-shortener
```

2. Start the application using Docker Compose
```bash
docker compose up --build
```

3. Access the services:
    - Frontend: http://localhost:3000
    - Backend API: http://localhost:5000
    - Swagger UI: http://localhost:5000/swagger

The application is fully configured to:
- Run the API and database inside containers
- Apply database migrations automatically on startup (local environment)
- Use environment variables for configuration
- Allow local frontend ↔ backend communication via CORS

## 🔜 Future improvements
- Automated testes (unit and integration)
- CI/CD pipeline
- Caching layer (e.g. Redis)
- Observability (logging, metrics)
- Rate limiting and abuse protection
- URL expiration
- Improved error handling and API responses
- User authentication and login
- Possibility to set a shortened URL as favorite and display it
- Option to delete or deactivate URLs
- Click reports per URL (count, date, etc.)
