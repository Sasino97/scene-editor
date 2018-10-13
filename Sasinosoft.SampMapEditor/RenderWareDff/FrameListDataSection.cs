using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class FrameListDataSection : DataSection
    {
        public class FrameStruct
        {
            public float[,] RotationalMatrix = new float[3, 3];
            public float[] CoordinatesOffset = new float[3];
            public UInt32 Parent;
            public UInt32 Unknown;
        }

        public UInt32 FrameCount;
        public List<FrameStruct> FrameInformation = new List<FrameStruct>();
    }
}
