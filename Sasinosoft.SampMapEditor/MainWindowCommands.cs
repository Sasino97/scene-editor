using System.Windows.Input;

namespace Sasinosoft.SampMapEditor
{
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
