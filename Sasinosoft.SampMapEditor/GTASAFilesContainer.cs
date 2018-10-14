using Sasinosoft.SampMapEditor.IMG;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Sasinosoft.SampMapEditor
{
    public static class GTASAFilesContainer
    {
        public static Dictionary<string, IMGArchiveFile> GTA3Files { get; private set; }
            = new Dictionary<string, IMGArchiveFile>();

        public static event EventHandler LoadCompleted;
        public static event EventHandler ProgressChanged;

        private enum WorkerOperation
        {
            None,
            LoadGTA3Img
        }
        private static BackgroundWorker worker;

        static GTASAFilesContainer()
        {
            worker = new BackgroundWorker();
            worker.DoWork += OnWorkerDoWork;
            worker.ProgressChanged += OnWorkerProgressChanged;
            worker.RunWorkerCompleted += OnWorkerRunWorkerCompleted;
        }

        public static void Load()
        {
            worker.RunWorkerAsync(WorkerOperation.LoadGTA3Img);
        }

        private static void OnWorkerRunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadCompleted?.Invoke(typeof(GTASAFilesContainer), new EventArgs());
        }

        private static void OnWorkerProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(typeof(GTASAFilesContainer), new EventArgs());
        }

        private static void OnWorkerDoWork(object sender, DoWorkEventArgs e)
        {
            switch((WorkerOperation)e.Argument)
            {
                case WorkerOperation.LoadGTA3Img:
                    string fname = Path.Combine(Properties.Settings.Default.GTASAPath, "models", "gta3.img");
                    using (var imgArchive = new IMGArchive(fname))
                    {
                        for (int i = 0; i < imgArchive.DirEntries.Count; i++)
                        {
                            if (e.Cancel)
                                break;

                            IMGArchiveFile? archiveFile = imgArchive.GetFileById(i);
                            if (archiveFile.HasValue)
                            {
                                GTA3Files.Add(imgArchive.DirEntries[i].FileName, archiveFile.Value);
                            }
                        }
                    }
                    break;
            }
        }

        // TODO: if another instance of Sasinosoft.SampMapEditor is already running
        // then don't load again 500MB of stuff into memory, but access to the data
        // stored in the other process.
    }
}
