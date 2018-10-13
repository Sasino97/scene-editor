using System;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class SectionHeader
    {
        public SectionType Type = SectionType.None;
        public UInt32 Size;
        public UInt16 Unknown;
        public UInt16 Version;
    }
}
