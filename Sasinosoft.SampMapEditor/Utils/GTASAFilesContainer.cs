/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.IMG;
using Sasinosoft.SampMapEditor.RenderWare;
using Sasinosoft.SampMapEditor.RenderWare.Dff;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Sasinosoft.SampMapEditor.Utils
{
    public static class GTASAFilesContainer
    {
        public static Dictionary<string, IMGArchiveFile> GTA3Files { get; private set; }
            = new Dictionary<string, IMGArchiveFile>();

        public static event EventHandler LoadCompleted;
        public static event EventHandler ProgressChanged;

        private static Thread loaderThread;

        static GTASAFilesContainer()
        {
            loaderThread = new Thread(new ThreadStart(DoWork), 5000000);
        }

        public static void Load()
        {
            loaderThread.Start();
        }
        
        private static void DoWork()
        {
            string fname = Path.Combine(Properties.Settings.Default.GTASAPath, "models", "gta3.img");
            using (var imgArchive = new IMGArchive(fname))
            {
                var parser = new DffParser();
                var list = new List<RenderWareModel>();
                var startTimestamp = DateTime.Now;
                for (int i = 0; i < imgArchive.DirEntries.Count; i++)
                {
                    IMGArchiveFile? archiveFile = imgArchive.GetFileById(i, false);
                    if (archiveFile.HasValue)
                    {
                        if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".dff")
                        {
                            archiveFile = imgArchive.GetFileById(i, true);
                            Debug.WriteLine(archiveFile.Value.FileEntry.FileName);
                            list.Add(parser.Parse(archiveFile.Value.FileByteBuffer));
                            ProgressChanged?.Invoke(typeof(GTASAFilesContainer), new EventArgs());
                        }
                    }
                }
                Debug.WriteLine("Time elapsed: {0}", DateTime.Now-startTimestamp);
                //for (int i = 0; i < imgArchive.DirEntries.Count; i++)
                //{
                //    if (e.Cancel)
                //        break;

                //    IMGArchiveFile? archiveFile = imgArchive.GetFileById(i);
                //    if (archiveFile.HasValue)
                //    {
                //        GTA3Files.Add(imgArchive.DirEntries[i].FileName, archiveFile.Value);
                //    }
                //}
            }
            LoadCompleted?.Invoke(typeof(GTASAFilesContainer), new EventArgs());
        }

        // TODO: if another instance of Sasinosoft.SampMapEditor is already running
        // then don't load again 500MB of stuff into memory, but access to the data
        // stored in the other process.
    }
}
