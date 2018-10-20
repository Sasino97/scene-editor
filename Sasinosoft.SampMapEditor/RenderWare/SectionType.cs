using System;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public enum SectionType : UInt32
    {
        None                = 0x00000000,
        RwData              = 0x00000001,
        RwString            = 0x00000002,
        RwExtension         = 0x00000003,
        RwTexture           = 0x00000006,
        RwMaterial          = 0x00000007,
        RwMaterialList      = 0x00000008,
        RwFrameList         = 0x0000000E,
        RwGeometry          = 0x0000000F,
        RwClump             = 0x00000010,
        RwLight             = 0x00000012,
        RwAtomic            = 0x00000014,
        RwTextureNative     = 0x00000015,
        RwTextureDictionary = 0x00000016,
        RwGeometryList      = 0x0000001A,
        RwRightToRender     = 0x0000001F,
        RwSkinPlugin        = 0x00000116,
        RwAnimPlugin        = 0x0000011E,
        RwMaterialEffects   = 0x00000120,
        RwMaterialSplit     = 0x0000050E,
        RwFrame             = 0x0253F2FE,
        RwNvColors          = 0x0253F2F9
    }
}
