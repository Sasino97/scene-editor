/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

using Sasinosoft.SampMapEditor.IMG;
using Sasinosoft.SampMapEditor.RenderWare;
using Sasinosoft.SampMapEditor.RenderWare.Dff;
using Sasinosoft.SampMapEditor.RenderWare.Txd;
using Sasinosoft.SampMapEditor.Utils;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.View
{
    public partial class Editor : UserControl
    {
        public EditorViewModel ViewModel { get; private set; }

        public Editor()
        {
            InitializeComponent();
            ViewModel = DataContext as EditorViewModel;
        }

        private void OnViewportPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                var rayMeshResult = VisualTreeHelper.HitTest(vp3d, e.GetPosition(vp3d))
                    as RayMeshGeometry3DHitTestResult;

                if (rayMeshResult != null)
                {
                    foreach (Visual3D visual in vp3d.Children)
                    {
                        if (rayMeshResult.VisualHit is RenderWareModel rwModel)
                        {
                            rwModel = (RenderWareModel)rayMeshResult.VisualHit;
                            rwModel.IsSelected = true;
                            break;
                        }
                    }
                }
                e.Handled = true;
            }
        }

        private void OnViewportPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {

            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            var fname = Path.Combine(Properties.Settings.Default.GTASAPath, "models", "gta3.img");
            if (!File.Exists(fname))
            {
                new SettingsWindow().ShowDialog();
            }

            // if, again, after prompting the user with the settings window, 
            // they didn't set a valid path to GTA San Andreas root folder, then:
            if (!File.Exists(fname))
            {
                MessageBox.Show(
                    "You must specify a valid GTA San Andreas installation folder. Unable to load model data.",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            else
            {
                // GTASAFilesContainer.Load();
                // GTASAFilesContainer.LoadCompleted += OnGTASAFilesContainerLoadCompleted;

                var dffParser = new DffParser();
                var txdParser = new TxdParser();
                //string modelPath = @"C:\Users\Salvatore\Documents\shamal.dff";
                using (var archive = new IMGArchive(fname))
                {
                    var model = dffParser.Parse(archive, "vla1.dff");
                    var texture = txdParser.Parse(archive, "vla1.txd");
                    model.SetTextureData(texture);

                    var transformGroup = new Transform3DGroup();
                    model.Transform = transformGroup;
                    transformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), 0.0)));
                    transformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), 270.0)));
                    transformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 270.0)));
                    transformGroup.Children.Add(new TranslateTransform3D(0.0, 0.0, 1.0));

                    vp3d.Children.Add(model);
                    ViewModel.IsReady = true;
                }
            }
        }

        private void OnGTASAFilesContainerLoadCompleted(object sender, System.EventArgs e)
        {
            ViewModel.IsReady = true;
        }
    }

    public class EditorViewModel : ViewModel
    {
        private bool isReady = false;
        public bool IsReady
        {
            get { return isReady; }
            set { SetProperty(ref isReady, value); }
        }
    }

    public static class EditorCommands
    {
        public static RoutedCommand NewCommand { get; set; } = new RoutedCommand();
    }
}
