using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;

namespace Sasinosoft.SampMapEditor.View
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void OnBrowseButtonClick(object sender, RoutedEventArgs e)
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            dialog.Description = "Select any valid GTA San Andreas installation directory";
            if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Properties.Settings.Default.GTASAPath = dialog.SelectedPath;
            }
        }

        private void OnSaveButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
