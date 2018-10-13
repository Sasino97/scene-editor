using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public class GeometryDataSection : DataSection
    {
        public GeometryDataFlags Flags = GeometryDataFlags.None;
        public UInt16 Unknown1;
        public UInt32 TriangleCount;
        public UInt32 VertexCount;
        public UInt32 MorphCount;

        // version 4099
        public float Ambient;
        public float Diffuse;
        public float Specular;
        //

        // color info
        // (if Flags contain RwObjectVertexColor)
        public struct VertexColor
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
        }
        public List<VertexColor> VertexColors = new List<VertexColor>();
        //

        // texture mapping info
        // (if Flags contain RwObjectVertexUv)
        public struct VertextUV
        {
            public UInt32 U;
            public UInt32 V;
        }
        // 
    }
}
