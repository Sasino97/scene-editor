using System;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public enum SectionType : UInt32
    {
        None = 0,
        RwData = 1,
        RwString = 2,
        RwExtension = 3,
        RwTexture = 6,
        RwMaterial = 7,
        RwMaterialList = 8,
        RwFrameList = 14,
        RwGeometry = 15,
        RwClump = 16,
        RwAtomic = 20,
        RwGeometryList = 26,
        RwAnimPlugin = 286,
        RwMaterialEffects = 288,
        RwMaterialSplit = 1294,
        RwFrame = 39056126,
        RwNvColors = 39056121
    }
}
