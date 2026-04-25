using System;

namespace TaskManager.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
