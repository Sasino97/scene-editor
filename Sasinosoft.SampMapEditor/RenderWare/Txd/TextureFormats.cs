using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sasinosoft.SampMapEditor.RenderWare.Txd
{
    public enum TextureFormats : UInt32
    {
        Unknown = 0x00000000,
        DXT1    = 0x31545844,
        DXT3    = 0x33545844
    }
}
