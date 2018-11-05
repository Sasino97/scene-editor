/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System.Windows.Input;

namespace Sasinosoft.SampMapEditor.View
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
