# CleanTemplate
This template is inspired on the following projects:
- EShop containers https://github.com/dotnet-architecture/eShopOnContainers
- jasontaylordev Clean Architecture https://github.com/jasontaylordev/CleanArchitecture
- Ardalis Clean Architecture https://github.com/ardalis/CleanArchitecture

## Contents
- Sample todo application
- Startup DDD helpers
    - ValueObject is a class that can be extended, It has a default implementation that allows you to compare value objects.
    - Enumeration Allows you to create enums that have a few advantages over build in ones, specially as they can be stored
    as complex values in the DB instead of just numbers.
    - IAuditableEntity allows us to track changes to entities just by implementing this interface.
    - Entity is a base class that has a default implementation to compare entities by their Id.
- Based on Commands and Queries.
- Structured logging with Serilog.
    - Logs are already configured and will be logged to ApplicationLogs table in PostgreSQL.
    - ILoggerAdapter abstraction is used for testability.
- Consistent style with .editorconfig
- Samples for different types of tests
    - Unit tests. Initially application and domain layers are covered 100% by unit tests and we strongly recommend to keep it that way.
    - Integration tests. They exercise the application as a black box and cover the API actions at a high level.
        - Note: Integration tests are setup to run in parallel and will share the WebApplicationFactory between test classes,
        This is a personal recommendation that will require a shift in your thinking as you need to consider that the db may be
        affected by other tests similar to a real application.
        If you are having problems with tests interfering with each other disable parallelization in x unit, this will make 
        your tests slower but sometimes is inevitable. https://stackoverflow.com/a/61122438/8765790
    - This project uses FakeItEasy to create test doubles https://github.com/FakeItEasy/FakeItEasy
- Default API conventions https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-3.1
- Helpful libraries that are used in the project:
    - Mediatr
    - Automapper
    - FakeItEasy
    - Entity Framework Core

## Requirements
- Install docker and docker-compose
  - PostgreSQL image https://hub.docker.com/_/postgres
  - Admininer image
    - http://localhost:8080/
    - Loging info in the connection string.
- Install .NET core
- Install nodejs

## Run the application

### Run the docker-compose project
The easiest way to run the application is to do it from VS Studio.
Visual Studio recognizes the docker-compose project so you just need to:
- Mark docker-compose project as startup project.
- Run the project.
- Run the migrations with $ dotnet ef database update

### Run from another IDE
#### Use the CLI
If you don't want to use VS you may want to run the project from the cli and start DB and other containers manually.
- Go the the root of the project and start PostgreSQL and adminer with:
    - $ docker-compose -f docker-compose.dev.yml up
- Run the migrations from the CleanTemplate.Infrastructure directory:
    - $ dotnet ef database update 
- To run the dotnet project:
    - Update CleanTemplate.API/appsettings.Development.json and CleanTemplate.API/appsettings.json connection strings 
    and set `localhost` instead of `db`.
    - from CleanTemplate.API run: $  dotnet watch run
    - This will start the project in the port 5000 and 5001 as defined on CleanTemplate.API/Properties/lauchSettings.json
- To run all the unit tests go to the root of the project and run:
    - dotnet test ./CleanTemplate.sln
 
#### Include the API in docker-compose
VS does a lot of things under the hood for which if you want to add it to the docker container without lossing debug capabilities
you may want to  review https://www.richard-banks.org/2018/07/debugging-core-in-docker.html

## Migrations
To run migrations navigate to CleanTemplate.Infrastructure project and then you can run the migrations like:
    $ dotnet ef database update
Migrations use the ApplicationDbContextFactory to create an instance of the DbContext, the connection string that it uses
can be found in the CleanTemplate.Infrastructure/appsettings.json file.
To Add a new migration run:
    $ dotnet ef migrations Add InitialCreate -o Persistence/Migrations
To recreate the database run:
    $ dotnet ef database drop
    $ dotnet ef database update

## Architecture
This project is structured in the following way, at the core we have Domain and Application which hold the types and business logic
of the system. They are independent of the other layers and frameworks which makes them maintainable, extensible and testable. We have
infrastructure which groups all external concerns

### Domain
The Domain holds enterprise logic that can be shared across systems.
It doesn't have a dependency in any other layer and it's the core of the project.
Folders should be build around context boundaries.

### Application
 Application has business logic and types that is specific for this system.
 It only depends on the Domain and is also considered the core of the project.
 All the logic is build based on commands and queries which are grouped by concrete features of the application, external
 dependencies are abstracted in interfaces so there is no real dependency to any concrete technology.

 ### Infrastructure
 Depends on application and contains all the external concerns (ex. Persistence, API clients, etc.)
 No layer should depend on infrastructure, all concrete implementations should be added here but should cohesively be grouped
 in folders so it's easy to move to a separate project if convenient.

 ### API
 API project only concern is to build the composition root and to provide a way to access our well defined views and models defined
 in the core of the system by making the API available to consumers.

**Entry Point**

 This layer shouldn't have any logic, it should only define the routes for the actions and correctly translate requests and responses
 between the client and the server.

 **Composition root**
 The Composition Root is a single, logical location in an application where modules are composed together. In this project we use
 the default DI container to build the dependency tree and associate concrete instances to abstractions.

 ### Presentation
 The presentation layer in this case is implemented with the VueJS framework, this is the face of the application.
 It provides a way for users to interact with the system.

