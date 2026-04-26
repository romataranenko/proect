using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaskManager.Interfaces;
using TaskManager.Models;

namespace TaskManager.ViewModels
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IDatabaseHelper _db;

        [ObservableProperty]
        private ObservableCollection<TaskItem> _tasks = new();

        [ObservableProperty]
        private string _newTaskTitle = string.Empty;

        [ObservableProperty]
        private string _newTaskDescription = string.Empty;

        public MainWindowViewModel(IDatabaseHelper db)
        {
            _db = db;
            LoadTasksCommand.Execute(null);
        }

        public IAsyncRelayCommand LoadTasksCommand => new AsyncRelayCommand(LoadTasksAsync);
        public IAsyncRelayCommand AddTaskCommand => new AsyncRelayCommand(AddTaskAsync, () => !string.IsNullOrWhiteSpace(NewTaskTitle));
        public IAsyncRelayCommand<TaskItem> DeleteTaskCommand => new AsyncRelayCommand<TaskItem>(DeleteTaskAsync);
        public IAsyncRelayCommand<TaskItem> ToggleCompleteCommand => new AsyncRelayCommand<TaskItem>(ToggleCompleteAsync);

        private async Task LoadTasksAsync()
        {
            var tasks = await _db.GetTasksAsync();
            Tasks.Clear();
            foreach (var task in tasks)
                Tasks.Add(task);
        }

        private async Task AddTaskAsync()
        {
            var newTask = new TaskItem
            {
                Title = NewTaskTitle,
                Description = NewTaskDescription
            };
            await _db.AddTaskAsync(newTask);
            NewTaskTitle = string.Empty;
            NewTaskDescription = string.Empty;
            await LoadTasksAsync();
        }

        private async Task DeleteTaskAsync(TaskItem task)
        {
            if (task != null)
            {
                await _db.DeleteTaskAsync(task.Id);
                await LoadTasksAsync();
            }
        }

        private async Task ToggleCompleteAsync(TaskItem task)
        {
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted;
                await _db.UpdateTaskAsync(task);
                await LoadTasksAsync();
            }
        }
    }
}