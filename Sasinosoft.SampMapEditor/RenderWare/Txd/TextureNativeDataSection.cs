using System;
using System.Collections.Generic;
 
namespace Sasinosoft.SampMapEditor.RenderWare.Txd
{
    public class TextureNativeDataSection : DataSection
    {
        public UInt32 TextureVersion;
        public UInt32 FilterFlags;
        public string TextureName; // 32 bytes
        public string AlphaName; // 32 bytes
        public UInt32 AlphaFlags;
        public TextureFormats D3DTextureFormat; // 4 bytes
        public UInt16 Width;
        public UInt16 Height;
        public byte BitDepth;
        public byte MipMapCount;
        public byte TexCodeType;
        public byte Compression;

        // if BitDepth == 8
        public List<byte> Palette = new List<byte>(); // 256*4 bytes
        //

        public UInt32 DataSize;
        public List<byte> Data = new List<byte>(); // DataSize bytes

        // if MipMapCount > 1
        public struct MipMap
        {
            public UInt32 MipMapDataSize;
            public List<byte> MipMapData; // MipMapDataSize bytes
        }
        public List<MipMap> MipMaps = new List<MipMap>(); // (MipMapCount - 1)
        //
    }
}
