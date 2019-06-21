/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

using System.Windows;
using System.Windows.Input;
using Sasinosoft.SampMapEditor.Utils;

namespace Sasinosoft.SampMapEditor.View
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = DataContext as MainWindowViewModel;
        }

        public void OnCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public void OnCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            // TODO: implement all commands
            if (e.Command == MainWindowCommands.NewCommand)
            {

            }
            else if (e.Command == MainWindowCommands.OpenCommand)
            {

            }
            else if (e.Command == MainWindowCommands.SaveCommand)
            {

            }
            else if (e.Command == MainWindowCommands.SaveAsCommand)
            {

            }
            else if (e.Command == MainWindowCommands.ExportCommand)
            {

            }
            else if (e.Command == MainWindowCommands.ExitCommand)
            {
                // TODO: check for saving before closing
                Application.Current.Shutdown();
            }
            else if(e.Command == MainWindowCommands.SettingsCommand)
            {
                new SettingsWindow().ShowDialog();
            }
        }
    }

    public class MainWindowViewModel : ViewModel
    {
        // Window information //
        private string title = "Sasinosoft Map Editor For SA-MP";
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        // Other information
        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
            set { SetProperty(ref isReady, value); }
        }
    }

    public static class MainWindowCommands
    {
        public static RoutedCommand NewCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand OpenCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand SaveCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand SaveAsCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand ExportCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand ExitCommand { get; set; } = new RoutedCommand();
        public static RoutedCommand SettingsCommand { get; set; } = new RoutedCommand();
    }
}
