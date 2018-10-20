using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    // variable length
    public class GeometryDataSection : DataSection
    {
        public GeometryDataFlags Flags = GeometryDataFlags.None;
        public byte UVMapCount;
        public byte HasNativeGeometry;
        public UInt32 TriangleCount;
        public UInt32 VertexCount;
        public UInt32 MorphCount;

        // color info
        // (if Flags contain RwObjectVertexColor)
        public struct VertexColor
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
        }
        public List<VertexColor> VertexColors = new List<VertexColor>(); // VertexCount
        //

        // texture mapping info
        // (if Flags contain RwObjectVertexUv)
        public struct VertexUV
        {
            public float U;
            public float V;
        }
        public List<VertexUV> VertexUVs = new List<VertexUV>(); // VertexCount
        // 

        // face info
        public struct Triangle
        {
            public UInt16 Vertex2;
            public UInt16 Vertex1;
            public UInt16 Flags;
            public UInt16 Vertex3;
        }
        public List<Triangle> Triangles = new List<Triangle>(); // TriangleCount

        // bounding sphere info
        public float BoundingSphereX;
        public float BoundingSphereY;
        public float BoundingSphereZ;
        public float BoundingSphereRadius;
        public UInt32 HasPosition;
        public UInt32 HasNormals;
        //

        // vertex info
        public struct Vertex 
        {
            public float X;
            public float Y;
            public float Z;
        }
        public List<Vertex> Vertices = new List<Vertex>(); // VertexCount
        //

        // normal info
        // (if Flags contain RwObjectVertexNormal)
        public struct Normal
        {
            public float X;
            public float Y;
            public float Z;
        }
        public List<Normal> Normals = new List<Normal>(); // VertexCount
        //
    }
}
