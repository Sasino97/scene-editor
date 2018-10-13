using System.ComponentModel;
using System.Windows;

namespace Sasinosoft.SampMapEditor
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
    }
}
