using System;
using System.Windows.Forms;
using System.Collections.Generic;

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
        dgvTasks.DataSource = dbHelper.LoadTasks();
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(txtTaskTitle.Text))
        {
            dbHelper.AddTask(txtTaskTitle.Text, txtDescription.Text, (int)nudPriority.Value);
            LoadTasksToGrid();
            txtTaskTitle.Clear();
            txtDescription.Clear();
            nudPriority.Value = 2;
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
            int taskId = (int)dgvTasks.SelectedRows[0].Cells["Id"].Value;
            dbHelper.MarkAsCompleted(taskId);
            LoadTasksToGrid();
        }
    }

    private void btnDelete_Click(object sender, EventArgs e)
    {
        if (dgvTasks.SelectedRows.Count > 0)
        {
            int taskId = (int)dgvTasks.SelectedRows[0].Cells["Id"].Value;
            dbHelper.DeleteTask(taskId);
            LoadTasksToGrid();
        }
    }
}