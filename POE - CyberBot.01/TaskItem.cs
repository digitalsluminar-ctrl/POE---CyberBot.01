using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace POE___CyberBot._01
{
    public partial class TaskWindow : Window
    {
        
    }
    public class TaskItem
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? ReminderDate { get; set; }
        public bool IsCompleted { get; set; }

        public override string ToString()
        {
            string status = IsCompleted ? "✔️ Completed" : "🕒 Pending";
            string reminder = ReminderDate.HasValue ? ReminderDate.Value.ToShortDateString() : "No reminder";
            return $"{Title} - {status} - Reminder: {reminder}";
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


    }
}
