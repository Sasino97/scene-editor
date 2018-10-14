using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    // variable length
    public class FrameListDataSection : DataSection
    {
        public class FrameStruct
        {
            public float[,] RotationalMatrix;
            public float[] CoordinatesOffset;
            public UInt32 Parent;
            public UInt32 Unknown1;
        }

        public UInt32 FrameCount;
        public List<FrameStruct> FrameInformation = new List<FrameStruct>();
    }
}
