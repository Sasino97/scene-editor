using System;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class MaterialDataSection : DataSection
    {
        public UInt32 Unknown1;
        public struct MaterialColor
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
        }
        public UInt32 Unknown2;
    }
}
