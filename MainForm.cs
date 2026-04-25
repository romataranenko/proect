using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Data;

public partial class MainForm : Form
{
    private DatabaseHelper dbHelper;

    public MainForm()
    {
        InitializeComponent();
        dbHelper = new DatabaseHelper();
        LoadTasksToGrid();
    }

    private void LoadTasksToGrid()
    {
        try
        {
            dgvTasks.DataSource = dbHelper.LoadTasks();
            dgvTasks.Refresh(); // Обновление отображения
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Ошибка загрузки задач: {ex.Message}");
        }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtTaskTitle.Text))
        {
            try
            {
                dbHelper.AddTask(txtTaskTitle.Text, txtDescription.Text, (int)nudPriority.Value);
                LoadTasksToGrid();
                txtTaskTitle.Clear();
                txtDescription.Clear();
                nudPriority.Value = 2;
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

    private void btnMarkCompleted_Click(object sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count > 0)
        {
            var cellValue = dgvTasks.SelectedRows[0].Cells["Id"].Value;
            if (cellValue != null && int.TryParse(cellValue.ToString(), out int taskId))
            {
                try
                {
                    dbHelper.MarkAsCompleted(taskId);
                    LoadTasksToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка отметки задачи: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить ID задачи.");
            }
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count > 0)
        {
            var cellValue = dgvTasks.SelectedRows[0].Cells["Id"].Value;
            if (cellValue != null && int.TryParse(cellValue.ToString(), out int taskId))
            {
                try
                {
                    dbHelper.DeleteTask(taskId);
                    LoadTasksToGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления задачи: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Не удалось получить ID задачи.");
            }
        }
    }
}