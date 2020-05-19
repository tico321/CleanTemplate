#CleanTemplate
This template is inspired on the following projects:
- EShop containers https://github.com/dotnet-architecture/eShopOnContainers
- jasontaylordev Clean Architecture https://github.com/jasontaylordev/CleanArchitecture
- Ardalis Clean Architecture https://github.com/ardalis/CleanArchitecture

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

If you don't want to use VS you may want to run the project from the cli and start DB and other containers manually.
If you want to setup docker-compose you may want to review https://www.richard-banks.org/2018/07/debugging-core-in-docker.html 

### Migrations
To run migrations navigate to CleanTemplate.Infrastructure project and then you can run the migrations like:
	$ dotnet ef migrations Add InitialCreate -o Persistence/Migrations
	$ dotnet ef database update
Migrations use the ApplicationDbContextFactory to create an instance of the DbContext, the connection string that it uses
can be found in the CleanTemplate.Infrastructure/appsettings.json file.
To recreate the database just run:
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
 
