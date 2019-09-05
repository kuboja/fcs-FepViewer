using System;
using System.Windows;


namespace FepViewer
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowsViewModel view;

        private readonly bool fileFromStartupArgument = false;

        public MainWindow()
        {
            InitializeComponent();

            string[] commandLineArgs = Environment.GetCommandLineArgs();

            view = new MainWindowsViewModel();
            
            if (commandLineArgs.Length >= 2)
            {
                view.FilePath = commandLineArgs[1];
                fileFromStartupArgument = true;
            }
            else
            {
                view.FilePath = Properties.Settings.Default.LastFile;
            }

            base.DataContext = view;
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            await view.LoadXml();
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            view.ExpandAllFirst();
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            await view.OpenFile();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            view.SaveSettings();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (fileFromStartupArgument && view.Autoload)
                await view.LoadXml();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            view.ResetData();
        }
    }
}
