using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace POE___CyberBot._01
{
    /// <summary>
    /// Interaction logic for TaskWindow.xaml
    /// </summary>
    public partial class TaskWindow : Window
    {
        public TaskWindow()
        {
            InitializeComponent();
            txtTitle.Foreground = Brushes.Gray;
            txtDescription.Foreground = Brushes.Gray;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text == "Task Title" || tb.Text == "Task Description")
            {
                tb.Text = "";
                tb.Foreground = Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tb.Text))
            {
                if (tb.Name == "txtTitle")
                    tb.Text = "Task Title";
                else if (tb.Name == "txtDescription")
                    tb.Text = "Task Description";

                tb.Foreground = Brushes.Gray;
            }
        }

        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            TaskItem task = new TaskItem
            {
                Title = txtTitle.Text,
                Description = txtDescription.Text,
                ReminderDate = dateReminder.SelectedDate,
                IsCompleted = false
            };

            lstTasks.Items.Add(task);

            // Optionally clear inputs
            txtTitle.Text = "Task Title";
            txtTitle.Foreground = Brushes.Gray;
            txtDescription.Text = "Task Description";
            txtDescription.Foreground = Brushes.Gray;
            dateReminder.SelectedDate = null;
        }

        private void MarkCompleted_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem is TaskItem selectedTask)
            {
                selectedTask.IsCompleted = true;

                // Refresh list
                lstTasks.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Please select a task to mark as completed.");
            }
        }

        private void DeleteTask_Click(object sender, RoutedEventArgs e)
        {
            if (lstTasks.SelectedItem != null)
            {
                lstTasks.Items.Remove(lstTasks.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a task to delete.");
            }
        }


    }
}
