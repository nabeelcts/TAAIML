using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TAAIML.Core.Services;
using System.Collections.Generic;

namespace TAAIML
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TaskRecorderService _recorder = new();
        private TaskPlayerService _player = new();
        private bool _isRecording = false;
        private bool _isReplaying = false;

        public MainWindow()
        {
            InitializeComponent();
            ReplayButton.IsEnabled = true;
            RecordingIndicator.Visibility = Visibility.Collapsed;
            ReplayIndicator.Visibility = Visibility.Collapsed;
            this.MouseMove += MainWindow_MouseMove;
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(this);
            MousePositionLabel.Content = $"Mouse: X={position.X:F0}, Y={position.Y:F0}";
        }

        private void SwitchToWidget(string mode)
        {
            NormalViewPanel.Visibility = Visibility.Collapsed;
            WidgetPanel.Visibility = Visibility.Visible;
            this.Width = 220;
            this.Height = 120;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = SystemParameters.WorkArea.Right - this.Width - 10;
            this.Top = SystemParameters.WorkArea.Top + 10;
            if (mode == "recording")
            {
                WidgetIndicator.Text = "Recording...";
                WidgetIndicator.Foreground = Brushes.Red;
                WidgetStopButton.Content = "Stop Recording";
            }
            else if (mode == "replaying")
            {
                WidgetIndicator.Text = "Replaying...";
                WidgetIndicator.Foreground = Brushes.Blue;
                WidgetStopButton.Content = "Stop Replay";
            }
        }

        private void SwitchToNormal()
        {
            NormalViewPanel.Visibility = Visibility.Visible;
            WidgetPanel.Visibility = Visibility.Collapsed;
            this.Width = 600;
            this.Height = 350;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void StartRecording_Click(object sender, RoutedEventArgs e)
        {
            _recorder.StartRecording();
            _isRecording = true;
            SwitchToWidget("recording");
        }

        private void StopRecording_Click(object sender, RoutedEventArgs e)
        {
            _recorder.StopRecording();
            _isRecording = false;
            SwitchToNormal();
            EventListBox.ItemsSource = null;
            EventListBox.ItemsSource = _recorder.Events;
        }

        private void Replay_Click(object sender, RoutedEventArgs e)
        {
            _isReplaying = true;
            SwitchToWidget("replaying");
            _player.Play(_recorder.Events);
            _isReplaying = false;
            SwitchToNormal();
        }

        private void WidgetStopButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isRecording)
            {
                StopRecording_Click(sender, e);
            }
            else if (_isReplaying)
            {
                SwitchToNormal();
                _isReplaying = false;
            }
        }

        private void SaveTask_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new InputDialog("Enter Task Name:");
            if (inputDialog.ShowDialog() == true)
            {
                TAAIML.Core.Services.TaskManagerService.SaveTask(inputDialog.ResponseText, _recorder.Events);
                MessageBox.Show("Task saved!");
            }
        }

        private void LoadTask_Click(object sender, RoutedEventArgs e)
        {
            var tasks = TAAIML.Core.Services.TaskManagerService.GetAvailableTasks();
            var selectionDialog = new SelectionDialog("Select a task:", tasks);
            if (selectionDialog.ShowDialog() == true)
            {
                var selected = selectionDialog.SelectedItem;
                var events = TAAIML.Core.Services.TaskManagerService.LoadTask(selected);
                _player.Play(events);
            }
        }
    }
}