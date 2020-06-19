# CleanTemplate
The intention of this project is to give you a head start in your next project. 
Take a look to the Architecture section to understand what this project gives you and shape it however you want.

## Requirements
- Install docker and docker-compose
- Install .NET core
- Install .NET core tools (dotnet tool install --global dotnet-ef)
- Install nodejs
- Visual Studio (Recommended for it's docker tools, if you don't want to use it review Run from another IDE section).

## Run the application

### In Visual Studio Run the docker-compose project
The easiest way to run the application is to do it from VS Studio.
Visual Studio recognizes the docker-compose project so you just need to:
- Mark docker-compose project as startup project.
- Run the project.
- Run the migrations
    - from src/Infrastructure/CleanTemplate.Infrastructure run $ dotnet ef database update
You are ready to go.

The GraphQL project uses the CleanTemplate.Auth as IdentityServer, in order to run it you need to:
- Go to docker-compose.yml file
    - Replace `${DOCKER_REGISTRY-}cleantemplategraphql` with `${DOCKER_REGISTRY-}cleantemplateapi`
    - Replace `dockerfile: ../CleanTemplate.API/Dockerfile` with `dockerfile: ../CleanTemplate.GraphQL/Dockerfile`
- Run CleanTemplate.Auth migrations from the src/Presentation/CleanTemplate.Auth folder:
    - dotnet ef database update -c AuthDbContext
    - dotnet ef database update -c ConfigurationDbContext
- Seed Auth Data
    - Send a Post request to /Account/Seed endpoint.
You are ready to go.

### Run from another IDE
#### Use the CLI
If you don't want to use VS you may want to run the project from the cli and start DB and other containers manually.
- Go the the src/Docker folder and start MariaDb, adminer and the CleanTemplate.Auth project with:
    - $ docker-compose -f docker-compose.dev.yml up --build
- If you want to remove all the containers you can run:
    - $ docker rm -f mariadb adminer CleanTemplate.Auth CleanTemplate.GraphQL CleanTemplate.API
- Run the migrations:
    - Go to src/Infrastructure/CleanTemplate.Infrastructure folder
    - run $ dotnet ef database update
- To run the dotnet projects:
    - from CleanTemplate.API run: $ dotnet watch run
    - This will start the project in the port 5000 and 5001 as defined on CleanTemplate.API/Properties/lauchSettings.json
- To run all the unit tests go to the root folder and run:
    - dotnet test ./CleanTemplate.sln
 - To run GraphQL project you need to setup CleanTemplate.Auth migrations as described on the Running on Visual Studio section.
 
##### Include the API in docker-compose
VS does a lot of things under the hood for which if you want to add it to the docker container without lossing debugging capabilities
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

### Auth project migrations
Auth project has two DbContext, one from IdentityServer4 and the project one.
To add migrations for the AuthDbContext run:
    $ dotnet ef migrations Add InitialCreate -o Persistence/Migrations -c AuthDbContext
and to run the migrations:
    $ dotnet ef database update -c AuthDbContext
To add migrations for the IdentityServer4 DbContext run: (only necessary in case of an update to the library on which the model is updated)
    $ dotnet ef migrations Add InitialCreate -o Persistence/IS4Migrations -c ConfigurationDbContext
and to run the migrations:
    $ dotnet ef database update -c ConfigurationDbContext
Please note that we are using different migration folders for each context.
Note: the first time you run the migrations you need to run AuthDbContext migrations first.

## Architecture
This project follows DDD architecture philosophy but the implementation takes many great ideas from the following repositories:
- EShop containers https://github.com/dotnet-architecture/eShopOnContainers
- jasontaylordev Clean Architecture https://github.com/jasontaylordev/CleanArchitecture
- Ardalis Clean Architecture https://github.com/ardalis/CleanArchitecture

This project is structured in the following way, at the core we have the SharedKernel which holds enterprise logic. 
On top of the SharedKernel we have the Application which holds the types and business logic that are specific to the system. 
Infrastructure groups all external concerns and finally the API is just a mean to expose the behavior defined in the Application layer.

### SharedKernel
The SharedKernel holds enterprise logic that can be shared across systems. 
It doesn't have a dependency in any other layer and it's the core of the project.
Folders should be build around context boundaries and should follow DDD principles.

### Application
Application has business logic and types that are specific for this system. This layer is intended to be the fattest one as it should 
contain all the logic. It only depends on the SharedKernel which makes it testable.
All the logic is build based on commands and queries which are grouped by concrete features of the application, you can think on each 
feature as a vertical slice **similar** to the [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/) but contained 
in this layer. This will make each Command/Query independent and more maintainable. External dependencies are abstracted in interfaces so 
there is no real dependency to any concrete technology.

