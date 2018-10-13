using System;

namespace Sasinosoft.SampMapEditor.IMG
{
    public struct DirEntry
    {
        public UInt32 Offset;
        public UInt16 Size;
        public UInt16 Size2;
        public string FileName; // 24 bytes
    }
}
