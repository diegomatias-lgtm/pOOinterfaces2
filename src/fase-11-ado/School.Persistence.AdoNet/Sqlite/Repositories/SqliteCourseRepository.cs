using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using School.Domain.Entities;
using School.Domain.Repositories;
using School.Persistence.AdoNet.Sqlite.Connections;

namespace School.Persistence.AdoNet.Sqlite.Repositories;

public class SqliteCourseRepository : ICourseRepository
{
    private readonly SqliteConnectionFactory _connectionFactory;

    public SqliteCourseRepository(SqliteConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        EnsureSchema();
    }

    private void EnsureSchema()
    {
        using var c = _connectionFactory.CreateConnection();
        using var cmd = c.CreateCommand();
        cmd.CommandText = @"CREATE TABLE IF NOT EXISTS Courses (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Name TEXT NOT NULL,
            WorkloadHours INTEGER NOT NULL,
            IsActive INTEGER NOT NULL
        );";
        cmd.ExecuteNonQuery();
    }

    public void Add(Course entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        using var c = _connectionFactory.CreateConnection();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "INSERT INTO Courses (Name, WorkloadHours, IsActive) VALUES (@Name, @WorkloadHours, @IsActive); SELECT last_insert_rowid();";
        var pName = cmd.CreateParameter(); pName.ParameterName = "@Name"; pName.DbType = DbType.String; pName.Value = entity.Name; cmd.Parameters.Add(pName);
        var pWork = cmd.CreateParameter(); pWork.ParameterName = "@WorkloadHours"; pWork.DbType = DbType.Int32; pWork.Value = entity.WorkloadHours; cmd.Parameters.Add(pWork);
        var pIsActive = cmd.CreateParameter(); pIsActive.ParameterName = "@IsActive"; pIsActive.DbType = DbType.Int32; pIsActive.Value = entity.IsActive ? 1 : 0; cmd.Parameters.Add(pIsActive);
        var result = cmd.ExecuteScalar();
        if (result is long id) entity.Id = (int)id;
    }

    public Course? GetById(int id)
    {
        using var c = _connectionFactory.CreateConnection();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, WorkloadHours, IsActive FROM Courses WHERE Id = @Id;";
        var p = cmd.CreateParameter(); p.ParameterName = "@Id"; p.DbType = DbType.Int32; p.Value = id; cmd.Parameters.Add(p);
        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;
        return MapCourse(reader);
    }

    public IReadOnlyList<Course> ListAll()
    {
        using var c = _connectionFactory.CreateConnection();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "SELECT Id, Name, WorkloadHours, IsActive FROM Courses;";
        using var reader = cmd.ExecuteReader();
        var list = new List<Course>();
        while (reader.Read()) list.Add(MapCourse(reader));
        return list;
    }

    public IReadOnlyList<Course> Find(Func<Course, bool> predicate)
    {
        if (predicate is null) throw new ArgumentNullException(nameof(predicate));
        return ListAll().Where(predicate).ToList().AsReadOnly();
    }

    public void Update(Course entity)
    {
        if (entity is null) throw new ArgumentNullException(nameof(entity));
        using var c = _connectionFactory.CreateConnection();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "UPDATE Courses SET Name=@Name, WorkloadHours=@WorkloadHours, IsActive=@IsActive WHERE Id=@Id;";
        var pId = cmd.CreateParameter(); pId.ParameterName = "@Id"; pId.DbType = DbType.Int32; pId.Value = entity.Id; cmd.Parameters.Add(pId);
        var pName = cmd.CreateParameter(); pName.ParameterName = "@Name"; pName.DbType = DbType.String; pName.Value = entity.Name; cmd.Parameters.Add(pName);
        var pWork = cmd.CreateParameter(); pWork.ParameterName = "@WorkloadHours"; pWork.DbType = DbType.Int32; pWork.Value = entity.WorkloadHours; cmd.Parameters.Add(pWork);
        var pIsActive = cmd.CreateParameter(); pIsActive.ParameterName = "@IsActive"; pIsActive.DbType = DbType.Int32; pIsActive.Value = entity.IsActive ? 1 : 0; cmd.Parameters.Add(pIsActive);
        cmd.ExecuteNonQuery();
    }

    public void Remove(int id)
    {
        using var c = _connectionFactory.CreateConnection();
        using var cmd = c.CreateCommand();
        cmd.CommandText = "DELETE FROM Courses WHERE Id=@Id;";
        var p = cmd.CreateParameter(); p.ParameterName = "@Id"; p.DbType = DbType.Int32; p.Value = id; cmd.Parameters.Add(p);
        cmd.ExecuteNonQuery();
    }

    private static Course MapCourse(IDataRecord r)
    {
        return new Course
        {
            Id = r.GetInt32(r.GetOrdinal("Id")),
            Name = r.GetString(r.GetOrdinal("Name")),
            WorkloadHours = r.GetInt32(r.GetOrdinal("WorkloadHours")),
            IsActive = r.GetInt32(r.GetOrdinal("IsActive")) != 0
        };
    }
}
