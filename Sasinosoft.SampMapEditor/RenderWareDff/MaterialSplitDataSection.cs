using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class MaterialSplitDataSection : DataSection
    {
        public class SplitStruct
        {
            public UInt32 FaceIndex;
            public UInt32 MaterialIndex;
            public List<UInt32> Vertex1 = new List<UInt32>();
        }

        public UInt32 TriangleStrip;
        public UInt32 SplitCount;
        public UInt32 FaceCount;
        public List<SplitStruct> SplitInformation = new List<SplitStruct>();
    }
}
