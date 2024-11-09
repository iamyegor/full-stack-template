using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

ApplicationContext dbContext = new ApplicationContext();
dbContext.Database.Migrate();

Console.WriteLine("Database is migrated");