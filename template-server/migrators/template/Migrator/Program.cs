using Microsoft.EntityFrameworkCore;
using Migrator;

ApplicationContext dbContext = new ApplicationContext();
dbContext.Database.Migrate();

Console.WriteLine("Database is migrated");