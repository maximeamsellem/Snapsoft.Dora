# Purpose

This application purpose is to collect data in order to report [DORA](https://dora.dev/) metrics in an organization.

# Context

I am writing this application in order to show an example of how I like to structure code.

# Disclaimer

This is not a production ready software for now.
There would be a lot to improve:

- Define a product roadmap based on which it would be possible to
  - Design API contracts to avoid breaking changes
  - Identify Entities, AggregateRoot, Bounded context
- Provide a user interface
- Security
  - It lacks at least authentication
  - Executing database model changes from the WebAPi involve a powerful database user which is not ideal
  - Make sure exception details are not returned to the client
- Maintenance
  - Log to a central place like datadog for example
  - Version APIs to anticipate future new API versions
- Data quality
  - Improve validations rules
- Developer experience
  - Automate code formatting
  - Provide a more detailed developer API documentation
  - Use serilog request middleware to have shorter logs
  - Use serilog colored console logging for better log parameter display
  - Consider reducing IOC declaration by scanning assemblies
- Tests
  - Some validation tests are missing
  - Ideally implement load tests
  - Think about snapshot testing to protect from breaking changes and more (for example: now command are exposed to the web which is a not ideal)

And probably many other things

# Prerequisites

## .Net SDK

To develop you will need:
[.Net 7 Sdk](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

## dotnet ef

If you need to add database migrations, you will have to install `dotnet ef`.
One way of doing it is to execute `dotnet tool install --global --version 7.0.11 dotnet-ef`.
Be aware, that if you are using `dotnet ef` on other project on your machine it might cause trouble if it is with a different version.
In that case you would better install it next to your git repository.

## Postgres database

It is currently developed running on version [Postgres version 16](https://www.postgresql.org/download/).

You can either download it and install it. Or run it with docker.
If you are using docker, you can run the `start-postgres.ps1` script at the root of the repository.

If you are using this script. You have nothing else to do.

If not, you will probably have to write a `.env` file with your own connection string.
In that case you can inspire from the `.env.example` file.

## Docker engine

If you want to run the tests locally you will need to install [Docker](https://www.docker.com/products/docker-desktop/).

# Start the app

Assuming you have already a postgres database up and the right connection string.
you just need to go to the root of the repository and execute:

- powershell : `cd .\src\Snapsoft.Dora.WebApi\ ; dotnet run`
- shell : `cd src/Snapsoft.Dora.WebApi && dotnet run`

Then you can browse the [swagger page](http://localhost:5171/swagger/index.html) and play with it.

You can also start the application from your IDE.

# Running automated test

you just need to go the root of the repository and execute:

- powershell : `cd .\src\Snapsoft.Dora.WebApi.Integration.Test\ ; dotnet test`
- shell : `cd src/Snapsoft.Dora.WebApi.Integration.Test && dotnet test`

# Create a database migration

From the root of the repository, you can execute the powershell script like that: `.\add-migration.ps1 YourMigrationName`

This script is just an helper you necessarily have to use it.

# Testing manually

There is swagger to help : http://localhost:5171/swagger/index.html

## How to create a component

You need to run

```
Method: POST
Url: http://localhost:5171/component
```

```json
{
  "name": "DoraMetricService"
}
```

You should get a 201 StatusCode with a body like

```json
{
  "data": {
    "name": "DoraMetricService",
    "id": 22
  }
}
```

## How to get a component

You need to run

```
Method: GET
Url: http://localhost:5171/component/{your_component_id}
```

You should get a 200 StatusCode with a body like

```json
{
  "data": {
    "name": "DoraMetricService",
    "id": 22
  }
}
```

## How to declare a component deployment

You need to run

```
Method: POST
Url: http://localhost:5171/componentdeployment
```

```json
{
  "componentId": 22,
  "version": "1.0.0.0",
  "commitId": "v4cbaa6d7eg84eb021df3bg01ae0a3087f82a21cbf1"
}
```

You should get a 201 StatusCode with a body like

```json
{
  "data": {
    "version": "1.0.0.0",
    "commitId": "v4cbaa6d7eg84eb021df3bg01ae0a3087f82a21cbf1",
    "componentId": 22,
    "id": 7
  }
}
```

## How to get a component deployment

You need to run

```
Method: GET
Url: http://localhost:5171/componentdeployment/{your_component_deployment_id}
```

You should get a 200 StatusCode with a body like

```json
{
  "data": {
    "version": "1.0.0.0",
    "commitId": "v4cbaa6d7eg84eb021df3bg01ae0a3087f82a21cbf1",
    "componentId": 22,
    "componentName": "DoraMetricService",
    "id": 7
  }
}
```
