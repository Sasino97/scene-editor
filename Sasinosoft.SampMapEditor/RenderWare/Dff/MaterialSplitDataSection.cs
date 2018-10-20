using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    public class MaterialSplitDataSection : DataSection
    {
        public UInt32 FaceType;
        public UInt32 SplitCount;
        public UInt32 FaceCount;

        public struct Split
        {
            public UInt32 IndexCount;
            public UInt32 Material;
            public List<UInt32> Indices;
        }
        public List<Split> Splits = new List<Split>();
    }
}
