using System;
using System.Windows;


namespace FepViewer
{
    public partial class MainWindow : Window
    {
        private MainWindowsViewModel view { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            string[] commandLineArgs = Environment.GetCommandLineArgs();


            view = new MainWindowsViewModel();
            
            if (commandLineArgs.Length >= 2)
            {
                view.FilePath = commandLineArgs[1];
            }
            else
            {
                view.FilePath = "C:\\Temp\\expressionSpeed\\log2018_0318_225226.xml";
            }

            base.DataContext = view;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            view.LoadXml();
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            view.ExpandAllFirst();
        }
    }
}
