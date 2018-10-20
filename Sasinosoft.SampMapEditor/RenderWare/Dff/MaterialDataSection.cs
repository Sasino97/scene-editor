using System;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    // 28 bytes
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
        public MaterialColor Color;
        public UInt32 Unknown2;
        public UInt32 TextureCount;
        public float[] Unknown3;
    }
}
