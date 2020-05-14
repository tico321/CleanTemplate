#CleanTemplate

## Requirements
- Install docker and docker-compose
- Install .NET core
- Install nodejs

## Migrations
To run migrations navigate to CleanTemplate.Infrastructure project and then you can run the migrations like:
	$ dotnet ef migrations Add InitialCreate -o Persistence/Migrations
	$ dotnet ef database update
Migrations use the ApplicationDbContextFactory to create an instance of the DbContext, the connection string that it uses
can be found in the CleanTemplate.Infrastructure/appsettings.json file.
To recreate the database just run:
	$ dotnet ef database drop
	$ dotnet ef database update