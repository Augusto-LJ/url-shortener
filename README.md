# 🔗 URL Shortener

A simple and functional web application that allows you to shorten long links, generating short URLs for easy sharing. Ideal for anyone who needs to organize or share links with practicality and clarity.

Software Development Life Cycle ([SDLC](https://aws.amazon.com/pt/what-is/sdlc/) of the application:
- ✅ Planning
- ✅ Design
- ✅ Implementation (in progress)
- 🛠️ **Testing**
- 🔜 Deployment
- 🔜 Maintenance

## 🚀 Technologies

- **Backend**: ASP.NET Core 8
- **Frontend**: Vue.js 3 + Vite
- **ORM**: Entity Framework Core
- **Database**: SQL Server
- **IDE**: Visual Studio
- **Version Control**: Git + GitHub
- **Standards**: RESTful API, Layers (Controller, Service, Model)
- **Package Management**:
  - Backend: NuGet
  - Frontend: npm
- **Others**: Axios (HTTP requests on frontend), Tailwind CSS

## 🎯 Features (to be developed)

- Generate a shortened URL from an original link
- Redirect the short URL to the original URL
- Store shortened URLs in a database
- Page with a form to shorten URLs
- Validation of the URL entered by the user

## 💻 Demo

To be filled in.

## 🛠️ How will the MVP work?

- The user accesses a screen with an input field
- The user enters a long URL in the field
- When clicking "Shorten", the app sends the URL to the backend
- The backend returns the shortened URL to the user (e.g., https://myshortener.com/abc123)
- When someone accesses https://myshortener.com/abc123, the system redirects to the original URL

## 🔜 Future improvements

- User authentication and login
- Users can view their favorite shortened URLs
- Option to delete or deactivate URLs
- Click reports per URL (count, date, etc.)
- Automatic URL expiration (by date or number of accesses)
