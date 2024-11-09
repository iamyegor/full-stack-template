# Build Image
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Template/app/Api/Api.csproj", "Api/"]
COPY ["Template/app/Application/Application.csproj", "Application/"]
COPY ["Template/app/Domain/Domain.csproj", "Domain/"]
COPY ["Template/app/Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]
RUN dotnet restore Api/Api.csproj -v d

COPY Template/app/ .
COPY SharedKernel/ SharedKernel/

WORKDIR /src/Api
RUN dotnet publish -c Release -o /app/publish

# Production image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Api.dll"]