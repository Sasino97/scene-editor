using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    [Flags]
    public enum GeometryDataFlags : UInt16
    {
        None = 0x00,
        RwObjectVertexTristrip = 0x01,
        RwObjectVertexPositions = 0x02,
        RwObjectVertexUv = 0x04,
        RwObjectVertexColor = 0x08,
        RwObjectVertexNormal = 0x10,
        RwObjectVertexLight = 0x20,
        RwObjectVertexModulate = 0x40,
        RwObjectVertexTextured = 0x80
    }
}
