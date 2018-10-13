﻿using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; private set; }

        private Point centerOfViewport;

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
        }

        public void OnViewportMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // TODO: (selection)
            }
            else if(e.ChangedButton == MouseButton.Right)
            {
                centerOfViewport = canvas.PointToScreen(new Point(canvas.ActualWidth / 2, canvas.ActualHeight / 2));
                MouseUtils.SetPosition(centerOfViewport);
                Cursor = Cursors.None;
            }
        }

        public void OnViewportMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // TODO: (selection)
            }
        }

        public void OnViewportMouseMove(object sender, MouseEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                Point relativePos = Mouse.GetPosition(canvas);
                Point actualRelativePos = new Point(relativePos.X - canvas.ActualWidth / 2,
                                                    canvas.ActualHeight / 2 - relativePos.Y);

                ViewModel.RotateCamera(actualRelativePos.X / 200, actualRelativePos.Y / 200);
                MouseUtils.SetPosition(centerOfViewport);
            }
        }

        public void OnViewportMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ViewModel.ZoomCamera(e.Delta / 110);
        }

        public void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                Cursor = Cursors.Arrow;
            }
        }
    }
}
