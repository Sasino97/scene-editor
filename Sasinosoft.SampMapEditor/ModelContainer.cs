using Sasinosoft.SampMapEditor.IMG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace Sasinosoft.SampMapEditor
{
    public static class ModelContainer
    {
        public static Dictionary<string, IMGArchiveFile> GTA3Files { get; private set; } 
            = new Dictionary<string, IMGArchiveFile>();

        public static void LoadSanAndreasArchives()
        {
            string fname = Path.Combine(Properties.Settings.Default.GTASAPath, "models", "gta3.img");
            using (var imgArchive = new IMGArchive(fname))
            {
                for(int i = 0; i < imgArchive.DirEntries.Count; i ++)
                {
                    IMGArchiveFile? archiveFile = imgArchive.GetFileById(i);
                    if (archiveFile.HasValue)
                    {
                        GTA3Files.Add(imgArchive.DirEntries[i].FileName, archiveFile.Value);
                    }
                }
            }
        }
    }
}
