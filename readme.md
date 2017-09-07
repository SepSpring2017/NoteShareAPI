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
2. Make sure you have the `ASPNETCORE_ENVIRONMENT` set to Development. [Docs here.](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/environments)
3. Run the development server with `dotnet run`

## API Endpoints

### User Registration
```http
POST /api/Users HTTP/1.1
Content-Type: application/json
{ Email:"email@domain.com", Password:"p@ssword123" }
```
This endpoint will register users when you provide a valid JSON object that matches our [Credentials model](https://github.com/SepSpring2017/NoteShareAPI/blob/master/Models/Credentials.cs).

### User Login

```http
POST /connect/token HTTP/1.1
Content-Type: application/x-www-form-urlencoded
grant_type=password&username=test%40test.com&password=J8cG!FjD
```
If login is successful this will return an access token in the following format. This token can be used to make authenticated requests to the API endpoints that have an `[Authorize]` attribute.
```json
{
  "token_type": "Bearer",
  "access_token": "CfDJ8Ec0ZpniaHhGg0e0UUvOH9BWZSGrPoEwGd0_Lq2cse-T29YOq985IBiT5fEe5tTSgY1vxq2Z2ZJ7Ikwlpmh0Lrc4x9pqhqHBziUzsP_rkGZkn47TkNkOkzKCwZJZK5x-irH3HROwClFFTq0rgWdb8rZ2xriffNzsby4VwhxhN5soFD435KzmVYkdv-VuaLYo3QiSuexbRi2USVO9LK30vomAG6h2SAxZ7R-jYsXgf0f5gAmdYxg7w3yicv9v8DpUSBiGGRRfymTOnvGEsFJjGuuP8OlY5qzMs6wGaRWkOvCyV2CK_RZF_3TMs7LYCdMQ-dqWY5A03-03OmP8blKzlrKJMDZfrPQHuysbS931xxy8b3kjicfjNLmMHqzQzbUO4fecm4kY8PFnKozojDtqajfTp2bYhxS65bmVYROrswYeUWEKYR6LSdS1K__IDaLoMlLa-Wf6x1wjM2CchzgqbHRF0KEtdL5Ks88dAS44mp9BM6iUOEWyL7VkbazsBdlNciM5ZZB1_6qunufDW_tcaR8",
  "expires_in": 3600
}
```