using System.Windows;
using System.Windows.Controls;

namespace TAAIML
{
    public class InputDialog : Window
    {
        public string ResponseText { get; private set; }
        private TextBox _inputBox;
        public InputDialog(string prompt)
        {
            Title = prompt;
            Width = 300;
            Height = 120;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var panel = new StackPanel { Margin = new Thickness(10) };
            _inputBox = new TextBox();
            var okButton = new Button { Content = "OK", Width = 60, Margin = new Thickness(0,10,0,0) };
            okButton.Click += (s, e) => { ResponseText = _inputBox.Text; DialogResult = true; };
            panel.Children.Add(_inputBox);
            panel.Children.Add(okButton);
            Content = panel;
        }
    }
}
