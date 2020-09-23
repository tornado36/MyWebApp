FROM mcr.microsoft.com/dotnet/core/sdk:3.1

EXPOSE 80

WORKDIR /app

COPY MyWebApp/ ./MyWebApp
COPY MyWebAppCore/ ./MyWebAppCore
COPY MyWebApp.sln .

#RUN dotnet restore MyWebApp.sln -s packages -s https://api.nuget.org/v3/index.json

RUN dotnet build MyWebApp/MyWebApp.csproj -c release -f netcoreapp3.1 -o /out
RUN dotnet build MyWebAppCore/MyWebAppCore.csproj -c release -f netcoreapp3.1 -o /out

WORKDIR /out

ENTRYPOINT ["dotnet", "MyWebApp.dll"]