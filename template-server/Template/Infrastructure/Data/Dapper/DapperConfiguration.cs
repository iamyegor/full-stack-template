using System.Reflection;
using Dapper;
using Infrastructure.Data.Dapper.Extensions;

namespace Infrastructure.Data.Dapper;

public class DapperConfiguration
{
    public static void ConfigureSnakeCaseMapping(Assembly assembly)
    {
        foreach (Type model in assembly.GetTypes())
        {
            SqlMapper.SetTypeMap(
                model,
                new CustomPropertyTypeMap(
                    model,
                    (modelType, columnName) =>
                        modelType
                            .GetProperties()
                            .FirstOrDefault(prop => prop.Name.ToSnakeCase() == columnName)!
                )
            );
        }
    }
}
