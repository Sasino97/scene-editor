﻿/*
 * Thanks to whoever wrote the code which is shown in this page:
 * https://www.grandtheftwiki.com/IMG_Archive#Version_2_-_GTA_SA
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Sasinosoft.SampMapEditor.IMG
{
    public class IMGArchive : IDisposable
    {
        private Stream stream;
        public List<DirEntry> DirEntries { get; private set; } = new List<DirEntry>();

        public IMGArchive(Stream stream)
        {
            this.stream = stream;
            var binReader = new BinaryReader(stream);

            byte[] version = binReader.ReadBytes(4);
            if (version[0] == 'V' && version[3] == '2')
            {
                UInt32 entryCount = binReader.ReadUInt32();
                for(int i = 0; i < entryCount; i ++)
                {
                    DirEntry dirEntry = new DirEntry
                    {
                        Offset = binReader.ReadUInt32(),
                        Size = binReader.ReadUInt16(),
                        Size2 = binReader.ReadUInt16(),
                        FileName = new string(binReader.ReadChars(24))
                    };
                    dirEntry.FileName = dirEntry.FileName.Remove(dirEntry.FileName.IndexOf('\0'));
                    DirEntries.Add(dirEntry);
                }
            }
            else
                throw new UnknownIMGVersionException();
        }

        // can throw an exception if the file path is not valid
        public IMGArchive(string filePath) 
            : this(new FileStream(filePath, FileMode.Open, FileAccess.Read)) { }

        public void Dispose()
        {
            stream.Dispose();
        }

        public Nullable<IMGArchiveFile> GetFileById(int id, bool buffer=true)
        {
            if(id >= DirEntries.Count)
                return null;

            IMGArchiveFile archiveFile = new IMGArchiveFile()
            {
                FileEntry = DirEntries[id],
                FileOffset = (ulong)DirEntries[id].Offset * 2048,
                FileSize = (ulong)DirEntries[id].Size * 2048,
            };

            if(buffer)
            {
                var binReader = new BinaryReader(stream);
                binReader.BaseStream.Position = (long)archiveFile.FileOffset;
                archiveFile.FileByteBuffer = binReader.ReadBytes((int)archiveFile.FileSize);
            }
            return archiveFile;
        }

        public Nullable<IMGArchiveFile> GetFileByName(string name, bool buffer=true)
        {
            DirEntry? entry = DirEntries
                .Where(e => e.FileName == name)
                .Cast<DirEntry?>()
                .FirstOrDefault();

            if (!entry.HasValue)
                return null;

            return GetFileById(DirEntries.IndexOf(entry.Value), buffer);
        }
    }
}
