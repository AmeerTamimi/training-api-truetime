# TrueTime Training API (ASP.NET Core)

A small ASP.NET Core Web API built during a training period with **True Time Solutions**.
This project was mainly for practice and learning (controllers, DTOs, basic CRUD structure, and EF Core DbContext).

## What’s inside
- ASP.NET Core Web API
- Controllers for: Address, Category, Order, Payment, Product, User
- DTO layer for request/response models
- EF Core DbContext (`MyAppContext`)
- Basic app configuration (`appsettings.json`)

## Project Structure
- `Controllers/` - API endpoints
- `DTOS/` - DTOs used by endpoints
- `Models/` - entity models
- `Data/` - database context / data access

## Run
```bash
dotnet restore
dotnet run
