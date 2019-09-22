/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

using Sasinosoft.SampMapEditor.Data;
using Sasinosoft.SampMapEditor.IMG;
using Sasinosoft.SampMapEditor.RenderWare;
using Sasinosoft.SampMapEditor.RenderWare.Dff;
using Sasinosoft.SampMapEditor.RenderWare.Txd;
using Sasinosoft.SampMapEditor.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

using static Sasinosoft.SampMapEditor.Utils.MathUtils;

namespace Sasinosoft.SampMapEditor.View
{
    public partial class Editor : UserControl
    {
        public EditorViewModel ViewModel { get; private set; }

        public Editor()
        {
            InitializeComponent();
            ViewModel = DataContext as EditorViewModel;

            //
            vp3d.Camera.Position = new Point3D(2484.0, -1662.0, 40.0); // Grove Street, home...
            vp3d.Camera.LookDirection = new Vector3D(2484.0, -1562.0, 30.0);
            vp3d.Camera.UpDirection = new Vector3D(0, 0, 1);

            //
            MasterDictionary.IMGLoadCompleted += OnMasterDictionaryIMGLoadCompleted;
            MasterDictionary.IDELoadCompleted += OnMasterDictionaryIDELoadCompleted;
            MasterDictionary.IPLLoadCompleted += OnMasterDictionaryIPLLoadCompleted;
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
                            if (ViewModel.InfoObject != null)
                            {
                                ViewModel.InfoObject.IsSelected = false;
                            }

                            rwModel = (RenderWareModel)rayMeshResult.VisualHit;
                            rwModel.IsSelected = true;
                            ViewModel.InfoObject = rwModel; // temp
                            ViewModel.Info = "";
                            var tra = ViewModel.InfoObject.Transform as Transform3DGroup;
                            foreach (var t3c in tra.Children)
                            {
                                if (t3c is RotateTransform3D)
                                {
                                    var rot = t3c as RotateTransform3D;
                                    ViewModel.Info += $"{(rot.Rotation as AxisAngleRotation3D).Angle} "; // debug info
                                }
                            }
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
            var gta3Fname = Path.Combine(Properties.Settings.Default.GTASAPath, "models", "gta3.img");
            var sampFname = Path.Combine(Properties.Settings.Default.GTASAPath, "SAMP", "samp.img");

            if (!File.Exists(gta3Fname))
            {
                new SettingsWindow().ShowDialog();
            }

            // if, again, after prompting the user with the settings window, 
            // they didn't set a valid path to GTA San Andreas root folder, then:
            if (!File.Exists(gta3Fname))
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
                MasterDictionary.LoadIMG();
            }
        }

        private void CreateObjects()
        {
            var dffParser = new DffParser();
            var txdParser = new TxdParser();
            var openedArchives = new Dictionary<string, IMGArchive>();
            var parsedModels = new Dictionary<string, ExtendedSection>();
            var parsedTextures = new Dictionary<string, ExtendedSection>();

            foreach (ObjectPlacementDefinition ipl in MasterDictionary.ObjectPlacementDefinitions)
            {
                ObjectDefinition ide;
                bool exists = MasterDictionary.ObjectDefinitions.TryGetValue(ipl.Id, out ide);
                if (exists)
                {
                    double distance = Point3D.Subtract(vp3d.Camera.Position, ipl.Position).Length;
                    if (ipl.Interior == 0 && distance < 500)
                    {
                        DirEntry modelDirEntry;
                        DirEntry textureDirEntry;
                        bool modelExists = MasterDictionary.Files.TryGetValue(ide.ModelName + ".dff", out modelDirEntry);
                        bool textureExists = MasterDictionary.Files.TryGetValue(ide.TextureDictionaryName + ".txd", out textureDirEntry);

                        RenderWareModel model = null;
                        RenderWareTextureDictionary textureDictionary = null;

                        if (modelExists)
                        {
                            if (!openedArchives.ContainsKey(modelDirEntry.ArchiveName))
                                openedArchives.Add(modelDirEntry.ArchiveName, new IMGArchive(modelDirEntry.ArchiveName));

                            if (!parsedModels.ContainsKey(modelDirEntry.FileName))
                            {
                                var rwData = dffParser.Parse(openedArchives[modelDirEntry.ArchiveName], modelDirEntry.FileName);
                                model = new RenderWareModel(rwData);
                                parsedModels.Add(modelDirEntry.FileName, rwData);
                            }
                            else
                                model = new RenderWareModel(parsedModels[modelDirEntry.FileName]);
                        }

                        if (textureExists)
                        {
                            if (!openedArchives.ContainsKey(textureDirEntry.ArchiveName))
                                openedArchives.Add(textureDirEntry.ArchiveName, new IMGArchive(textureDirEntry.ArchiveName));

                            if (!parsedTextures.ContainsKey(textureDirEntry.FileName))
                            {
                                var rwData = txdParser.Parse(openedArchives[textureDirEntry.ArchiveName], textureDirEntry.FileName);
                                textureDictionary = new RenderWareTextureDictionary(rwData);
                                parsedTextures.Add(textureDirEntry.FileName, rwData);
                            }
                            else
                                textureDictionary = new RenderWareTextureDictionary(parsedTextures[textureDirEntry.FileName]);
                        }

                        if (model != null)
                        {
                            if (textureDictionary != null)
                                model.SetTextureData(textureDictionary);

                            var transformGroup = new Transform3DGroup();
                            model.Transform = transformGroup;

                            var r = QuaternionToEuler(ipl.Rotation);
                            transformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(1, 0, 0), r.X)));
                            transformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), r.Y)));
                            transformGroup.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), r.Z)));
                            transformGroup.Children.Add(new TranslateTransform3D(new Vector3D(ipl.Position.X, ipl.Position.Y, ipl.Position.Z)));
                            vp3d.Children.Add(model);
                        }
                    }
                }
            }

            foreach (IMGArchive ark in openedArchives.Values)
            {
                ark.Dispose();
            }
        }

        private void OnMasterDictionaryIMGLoadCompleted(object sender, EventArgs e)
        {
            MasterDictionary.LoadIDE();
        }

        private void OnMasterDictionaryIDELoadCompleted(object sender, EventArgs e)
        {
            MasterDictionary.LoadIPL();
        }
        private void OnMasterDictionaryIPLLoadCompleted(object sender, EventArgs e)
        {
            Dispatcher.Invoke(
                DispatcherPriority.Normal,
                new Action(() => { CreateObjects(); ViewModel.IsReady = true; })
            );
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

        private RenderWareModel infoObject;
        public RenderWareModel InfoObject
        {
            get { return infoObject; }
            set { SetProperty(ref infoObject, value); }
        }

        private string info;
        public string Info
        {
            get { return info; }
            set { SetProperty(ref info, value); }
        }

        
    }

    public static class EditorCommands
    {
        public static RoutedCommand NewCommand { get; set; } = new RoutedCommand();
    }
}
