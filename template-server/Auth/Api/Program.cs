using Api;
using Application;
using Infrastructure.Data;
using Infrastructure.Data.Dapper;

DapperConfiguration.ConfigureSnakeCaseMapping(typeof(IApplication).Assembly);
WebApplication app = WebApplication.CreateBuilder(args).ConfigureServices().ConfigureMiddlewares();

await DatabaseAvailabilityChecker.WaitForDatabaseAsync(app.Services);

app.Run();
