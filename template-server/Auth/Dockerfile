# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Auth/app/Api/Api.csproj", "Api/"]
COPY ["Auth/app/Application/Application.csproj", "Application/"]
COPY ["Auth/app/Domain/Domain.csproj", "Domain/"]
COPY ["Auth/app/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
RUN dotnet restore Api/Api.csproj -v d

COPY Auth/app/ .
COPY SharedKernel/ SharedKernel/

WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/publish

# Production image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]