# Health Tracker Processor
## My first web app

This is an ASP.NET core web app, back-end of Health Tracker.

Health Tracker is my small idea, aim to track daily energy consumption, energy input and body weight.

The preliminary design is back-end will run in a container (microservice) in Azure, and database (PostgreSQL) is run in another container in Azure (or maybe use Azure PostgreSQL).

## build the back-end: 

```bash
docker build -t health-tracker-processor:latest .
```

## set up the  latest PostgreSQL container: 

```bash
docker pull postgres:latest
```

## launch PostgreSQL: 

```bash
docker run -p 5432:5432/tcp --hostname hardcore-eric --name my-web-pgsql --env POSTGRES_PASSWORD=postgres postgres
```

## Get the IP address of PostgreSQL container:

```bash
Then use docker inspect <CONTAINERID>
```

## Change the IP address of DBConnectionStr in
 
> .\MyWebApp\Models\ConfigContext.cs

## Launch application: 

```bash
docker run -rm -p 80:80/tcp health-tracker-processor:latest
```