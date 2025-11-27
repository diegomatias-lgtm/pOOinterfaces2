using System;
using System.Data;
using Microsoft.Data.Sqlite;

namespace School.Persistence.AdoNet.Sqlite.Connections;

public sealed class SqliteConnectionFactory
{
    private readonly string _connectionString;
    public SqliteConnectionFactory(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public IDbConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
