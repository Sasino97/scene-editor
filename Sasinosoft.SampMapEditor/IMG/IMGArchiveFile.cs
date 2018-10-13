using System;

namespace Sasinosoft.SampMapEditor.IMG
{
    public struct IMGArchiveFile
    {
        public DirEntry FileEntry;
        public UInt64 FileOffset;
        public UInt64 FileSize;
        public byte[] FileByteBuffer;
    }
}