This layer depends on EF, this dependency is intentional as EF already implements the unit of work pattern and exposes repositories 
as DbSets and allows us to easily switch between different data source providers. Additionally EF Core supports DDD best practices and
allows us to encapsulate behavior. 
For this reason I strongly believe that in the majority of cases an abstraction over EF is not necessary and that's why this layer relies
on EF Core. However if your use case is one of those that do need to abstract EF over a repository, I strongly recommend to take a look at 
this [Repository Interface](https://github.com/dotnet-architecture/eShopOnWeb/blob/master/src/ApplicationCore/Interfaces/IAsyncRepository.cs)
from [eShopOnWeb](https://github.com/dotnet-architecture/eShopOnWeb) Microsoft sample that follows the specification pattern.

 ### Infrastructure
 Depends on application and contains all the external concerns (ex. Persistence, API clients, etc.)
 No layer should depend on infrastructure, all concrete implementations should be added here but should cohesively be grouped
 in folders so it's easy to move them to a separate project if convenient.

### Auth
Authentication uses [Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio) to store the users and to handle authentication it uses [IdentityServer4](https://docs.identityserver.io/en/latest/index.html).
ASP.NET Core Identity allows you to manage users, passwords, profile data, roles, claims, tokens, email confirmation, and more.
IdentityServer4 is an OpenID Connect and OAuth 2.0 framework for ASP.NET Core. IdentityServer4 enables the following security features:
* Authentication as a Service (AaaS)
* Single sign-on/off (SSO) over multiple application types
* Access control for APIs
* Federation Gateway

Although we just use the bare functionality of IdentityServer as we only use it to secure the API and to provide authentication 
it is meant to be extensible so you are able to add things like Single Sign On and other features that IdentityServer supports.
IMPORTANT: We use a development certificate you should replace it with a trusted certificate for production.

### API
 API project only concern is to build the composition root and to provide a way to access our well defined views and models defined
 in the core of the system by making the API available to consumers. The only real concern this layer controls is Authentication.

**Entry Point**

 This layer shouldn't have any logic, it should only define the routes for the actions and correctly translate requests and responses
 between the client and the server.

 **Composition root**
 The Composition Root is a single, logical location in an application where modules are composed together. In this project we use
 the default DI container to build the dependency tree and associate concrete instances to abstractions.

### GraphQL
This project has the same responsibilities as the API project but it  exposes the functionality through [HotChocolate](https://hotchocolate.io/docs/introduction) GraphQL implementation.
Is quite similar to the API project but uses GraphQL instead of REST.

### Presentation
 The presentation layer in this case is implemented with the VueJS framework, this is the face of the application.
 It provides a way for users to interact with the system.

## Contents
- Startup DDD helpers
    - ValueObject is a class that can be extended, It has a default implementation that allows you to compare value objects.
    - Enumeration Allows you to create enums that have a few advantages over build in ones, specially as they can be stored
    as complex values in the DB instead of just numbers.
    - IAuditableEntity allows us to track changes to entities just by implementing this interface.
    - Entity is a base class that has a default implementation to compare entities by their Id.
- Commands and Queries. 
- Structured logging with Serilog.
    - Logs are already configured with [MariaDb sink](https://github.com/TeleSoftas/serilog-sinks-mariadb) and will be logged to the Logs table and to the console.
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
- API
    - Default API conventions https://docs.microsoft.com/en-us/aspnet/core/web-api/advanced/conventions?view=aspnetcore-3.1
    - Errors use [Problem details](https://tools.ietf.org/html/rfc7807) format but we don't use [.net problem details](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.problemdetails?view=aspnetcore-2.2) directly
    we use [AutoWrapper](https://github.com/proudmonkey/AutoWrapper) To perform the dirty work. 

- GraphQL api
    - GraphQL Api is implemented with [HotChocolate](https://hotchocolate.io/docs/introduction) implementation.
    - [Banana Cake Pop](https://hotchocolate.io/docs/banana-cakepop) is a client that goes along with HotChocolate until we setup graphiql.
- Auth
    - Oath and OpenId are exposed in the Auth project with the help of [IdentityServer4](https://docs.identityserver.io/en/latest/index.html).
    - An example of securing an Api can be seen in the GraphQL project.

- Helpful libraries that are used in the project:
    - Mediatr -> Used to dispatch our Commands and Queries. 
    IRequest implementations are automatically registered in CleanTemplate.Application/DependencyInjection.cs
    - Automapper -> Used to map between models and DTOs. 
    IMapFrom implementation are automatically registered in CleanTemplate.Application/DependencyInjection.cs
    - FluentValidator. Used to validate Commands and Queries automatically before accessing our core logic.
    Our Anti-Corruption layer consists on only using Comands and Queries with Mediatr that are validated with FluentValidation.
    AbstractValidator implementations are automatically registered in CleanTemplate.Application/DependencyInjection.cs. 
    - FakeItEasy
    - Entity Framework Core
    - Hotcholate -> Hotcholate is the library that allows us to expose our resources through GraphQL. 
    - IdentityServer4 -> Auth service is implemented with the help of IdentityServer4.
    - EF with [Pomelo](https://github.com/PomeloFoundation/Pomelo.EntityFrameworkCore.MySql) to connect with MariaDb.