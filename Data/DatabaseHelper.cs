using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Models;

namespace TaskManager.Interfaces;

public interface IDatabaseHelper
{
    Task<List<TaskItem>> GetTasksAsync();
    Task AddTaskAsync(TaskItem task);
    Task UpdateTaskAsync(TaskItem task);
    Task DeleteTaskAsync(int id);
}