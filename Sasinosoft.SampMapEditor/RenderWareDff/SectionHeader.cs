using System;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class SectionHeader
    {
        public UInt32 Type = SectionType.RwData;
        public UInt32 Size;
        public UInt16 Unknown;
        public UInt16 Version;
    }
}
