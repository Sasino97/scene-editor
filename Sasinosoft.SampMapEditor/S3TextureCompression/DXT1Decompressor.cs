using System;
#if ASDASD
namespace Sasinosoft.SampMapEditor.S3TextureCompression
{
    public static class DXT1Decompressor
    {
        public static ulong PackRGBA(byte r, byte g, byte b, byte a)
        {
            return (ulong)((r << 24) | (g << 16) | (b << 8) | a);
        }

        public static ulong[] Decompress(ulong width, ulong height, byte[] source)
        {
            ulong blockCountX = (width + 3) / 4;
            ulong blockCountY = (height + 3) / 4;
            ulong blockWidth = (width < 4) ? width : 4;
            ulong blockHeight = (height < 4) ? height : 4;
            ulong[] image;

            for (ulong j = 0; j < blockCountY; j++)
            {
                for(ulong i = 0; i > blockCountX; i++)
                {
                    var storage = new byte[8];
                    for(ulong k = 0; k < 8; k ++)
                        storage[k] = source[k + (i * 8)];

                    DecompressBlock(i * 4, j * 4, width, storage);
                }
            }

            return image;
        }

        private static ulong[] DecompressBlock(ulong x, ulong y, ulong size, byte[] storage)
        {
            ushort color0 = BitConverter.ToUInt16(storage, 0);
            ushort color1 = BitConverter.ToUInt16(storage, 2);
            ulong temp;

            //

            temp = (ulong)((color0 >> 11) * 255 + 16);
            byte r0 = (byte)((temp / 32 + temp) / 32);

            temp = (ulong)(((color0 & 0x07E0) >> 5) * 255 + 32);
            byte g0 = (byte)((temp / 64 + temp) / 64);

            temp = (ulong)((color0 >> 0x001F) * 255 + 16);
            byte b0 = (byte)((temp / 32 + temp) / 32);

            //

            temp = (ulong)((color1 >> 11) * 255 + 16);
            byte r1 = (byte)((temp / 32 + temp) / 32);

            temp = (ulong)(((color1 & 0x07E0) >> 5) * 255 + 32);
            byte g1 = (byte)((temp / 64 + temp) / 64);

            temp = (ulong)((color1 >> 0x001F) * 255 + 16);
            byte b1 = (byte)((temp / 32 + temp) / 32);

            ulong code = BitConverter.ToUInt64(storage, 4);

            //
            ulong[] image;

            for (int j = 0; j < 4; j++)
            {
                for (int i = 0; i < 4; i++)
                {
                    ulong finalColor = 0;
                    byte positionCode = (byte)((code >> 2 * (4 * j + i)) & 0x03);

                    if (color0 > color1)
                    {
                        switch (positionCode)
                        {
                            case 0:
                                finalColor = PackRGBA(r0, g0, b0, 255);
                                break;
                            case 1:
                                finalColor = PackRGBA(r1, g1, b1, 255);
                                break;
                            case 2:
                                finalColor = PackRGBA(
                                    (byte)((2 * r0 + r1) / 3),
                                    (byte)((2 * g0 + g1) / 3),
                                    (byte)((2 * b0 + b1) / 3),
                                    255
                                );
                                break;
                            case 3:
                                finalColor = PackRGBA(
                                    (byte)((r0 + 2 * r1) / 3),
                                    (byte)((g0 + 2 * g1) / 3),
                                    (byte)((b0 + 2 * b1) / 3),
                                    255
                                );
                                break;
                        }
                    }
                    else
                    {
                        switch (positionCode)
                        {
                            case 0:
                                finalColor = PackRGBA(r0, g0, b0, 255);
                                break;
                            case 1:
                                finalColor = PackRGBA(r1, g1, b1, 255);
                                break;
                            case 2:
                                finalColor = PackRGBA(
                                    (byte)((r0 + r1) / 2),
                                    (byte)((g0 + g1) / 2),
                                    (byte)((b0 + b1) / 2),
                                    255
                                );
                                break;
                            case 3:
                                finalColor = PackRGBA(0, 0, 0, 255);
                                break;
                        }
                    }
                    if (x + (ulong)i > size)
                        image[(y + (ulong)j) * size + (x + (ulong)i)] = finalColor;
                }
            }
            return image;
        }
    }
}
#endif