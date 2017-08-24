# NoteShareAPI [![Build Status](https://travis-ci.org/SepSpring2017/NoteShareAPI.svg?branch=master)](https://travis-ci.org/SepSpring2017/NoteShareAPI)

This will serve as the Web API for the [NoteShare web app](https://github.com/SepSpring2017/NoteShareWeb).

## Technologies

* [.NET Core WebAPI](https://docs.microsoft.com/en-us/dotnet/core/)
* [EF Core](https://docs.microsoft.com/en-us/ef/core/) - Entity Framework for database access
* [.NET Core Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity) - Authentication
* [MS SQL for EF Core](https://docs.microsoft.com/en-us/ef/core/providers/sql-server/) - SQL Server
* [Microsoft Azure](https://portal.azure.com) - Free web app hosting

## Running this on your machine

1. Install packages with `dotnet restore`
2. Run migrations and create database with `dotnet ef database update`
3. Run the development server with `dotnet run`
