using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.Services
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly string _connectionString;

        // Замените строку подключения на вашу реальную
        public DatabaseHelper(string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=TaskDB;Integrated Security=True")
        {
            _connectionString = connectionString;
        }

        public async Task<List<TaskItem>> GetTasksAsync()
        {
            var tasks = new List<TaskItem>();
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "SELECT Id, Title, Description, IsCompleted FROM Tasks";
                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tasks.Add(new TaskItem
                        {
                            Id = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                            IsCompleted = reader.GetBoolean(3)
                        });
                    }
                }
            }
            return tasks;
        }

        public async Task AddTaskAsync(TaskItem task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "INSERT INTO Tasks (Title, Description, IsCompleted) VALUES (@Title, @Description, @IsCompleted)";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", (object)task.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateTaskAsync(TaskItem task)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "UPDATE Tasks SET Title = @Title, Description = @Description, IsCompleted = @IsCompleted WHERE Id = @Id";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", task.Id);
                    command.Parameters.AddWithValue("@Title", task.Title);
                    command.Parameters.AddWithValue("@Description", (object)task.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteTaskAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string sql = "DELETE FROM Tasks WHERE Id = @Id";
                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}