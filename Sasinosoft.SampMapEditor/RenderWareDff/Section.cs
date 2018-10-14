using System;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public abstract class Section
    {
        public const int HEADER_SIZE = 12;

        public SectionType Type = SectionType.None;
        public UInt32 Size;
        public UInt16 Unknown;
        public UInt16 Version;
    }
}
