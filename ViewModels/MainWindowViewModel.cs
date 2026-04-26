using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Windows;

namespace TaskManager
{
    public partial class MainWindow : Window
    {
        private DatabaseHelper dbHelper;

        public MainWindow()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadTasksToGrid();
        }

        private void LoadTasksToGrid()
        {
            try
            {
                dgvTasks.ItemsSource = dbHelper.LoadTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки задач: {ex.Message}");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtTaskTitle.Text))
            {
                try
                {
                    var selectedItem = cmbPriority.SelectedItem as ComboBoxItem;
                    int priority = selectedItem != null && int.TryParse(selectedItem.Tag?.ToString(), out int tag) ? tag : 2;

                    dbHelper.AddTask(txtTaskTitle.Text, txtDescription.Text, priority);
                    LoadTasksToGrid();
                    txtTaskTitle.Clear();
                    txtDescription.Clear();
                    cmbPriority.SelectedIndex = 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при добавлении задачи: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Введите название задачи!");
            }
        }

        private void BtnMarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTasks.SelectedItem is TaskItem selectedTask)
            {
                try
                {
                    dbHelper.MarkAsCompleted(selectedTask.Id);
                    LoadTasksToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка отметки выполнения: {ex.Message}");
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgvTasks.SelectedItem is TaskItem selectedTask)
            {
                try
                {
                    dbHelper.DeleteTask(selectedTask.Id);
                    LoadTasksToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления задачи: {ex.Message}");
                }
            }
        }
    }
}