using Sasinosoft.SampMapEditor.IMG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sasinosoft.SampMapEditor.RenderWare.Txd
{
    public class TxdParser
    {
        private byte[] parserData;
        private int parserOffset;

        /// <summary>
        /// Creates a new instance of the TXD Parser.
        /// </summary>
        public TxdParser() { }

        /// <summary>
        /// Parses a RenderWare TXD texture from a stream given its start offset and its length.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="offset">The offset from which it must start reading the stream.</param>
        /// <param name="length">The length in bytes of the section of the stream to read.</param>
        /// <returns>A new RenderWareTexture object containing the TXD data read from the stream.</returns>
        public RenderWareTexture Parse(Stream stream, int offset, int length)
        {
            byte[] data = new byte[length];
            stream.Read(data, offset, length);
            return Parse(data);
        }

        /// <summary>
        /// Parses a RenderWare TXD texture from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>A new RenderWareTexture object containing the TXD data read from the stream.</returns>
        public RenderWareTexture Parse(Stream stream)
        {
            int offset = 0;
            long size = stream.Length;
            return Parse(stream, offset, (int)size);
        }

        /// <summary>
        /// Parses a RenderWare TXD texture from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <returns>A new RenderWareTexture object containing the TXD data read from the file stream.</returns>
        public RenderWareTexture Parse(string fileName)
        {
            return Parse(new FileStream(fileName, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// Parses a RenderWare TXD texture from an IMG archive, given the index of the file.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the TXD file.</param>
        /// <param name="index">The index of the file inside the archive.</param>
        /// <returns>A new RenderWareTexture object containing the TXD data read from the archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function.
        /// </remarks>
        /// <example>
        /// var fileIdx = 1244;
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     RenderWareTexture texture = TxdParser.Parse(imgArchive, fileIdx);
        /// }
        /// </example>
        public RenderWareTexture Parse(IMGArchive imgArchive, int index)
        {
            IMGArchiveFile? archiveFile = imgArchive.GetFileById(index);
            if (archiveFile.HasValue)
            {
                if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".txd")
                    return Parse(archiveFile.Value.FileByteBuffer);
            }
            return null;
        }

        /// <summary>
        /// Parses a RenderWare TXD texture from an IMG archive, given the name of the file.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the TXD file.</param>
        /// <param name="fileName">The name of the file inside the archive.</param>
        /// <returns>A new RenderWareTexture object containing the TXD data read from the archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function.
        /// </remarks>
        /// <example>
        /// var fileName = "ryder.txd";
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     RenderWareTexture texture = TxdParser.Parse(imgArchive, fileName);
        /// }
        /// </example>
        public RenderWareTexture Parse(IMGArchive imgArchive, string fileName)
        {
            IMGArchiveFile? archiveFile = imgArchive.GetFileByName(fileName);
            if (archiveFile.HasValue)
            {
                if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".txd")
                    return Parse(archiveFile.Value.FileByteBuffer);
            }
            return null;
        }

        /// <summary>
        /// Parses all RenderWare TXD textures from an IMG archive.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the TXD files.</param>
        /// <returns>A List of all the RenderWareTextures read from the IMG archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function. Only the files having
        /// an extension of '.txd' will be considered TXD textures, and thus parsed and added to the returned List.
        /// </remarks>
        /// <example>
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     List&lt;RenderWareTexture&gt; textures = TxdParser.ParseAll(imgArchive);
        /// }
        /// </example>
        public List<RenderWareTexture> ParseAll(IMGArchive imgArchive)
        {
            var list = new List<RenderWareTexture>();
            for (int i = 0; i < imgArchive.DirEntries.Count; i++)
            {
                IMGArchiveFile? archiveFile = imgArchive.GetFileById(i, false);
                if (archiveFile.HasValue)
                {
                    if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".txd")
                    {
                        archiveFile = imgArchive.GetFileById(i, true);
                        Debug.WriteLine(archiveFile.Value.FileEntry.FileName);
                        list.Add(Parse(archiveFile.Value.FileByteBuffer));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// Parses a RenderWare TXD texture from a byte array.
        /// </summary>
        /// <param name="data">The TXD data to read in binary form.</param>
        /// <returns>A new RenderWareTexture object containing the parsed TXD data.</returns>
        public RenderWareTexture Parse(byte[] data)
        {
            parserData = data;
            parserOffset = 0;
            ExtendedSection root = NextSection(null);
            return new RenderWareTexture(root);
        }

        private byte[] SubByteArray(int length)
        {
            var bytes = new byte[length];
            for (int i = parserOffset; i < (parserOffset + length); i++)
            {
                bytes[i - parserOffset] = parserData[i];
            }
            parserOffset += length;
            return bytes;
        }

        private UInt32 NextUInt32()
        {
            return BitConverter.ToUInt32(SubByteArray(sizeof(UInt32)), 0);
        }

        private UInt16 NextUInt16()
        {
            return BitConverter.ToUInt16(SubByteArray(sizeof(UInt16)), 0);
        }

        private byte NextByte() // = UInt8
        {
            return SubByteArray(sizeof(byte))[0];
        }

        private float NextFloat()
        {
            return BitConverter.ToSingle(SubByteArray(sizeof(float)), 0);
        }

        private string NextString(int length)
        {
            if (length <= 0)
                return string.Empty;
            byte[] chars = SubByteArray(length);
            string str = Encoding.ASCII.GetString(chars);
            if (str.IndexOf('\0') != -1)
                str = str.Remove(str.IndexOf('\0'));
            return str;
        }

        private ExtendedSection NextSection(ExtendedSection parent)
        {
            // 1. Check whether it's possible to create 
            //    other sections inside the parent Section
            if (parent != null)
            {
                var bytesReadSinceParentCreation = (parserOffset - parent.CreationOffset);
                if (bytesReadSinceParentCreation == parent.Size)
                {
                    // go up in the tree
                    if (parent.Parent != null)
                        return NextSection(parent.Parent);
                    //else
                    //    Debug.Write(parent.GetTreeString());
                    return parent;
                }
                else if (bytesReadSinceParentCreation > parent.Size)
                {
                    // file structure error
                    throw new TxdParsingException();
                }
            }

            // 2. Read the header information
            SectionType sectionType = (SectionType)NextUInt32();
            UInt32 sectionSize = NextUInt32();
            UInt32 sectionVersion = NextUInt32();
            int startOffset = parserOffset;

            // 3. Determine the type of section
            switch (sectionType)
            {
                // 3.1 In case of a complex section, we create a new
                // ExtendedSection object, and we call this function
                // recursively with 'parent' set to the new instance
                case SectionType.RwExtension:
                case SectionType.RwTextureNative:
                case SectionType.RwTextureDictionary:
                    var extendedSection = new ExtendedSection()
                    {
                        Type = sectionType,
                        Size = sectionSize,
                        Version = sectionVersion,
                        CreationOffset = parserOffset
                    };
                    if (parent != null)
                        parent.AddChild(extendedSection);

                    return NextSection(extendedSection);

                // 3.2 In case of a data section, we need to know the 
                // parent Section type in order to instantiate the 
                // appropriate class and fill it with data in a
                // meaningful way
                case SectionType.RwData:
                    switch (parent.Type) // parent cannot be null
                    {
                        case SectionType.RwTextureNative:
                            var textureNativeDataSection = new TextureNativeDataSection()
                            {
                                Type = sectionType,
                                Size = sectionSize,
                                Version = sectionVersion,
                                //
                                TextureVersion = NextUInt32(),
                                FilterFlags = NextUInt32(),
                                TextureName = NextString(32),
                                AlphaName = NextString(32),
                                AlphaFlags = NextUInt32(),
                                D3DTextureFormat = (TextureFormats)NextUInt32(),
                                Width = NextUInt16(),
                                Height = NextUInt16(),
                                BitDepth = NextByte(),
                                MipMapCount = NextByte(),
                                TexCodeType = NextByte(),
                                Compression = NextByte()
                                // TODO: to be implemented
                            };

                            if(textureNativeDataSection.BitDepth == 8)
                            {
                                for (int i = 0; i < (256 * 4); i++)
                                {
                                    textureNativeDataSection.Palette.Add(NextByte());
                                }
                            }

                            textureNativeDataSection.DataSize = NextUInt32();
                            for(int i = 0; i < textureNativeDataSection.DataSize; i++)
                            {
                                textureNativeDataSection.Data.Add(NextByte());
                            }

                            if(textureNativeDataSection.MipMapCount > 1)
                            {
                                for(int i = 0; i < (textureNativeDataSection.MipMapCount - 1); i++)
                                {
                                    TextureNativeDataSection.MipMap mipMap 
                                        = new TextureNativeDataSection.MipMap()
                                    {
                                        MipMapDataSize = NextUInt32(),
                                        MipMapData = new List<byte>()
                                    };
                                    for(int j = 0; j < mipMap.MipMapDataSize; j++)
                                    {
                                        mipMap.MipMapData.Add(NextByte());
                                    }
                                    textureNativeDataSection.MipMaps.Add(mipMap);
                                }
                            }

                            if (parserOffset != (startOffset + (int)textureNativeDataSection.Size))
                            {
                                parserOffset = (startOffset + (int)textureNativeDataSection.Size);
                                textureNativeDataSection.IsDamaged = true;
                            }
                            parent.AddChild(textureNativeDataSection);
                            return NextSection(parent);

                        case SectionType.RwTextureDictionary:
                            var textureDictionaryDataSection = new TextureDictionaryDataSection()
                            {
                                Type = sectionType,
                                Size = sectionSize,
                                Version = sectionVersion,
                                //
                                TextureCount = NextUInt16(),
                                Unknown1 = NextUInt16()
                            };

                            if (parserOffset != (startOffset + (int)textureDictionaryDataSection.Size))
                            {
                                parserOffset = (startOffset + (int)textureDictionaryDataSection.Size);
                                textureDictionaryDataSection.IsDamaged = true;
                            }
                            parent.AddChild(textureDictionaryDataSection);
                            return NextSection(parent);

                        default:
                            // Unreadable: skip the number of bytes defined in the section header
                            NextString((int)sectionSize);
                            return NextSection(parent);
                    }
                // In all other cases we simply skip the number of bytes
                // declared in the section header, without creating a Section
                // object, nor adding it to the tree
                default:
                    NextString((int)sectionSize);
                    return NextSection(parent);
            }
        }
    }
}
