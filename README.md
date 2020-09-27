# Health Tracker Processor
My first web app

This is an ASP.NET core web app, back-end of Health Tracker.

Health Tracker is my small idea, aim to track daily energy consumption, energy input and body weight.

The preliminary design is back-end will run in a container (microservice) in Azure, and database (PostgreSQL) is run in another container in Azure (or maybe use Azure PostgreSQL).

To build the back-end, please run: 
    docker build -t health-tracker-processor:latest .

To set up the database, please pull the latest PostgreSQL: 
    docker pull postgres:latest

To run the application, please launch PostgreSQL firstly: 
    docker run -p 5432:5432/tcp --hostname hardcore-eric --name my-web-pgsql --env POSTGRES_PASSWORD=postgres postgres

Then use
    docker inspect <CONTAINERID> 
to find the IP address of PostgreSQL container

Then change the IP address of DBConnectionStr in .\MyWebApp\Models\ConfigContext.cs

Save your change, then run 
    docker run -rm -p 80:80/tcp health-tracker-processor:latest
