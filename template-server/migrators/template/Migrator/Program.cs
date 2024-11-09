using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;

ApplicationContext dbContext = new ApplicationContext();
dbContext.Database.Migrate();

Console.WriteLine("Database is migrated");