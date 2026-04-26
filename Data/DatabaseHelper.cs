using Npgsql;
using System.Collections.Generic;
using TaskManager.Models; 

public class DatabaseHelper
{
    private static string connectionString = "Host=localhost;Port=5432;Username=postgres;Password=root;Database=p511_db";

    public static NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(connectionString);
    }

    public List<TaskItem> LoadTasks()
    {
        var tasks = new List<TaskItem>();
        using (var conn = GetConnection())
        {
            conn.Open();
            var query = "SELECT * FROM tasks ORDER BY created_date DESC"; // ОШИБКА В SQL!
            using (var cmd = new NpgsqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        IsCompleted = reader.GetBoolean(2),
                        Description = reader.IsDBNull(3) ? "" : reader.GetString(3),
                        Priority = reader.IsDBNull(4) ? 2 : reader.GetInt32(4),
                        CreatedDate = reader.GetDateTime(5)
                    });
                }
            }
            return tasks;
        }
    }
}