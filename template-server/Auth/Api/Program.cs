using Api;
using Application;
using Infrastructure.Data;
using Infrastructure.Data.Dapper;
using Infrastructure.Data.Helpers;

DapperConfiguration.ConfigureSnakeCaseMapping(typeof(IApplication).Assembly);
WebApplication app = WebApplication.CreateBuilder(args).ConfigureServices().ConfigureMiddlewares();

await DbAvailabilityChecker.WaitForDatabaseAsync(app.Services);

app.Run();
