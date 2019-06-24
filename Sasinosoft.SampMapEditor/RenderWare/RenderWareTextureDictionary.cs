/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using ManagedSquish;
using Sasinosoft.SampMapEditor.RenderWare.Txd;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.RenderWare
{
    public class RenderWareTextureDictionary
    {
        public Dictionary<string, Material> MaterialDictionary { get; } 
            = new Dictionary<string, Material>();

        public RenderWareTextureDictionary(ExtendedSection clump)
        {
            foreach (Section section in clump.GetChildren(SectionType.RwTextureNative))
            {
                var textureInfo = (TextureNativeDataSection) ((ExtendedSection)section).GetChild(0);
                var brush = new ImageBrush()
                {
                    ViewportUnits = BrushMappingMode.Absolute,
                };

                // set the brush to image data after decoding the format
                if (textureInfo.D3DTextureFormat == TextureFormats.DXT1)
                {
                    // decode dxt1
                    byte[] decompressedImage = Squish.DecompressImage(
                        textureInfo.Data.ToArray(),
                        (int)textureInfo.Width,
                        (int)textureInfo.Height,
                        SquishFlags.Dxt1
                    );

                    PixelFormat pixelFormat = PixelFormats.Bgr565;
                    int width = (int)textureInfo.Width;
                    int height = (int)textureInfo.Height;
                    int rawStride = (width * pixelFormat.BitsPerPixel + 7) / 8;

                    if (rawStride * height != textureInfo.DataSize)
                    {
                        //Debug.WriteLine("Data Length Error. {0} != {1}",
                        //    rawStride * height,
                        //    textureInfo.DataSize);
                        return;
                    }

                    BitmapSource bmpSrc = BitmapSource.Create(
                        width,
                        height,
                        96,
                        96,
                        pixelFormat,
                        null,
                        decompressedImage,
                        rawStride
                    );

                    brush.ImageSource = bmpSrc;
                }
                else if (textureInfo.D3DTextureFormat == TextureFormats.DXT3)
                {
                    // decode dxt3
                    byte[] decompressedImage = Squish.DecompressImage(
                        textureInfo.Data.ToArray(),
                        (int)textureInfo.Width,
                        (int)textureInfo.Height,
                        SquishFlags.Dxt3
                    );
                }
                else
                {
                    if(textureInfo.BitDepth == 8)
                    {
                        PixelFormat pixelFormat = PixelFormats.Indexed8;
                        int width = (int)textureInfo.Width;
                        int height = (int)textureInfo.Height;
                        int rawStride = (width * pixelFormat.BitsPerPixel + 7) / 8;

                        if (rawStride * height != textureInfo.DataSize)
                        {
                            //Debug.WriteLine("Data Length Error. {0} != {1}",
                            //    rawStride * height,
                            //    textureInfo.DataSize);
                            return;
                        }

                        List<Color> paletteColors = new List<Color>();
                        for(int j = 0; j < textureInfo.Palette.Count; j += 4)
                        {
                            Color col = new Color
                            {
                                B = textureInfo.Palette[j],
                                G = textureInfo.Palette[j + 1],
                                R = textureInfo.Palette[j + 2],
                                A = textureInfo.Palette[j + 3]
                            };
                            paletteColors.Add(col);
                        }
                        var palette = new BitmapPalette(paletteColors);

                        BitmapSource bmpSrc = BitmapSource.Create(
                            width,
                            height,
                            96,
                            96,
                            pixelFormat,
                            palette,
                            textureInfo.Data.ToArray(),
                            rawStride
                        );

                        brush.ImageSource = bmpSrc;
                    }
                    else if(textureInfo.BitDepth == 16)
                    {
                        PixelFormat pixelFormat = PixelFormats.Bgr565;
                        int width = (int)textureInfo.Width;
                        int height = (int)textureInfo.Height;
                        int rawStride = (width * pixelFormat.BitsPerPixel + 7) / 8;

                        if (rawStride * height != textureInfo.DataSize)
                        {
                            //Debug.WriteLine("Data Length Error. {0} != {1}",
                            //    rawStride * height,
                            //    textureInfo.DataSize);
                            return;
                        }

                        BitmapSource bmpSrc = BitmapSource.Create(
                            width,
                            height,
                            96,
                            96,
                            pixelFormat,
                            null,
                            textureInfo.Data.ToArray(), 
                            rawStride
                        );

                        brush.ImageSource = bmpSrc;
                    }
                    else if(textureInfo.BitDepth == 32)
                    {
                        PixelFormat pixelFormat = PixelFormats.Bgra32;
                        int width = (int)textureInfo.Width;
                        int height = (int)textureInfo.Height;
                        int rawStride = (width * pixelFormat.BitsPerPixel + 7) / 8;
                        
                        if(rawStride * height != textureInfo.DataSize)
                        {
                            //Debug.WriteLine("Data Length Error. {0} != {1}",
                            //    rawStride * height, 
                            //    textureInfo.DataSize);
                            return;
                        }

                        BitmapSource bmpSrc = BitmapSource.Create(
                            width, 
                            height, 
                            96, 
                            96, 
                            pixelFormat, 
                            null,
                            textureInfo.Data.ToArray(), 
                            rawStride
                        );

                        brush.ImageSource = bmpSrc;
                    }
                    else
                    {
                        brush = null;
                    }
                }
                if(brush != null)
                    MaterialDictionary.Add(textureInfo.TextureName, new DiffuseMaterial(brush));
                else
                    MaterialDictionary.Add(textureInfo.TextureName, new DiffuseMaterial(Brushes.White));
            }
        }
    }
}
