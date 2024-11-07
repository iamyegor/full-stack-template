using Api;
using Application;
using Infrastructure.Data.Dapper;

DapperConfiguration.ConfigureSnakeCaseMapping(typeof(IApplication).Assembly);
WebApplication.CreateBuilder(args).ConfigureServices().ConfigureMiddlewares().Run();