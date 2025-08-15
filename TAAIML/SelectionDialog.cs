using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TAAIML
{
    public class SelectionDialog : Window
    {
        public string SelectedItem { get; private set; }
        public SelectionDialog(string prompt, List<string> items)
        {
            Title = prompt;
            Width = 300;
            Height = 200;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
            var panel = new StackPanel { Margin = new Thickness(10) };
            var listBox = new ListBox { ItemsSource = items };
            var okButton = new Button { Content = "OK", Width = 60, Margin = new Thickness(0,10,0,0) };
            okButton.Click += (s, e) => { SelectedItem = listBox.SelectedItem as string; DialogResult = true; };
            panel.Children.Add(listBox);
            panel.Children.Add(okButton);
            Content = panel;
        }
    }
}
