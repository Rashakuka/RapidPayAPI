# API Documentation

Welcome to our API! This guide will walk you through the steps needed to effectively test and interact with the API endpoints.

## Prerequisites

Before running the API, ensure that the database connection is properly set up:

- Verify the database `ConnectionString` in the `appsettings.Development.json` file. The project is configured to automatically create the database from scratch when you run it.

## Create database

 - dotnet ef database update

## Authentication

This API uses token-based authentication. Start by authenticating using the login endpoint from the Authentication module to obtain a token.

### Available User Roles

- **Admin**
  - Role: `Admin`
  - Credentials:
    - UserName: `admin`
    - Password: `adminpassword`
  - Permissions: Admin users can access any endpoint from the Card Management module.

- **User**
  - Role: `User`
  - Credentials:
    - UserName: `user`
    - Password: `userpassword`
  - Permissions: User role can access `card` and `pay` endpoints from the Card module.

### Authorization

Unauthorized requests to the Card Management module will return an error code `401 (Unauthorized)`. To authorize:

- **Browser Usage**
  - Navigate to SWAGGER UI.
  - Press the "Authorize" button and paste the token generated from the login endpoint.

- **Postman Usage**
  - Go to the "Authorization" tab.
  - Choose "Bearer Token" type.
  - Paste the generated token into the "Token" text box.

## Testing Endpoints

Once authenticated and authorized, you can interact with the following endpoints in the Card Management module:

- `card/create`
  - Description: Allows you to create new credit cards.

- `card/balance`
  - Description: Retrieves the current balance of an existing credit card.

- `card/pay`
  - Description: Registers new payments to an existing credit card.
  - Note: Payment fees are calculated within this module and used when executing the pay endpoint.

