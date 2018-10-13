using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class MaterialSplitDataSection : DataSection
    {
        public UInt32 TriangleStrip;
        public UInt32 SplitCount;
        public UInt32 FaceCount;

        public struct Split
        {
            public UInt32 FaceIndex;
            public UInt32 MaterialIndex;
            public List<UInt32> Vertex1;
        }
        public List<Split> Splits = new List<Split>();
    }
}
