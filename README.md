## 📋 README.md

```markdown
# SimpleWPFWork - Todo Management Application

A modern todo management application built with WPF, .NET 10, and SQL Server featuring CQRS pattern, temporal tables, and stored procedures.

## 🛠️ Technology Stack

### Backend
- **.NET 10** - Framework
- **ASP.NET Core Web API** - REST API
- **Entity Framework Core** - ORM
- **SQL Server** - Database with Temporal Tables
- **Dapper** - Micro-ORM for update operations
- **MediatR** - CQRS implementation
- **AutoMapper** - Object mapping
- **FluentValidation** - Input validation
- **Serilog** - Structured logging
- **ADO.NET** - GetList Functions With Stored Procedures
-- **Xunit Test Layer ** - API endpoints testings
### Frontend
- **WPF (.NET 10)** - Desktop UI
- **MVVM Pattern** - Architecture
- **NSwag** - API client generation
- **Microsoft.Extensions.DependencyInjection** - IoC container

### Database
- **SQL Server** - Primary database
- **Temporal Tables** - Automatic history tracking
- **Stored Procedures** - Performance-optimized queries (ADO.NET)
- **Repository Pattern** - Data access abstraction

### Architecture & Patterns
- **CQRS** (Command Query Responsibility Segregation)
- **Repository Pattern**
- **Dependency Injection**
- **Domain-Driven Design** (DDD)
- **Clean Architecture**

## 🚀 How to Run

### Prerequisites
- Visual Studio 2022
- .NET 10 SDK
- SQL Server (LocalDB or full instance)
- Git

### Step 1: Database Setup
Run **SimpleWPFWork.DbMigrator** project to:
- Create database and tables
- Create stored procedures
- Seed initial data

### Step 2: Start API Server
Run **SimpleWPFWork.Host** project to start the REST API at `https://localhost:7213`

### Step 3: Start Desktop Application
Run **SimpleWPFWork.WPFUI** project to launch the WPF application

**Note:** API server (Step 2) must be running before starting the WPF app (Step 3)

