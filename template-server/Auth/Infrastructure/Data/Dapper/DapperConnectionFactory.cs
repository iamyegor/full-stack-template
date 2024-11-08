using Infrastructure.Data.Helpers;
using Npgsql;

namespace Infrastructure.Data.Dapper;

public class DapperConnectionFactory
{
    private readonly string _connectionString;

    public DapperConnectionFactory(ConnectionStringResolver connectionStringResolver)
    {
        _connectionString = connectionStringResolver.GetBasedOnEnvironment();
    }

    public NpgsqlConnection Create()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
