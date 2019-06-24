/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.IDE;
using Sasinosoft.SampMapEditor.IMG;
using Sasinosoft.SampMapEditor.IPL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Sasinosoft.SampMapEditor.Data
{
    public static class MasterDictionary
    {
        // IMG indexing
        public static Dictionary<string, DirEntry> Files { get; private set; } = new Dictionary<string, DirEntry>();

        // IDE containers
        public static Dictionary<int, ObjectDefinition> ObjectDefinitions = new Dictionary<int, ObjectDefinition>();
        public static Dictionary<int, VehicleDefinition> VehicleDefinitions = new Dictionary<int, VehicleDefinition>();
        public static Dictionary<int, SkinDefinition> SkinDefinitions = new Dictionary<int, SkinDefinition>();
        public static Dictionary<int, WeaponDefinition> WeaponDefinitions = new Dictionary<int, WeaponDefinition>();

        // IPL containers
        public static Dictionary<int, ObjectPlacementDefinition> ObjectPlacementDefinitions = new Dictionary<int, ObjectPlacementDefinition>();
        public static Dictionary<int, VehiclePlacementDefinition> VehiclePlacementDefinitions = new Dictionary<int, VehiclePlacementDefinition>();

        // Events
        public static event EventHandler IMGLoadCompleted;
        public static event EventHandler IDELoadCompleted;
        public static event EventHandler IPLLoadCompleted;

        // Configuration
        private static readonly string[] imgFileNames =
        {
            @"models\gta3.img",
            @"models\gta_int.img",
            @"SAMP\SAMP.img"
        };
        private static readonly bool imgOverwriteDuplicates = true;

        // if a .DAT file is specified, it will read the files indicated by the "IDE" entries in that file
        private static readonly string[] ideFileNames =
        {
            @"data\default.dat",
            @"data\gta.dat",
            @"SAMP\SAMP.ide"
        };
        // if a .DAT file is specified, it will read the files indicated by the "IPL" entries in that file
        private static readonly string[] iplFileNames =
        {
            @"data\default.dat",
            @"data\gta.dat",
            @"SAMP\SAMP.ipl"
        };

        // Other
        private static Thread imgLoaderThread;
        private static Thread ideLoaderThread;
        private static Thread iplLoaderThread;

        // Static constructor
        static MasterDictionary()
        {
            imgLoaderThread = new Thread(new ThreadStart(DoLoadIMG));
            ideLoaderThread = new Thread(new ThreadStart(DoLoadIDE));
            iplLoaderThread = new Thread(new ThreadStart(DoLoadIPL));
        }

        /// <summary>
        /// Scans all the IMG files specified in the configuration and 
        /// adds them to a single .NET Dictionary.
        /// </summary>
        public static void LoadIMG()
        {
            imgLoaderThread.Start();
        }
        
        private static void DoLoadIMG()
        {
            var t = DateTime.Now;
            foreach (string imgFileName in imgFileNames)
            {
                string fname = Path.Combine(Properties.Settings.Default.GTASAPath, imgFileName);
                using (var imgArchive = new IMGArchive(fname))
                {
                    Debug.WriteLine($"Loading '{fname}'...");
                    foreach (DirEntry entry in imgArchive.DirEntries)
                    {
                        if (Files.ContainsKey(entry.FileName))
                        {
                            if (imgOverwriteDuplicates)
                            {
                                Files.Remove(entry.FileName);
                            }
                            else
                            {
                                DirEntry otherEntry;
                                Files.TryGetValue(entry.FileName, out otherEntry);
                                Debug.WriteLine($"Error! Cannot add file '{entry.FileName}' from {entry.ArchiveName} because it was already added to the dictionary from {otherEntry.ArchiveName}.");
                                continue;
                            }
                        }
                        Files.Add(entry.FileName, entry);
                        //Debug.WriteLine($"* {entry.FileName}"); // printing slows down a lot
                    }
                }
            }
            Debug.WriteLine($"Total time elapsed: {DateTime.Now - t}");
            IMGLoadCompleted?.Invoke(typeof(MasterDictionary), new EventArgs());
        }

        /// <summary>
        /// Scans all the IDE files specified in the configuration, and adds
        /// the entries to different Dictionaries according to their type.
        /// </summary>
        public static void LoadIDE()
        {
            ideLoaderThread.Start();
        }

        private static void DoLoadIDE()
        {
            var t = DateTime.Now;
            var ideParser = new IDEParser();
            var errorCount = 0;

            foreach (string ideFileName in ideFileNames)
            {
                string fname = Path.Combine(Properties.Settings.Default.GTASAPath, ideFileName);

                if (Path.GetExtension(fname).ToLowerInvariant() == ".dat")
                {
                    var content = File.ReadAllText(fname);
                    string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("IDE", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string ideFileToRead = Path.Combine(Properties.Settings.Default.GTASAPath, line.Split(' ')[1]);
                            ideParser.Parse(ideFileToRead, out int err);
                            errorCount += err;
                        }
                    }
                }
                else if (Path.GetExtension(fname).ToLowerInvariant() == ".ide")
                {
                    ideParser.Parse(fname, out int err);
                    errorCount += err;
                }
            }
            Debug.WriteLine($"Total time elapsed: {DateTime.Now - t}");

            if (errorCount > 0)
                Debug.WriteLine($"{errorCount} errors.");

            IDELoadCompleted?.Invoke(typeof(MasterDictionary), new EventArgs());
        }

        /// <summary>
        /// Scans all the IPL files specified in the configuration, and adds
        /// the entries to different Dictionaries according to their type.
        /// </summary>
        public static void LoadIPL()
        {
            iplLoaderThread.Start();
        }

        private static void DoLoadIPL()
        {
            var t = DateTime.Now;
            var iplParser = new IPLParser();
            var errorCount = 0;

            foreach (string iplFileName in iplFileNames)
            {
                string fname = Path.Combine(Properties.Settings.Default.GTASAPath, iplFileName);

                if (Path.GetExtension(fname).ToLowerInvariant() == ".dat")
                {
                    var content = File.ReadAllText(fname);
                    string[] lines = content.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("IPL", StringComparison.InvariantCultureIgnoreCase))
                        {
                            string iplFileToRead = Path.Combine(Properties.Settings.Default.GTASAPath, line.Split(' ')[1]);
                            iplParser.Parse(iplFileToRead, out int err);
                            errorCount += err;
                        }
                    }
                }
                else if (Path.GetExtension(fname).ToLowerInvariant() == ".ipl")
                {
                    iplParser.Parse(fname, out int err);
                    errorCount += err;
                }
            }
            Debug.WriteLine($"Total time elapsed: {DateTime.Now - t}");

            if (errorCount > 0)
                Debug.WriteLine($"{errorCount} errors.");

            IPLLoadCompleted?.Invoke(typeof(MasterDictionary), new EventArgs());
        }
    }
}
