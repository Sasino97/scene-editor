/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using Sasinosoft.SampMapEditor.IMG;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    public class DffParser
    {
        private byte[] parserData;
        private int parserOffset;

        /// <summary>
        /// Creates a new instance of the DFF parser.
        /// </summary>
        public DffParser() { }

        /// <summary>
        /// Parses a RenderWare DFF model from a stream given its start offset and its length.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="offset">The offset from which it must start reading the stream.</param>
        /// <param name="length">The length in bytes of the section of the stream to read.</param>
        /// <returns>A new ExtendedSection object containing the DFF data read from the stream.</returns>
        public ExtendedSection Parse(Stream stream, int offset, int length)
        {
            byte[] data = new byte[length];
            stream.Read(data, offset, length);
            return Parse(data);
        }

        /// <summary>
        /// Parses a RenderWare DFF model from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>A new ExtendedSection object containing the DFF data read from the stream.</returns>
        public ExtendedSection Parse(Stream stream)
        {
            int offset = 0;
            long size = stream.Length;
            return Parse(stream, offset, (int)size);
        }

        /// <summary>
        /// Parses a RenderWare DFF model from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <returns>A new ExtendedSection object containing the DFF data read from the file stream.</returns>
        public ExtendedSection Parse(string fileName)
        {
            return Parse(new FileStream(fileName, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// Parses a RenderWare DFF model from an IMG archive, given the index of the file.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the DFF file.</param>
        /// <param name="index">The index of the file inside the archive.</param>
        /// <returns>A new ExtendedSection object containing the DFF data read from the archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function.
        /// </remarks>
        /// <example>
        /// var fileIdx = 2415;
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     RenderWareModel model = new RenderWareModel(DffParser.Parse(imgArchive, fileIdx));
        /// }
        /// </example>
        public ExtendedSection Parse(IMGArchive imgArchive, int index)
        {
            IMGArchiveFile? archiveFile = imgArchive.GetFileById(index);
            if (archiveFile.HasValue)
            {
                if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".dff")
                    return Parse(archiveFile.Value.FileByteBuffer);
            }
            return null;
        }

        /// <summary>
        /// Parses a RenderWare DFF model from an IMG archive, given the name of the file.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the DFF file.</param>
        /// <param name="fileName">The name of the file inside the archive.</param>
        /// <returns>A new ExtendedSection object containing the DFF data read from the archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function.
        /// </remarks>
        /// <example>
        /// var fileName = "shamal.dff";
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     RenderWareModel model = new RenderWareModel(DffParser.Parse(imgArchive, fileIdx));
        /// }
        /// </example>
        public ExtendedSection Parse(IMGArchive imgArchive, string fileName)
        {
            IMGArchiveFile? archiveFile = imgArchive.GetFileByName(fileName);
            if (archiveFile.HasValue)
            {
                if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".dff")
                    return Parse(archiveFile.Value.FileByteBuffer);
            }
            return null;
        }

        /// <summary>
        /// Parses all RenderWare DFF models from an IMG archive.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the DFF files.</param>
        /// <returns>A List of ExtendedSection containing all the DFF data read from the IMG archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function. Only the files having
        /// an extension of '.dff' will be considered DFF models, and thus parsed and added to the returned List.
        /// </remarks>
        /// <example>
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     List&lt;ExtendedSection&gt; data = DffParser.ParseAll(imgArchive);
        /// }
        /// </example>
        public List<ExtendedSection> ParseAll(IMGArchive imgArchive)
        {
            var list = new List<ExtendedSection>();
            for (int i = 0; i < imgArchive.DirEntries.Count; i++)
            {
                IMGArchiveFile? archiveFile = imgArchive.GetFileById(i, false);
                if (archiveFile.HasValue)
                {
                    if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".dff")
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
        /// Parses a RenderWare DFF model from a byte array.
        /// </summary>
        /// <param name="data">The DFF data to read in binary form.</param>
        /// <returns>A new ExtendedSection object containing the parsed DFF data.</returns>
        public ExtendedSection Parse(byte[] data)
        {
            parserData = data;
            parserOffset = 0;
            ExtendedSection root = CreateTree();
            return root;
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

        private ExtendedSection CreateTree()
        {
            ExtendedSection root = null;
            ExtendedSection parent = null;

            while (true)
            {
                // 1. Check whether it's possible to create 
                //    other sections inside the parent Section
                if (parent != null)
                {
                    var bytesReadSinceParentCreation = (parserOffset - parent.CreationOffset);
                    if (bytesReadSinceParentCreation == parent.Size)
                    {
                        // 1.1 Attempt to navigate the tree up, by setting
                        // the parent variable to its own Parent. If this 
                        // is not possible, then break out of the loop.
                        if (parent.Parent != null)
                        {
                            parent = parent.Parent;
                            continue;
                        }
                        //else
                        //    Debug.Write(parent.GetTreeString());
                        break;
                    }
                    else if (bytesReadSinceParentCreation > parent.Size)
                    {
                        // 1.2 File structure error. Throw an exception.
                        throw new DffParsingException();
                    }
                }

                // 2. Read the header information
                SectionType sectionType = (SectionType)NextUInt32();
                uint sectionSize = NextUInt32();
                uint sectionVersion = NextUInt32();
                int startOffset = parserOffset;

                // 3. Determine the type of section
                switch (sectionType)
                {
                    // 3.1 In case of a complex section, we create a new
                    // ExtendedSection object, and we call this function
                    // recursively with 'parent' set to the new instance
                    case SectionType.RwClump:
                    case SectionType.RwFrameList:
                    case SectionType.RwExtension:
                    case SectionType.RwGeometryList:
                    case SectionType.RwGeometry:
                    case SectionType.RwMaterialList:
                    case SectionType.RwMaterial:
                    case SectionType.RwTexture:
                    case SectionType.RwAtomic:
                    case SectionType.RwLight:
                        var extendedSection = new ExtendedSection()
                        {
                            Type = sectionType,
                            Size = sectionSize,
                            Version = sectionVersion,
                            CreationOffset = parserOffset
                        };

                        if (root == null)
                            root = extendedSection;

                        if (parent != null)
                            parent.AddChild(extendedSection);

                        parent = extendedSection;
                        continue;

                    // 3.2 In case of a data section, we need to know the 
                    // parent Section type in order to instantiate the 
                    // appropriate class and fill it with data in a
                    // meaningful way
                    case SectionType.RwData:
                        switch (parent.Type) // parent cannot be null
                        {
                            case SectionType.RwClump:
                                var clumpDataSection = new ClumpDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    ObjectCount = NextUInt32(),
                                    Unknown1 = NextUInt32(),
                                    Unknown2 = NextUInt32()
                                };

                                if (parserOffset != (startOffset + (int)clumpDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)clumpDataSection.Size);
                                    clumpDataSection.IsDamaged = true;
                                }
                                parent.AddChild(clumpDataSection);
                                continue;

                            case SectionType.RwFrameList:
                                var frameListDataSection = new FrameListDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    FrameCount = NextUInt32()
                                };

                                for (int i = 0; i < frameListDataSection.FrameCount; i++)
                                {
                                    frameListDataSection.FrameInformation.Add(new FrameListDataSection.FrameStruct()
                                    {
                                        RotationalMatrix = new float[,]
                                        {
                                        { NextFloat(), NextFloat(), NextFloat() },
                                        { NextFloat(), NextFloat(), NextFloat() },
                                        { NextFloat(), NextFloat(), NextFloat() }
                                        },
                                        CoordinatesOffset = new float[]
                                        {
                                        NextFloat(),
                                        NextFloat(),
                                        NextFloat()
                                        },
                                        Parent = NextUInt32(),
                                        Unknown1 = NextUInt32()
                                    });
                                }
                                if (parserOffset != (startOffset + (int)frameListDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)frameListDataSection.Size);
                                    frameListDataSection.IsDamaged = true;
                                }
                                parent.AddChild(frameListDataSection);
                                continue;

                            case SectionType.RwGeometryList:
                                var geometryListDataSection = new GeometryListDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    GeometryCount = NextUInt32()
                                };
                                if (parserOffset != (startOffset + (int)geometryListDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)geometryListDataSection.Size);
                                    geometryListDataSection.IsDamaged = true;
                                }
                                parent.AddChild(geometryListDataSection);
                                continue;

                            case SectionType.RwGeometry:
                                var geometryDataSection = new GeometryDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    Flags = (GeometryDataFlags)NextUInt16(),
                                    UVMapCount = NextByte(),
                                    HasNativeGeometry = NextByte(),
                                    TriangleCount = NextUInt32(),
                                    VertexCount = NextUInt32(),
                                    MorphCount = NextUInt32()
                                };

                                if (geometryDataSection.Version < 0x34000)
                                    NextString(12);

                                if (geometryDataSection.HasNativeGeometry == 0)
                                {
                                    if ((geometryDataSection.Flags & GeometryDataFlags.RwObjectVertexColor) != GeometryDataFlags.None)
                                    {
                                        for (int i = 0; i < geometryDataSection.VertexCount; i++)
                                        {
                                            geometryDataSection.VertexColors.Add(new GeometryDataSection.VertexColor()
                                            {
                                                R = NextByte(),
                                                G = NextByte(),
                                                B = NextByte(),
                                                A = NextByte(),
                                            });
                                        }
                                    }

                                    if ((geometryDataSection.Flags & GeometryDataFlags.RwObjectVertexUv) != GeometryDataFlags.None)
                                    {
                                        for (int i = 0; i < geometryDataSection.VertexCount; i++)
                                        {
                                            geometryDataSection.VertexUVs.Add(new GeometryDataSection.VertexUV()
                                            {
                                                U = NextFloat(),
                                                V = NextFloat()
                                            });
                                        }
                                        geometryDataSection.UVMapCount = 1;
                                    }

                                    if ((geometryDataSection.Flags & GeometryDataFlags.RwObjectVertexTextured) != GeometryDataFlags.None)
                                    {
                                        for (int i = 0; i < geometryDataSection.UVMapCount; i++)
                                        {
                                            geometryDataSection.VertexUVs.Add(new GeometryDataSection.VertexUV()
                                            {
                                                U = NextFloat(),
                                                V = NextFloat()
                                            });
                                        }
                                    }

                                    for (int i = 0; i < geometryDataSection.TriangleCount; i++)
                                    {
                                        geometryDataSection.Triangles.Add(new GeometryDataSection.Triangle()
                                        {
                                            Vertex2 = NextUInt16(),
                                            Vertex1 = NextUInt16(),
                                            Flags = NextUInt16(),
                                            Vertex3 = NextUInt16()
                                        });
                                    }
                                }

                                geometryDataSection.BoundingSphereX = NextFloat();
                                geometryDataSection.BoundingSphereY = NextFloat();
                                geometryDataSection.BoundingSphereZ = NextFloat();
                                geometryDataSection.BoundingSphereRadius = NextFloat();
                                geometryDataSection.HasPosition = NextUInt32();
                                geometryDataSection.HasNormals = NextUInt32();

                                if (geometryDataSection.HasNativeGeometry == 0)
                                {
                                    for (int i = 0; i < geometryDataSection.VertexCount; i++)
                                    {
                                        geometryDataSection.Vertices.Add(new GeometryDataSection.Vertex()
                                        {
                                            X = NextFloat(),
                                            Y = NextFloat(),
                                            Z = NextFloat()
                                        });
                                    }

                                    if ((geometryDataSection.Flags & GeometryDataFlags.RwObjectVertexNormal)
                                        != GeometryDataFlags.None)
                                    {
                                        for (int i = 0; i < geometryDataSection.VertexCount; i++)
                                        {
                                            geometryDataSection.Normals.Add(new GeometryDataSection.Normal()
                                            {
                                                X = NextFloat(),
                                                Y = NextFloat(),
                                                Z = NextFloat()
                                            });
                                        }
                                    }
                                }
                                if (parserOffset != (startOffset + (int)geometryDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)geometryDataSection.Size);
                                    geometryDataSection.IsDamaged = true;
                                }
                                parent.AddChild(geometryDataSection);
                                continue;

                            case SectionType.RwMaterialList:
                                var materialListDataSection = new MaterialListDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    MaterialCount = NextUInt32(),
                                    Unknown1 = NextUInt32()
                                };
                                if (parserOffset != (startOffset + (int)materialListDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)materialListDataSection.Size);
                                    materialListDataSection.IsDamaged = true;
                                }
                                parent.AddChild(materialListDataSection);
                                continue;

                            case SectionType.RwMaterial:
                                var materialDataSection = new MaterialDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    Unknown1 = NextUInt32(),
                                    Color = new MaterialDataSection.MaterialColor()
                                    {
                                        R = NextByte(),
                                        G = NextByte(),
                                        B = NextByte(),
                                        A = NextByte()
                                    },
                                    Unknown2 = NextUInt32(),
                                    TextureCount = NextUInt32(),
                                    Unknown3 = new float[]
                                    {
                                    NextFloat(),
                                    NextFloat(),
                                    NextFloat()
                                    }
                                };
                                if (parserOffset != (startOffset + (int)materialDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)materialDataSection.Size);
                                    materialDataSection.IsDamaged = true;
                                }
                                parent.AddChild(materialDataSection);
                                continue;

                            case SectionType.RwTexture:
                                var textureDataSection = new TextureDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    TextureFilterModeFlags = NextUInt16(),
                                    Unknown1 = NextUInt16()
                                };
                                if (parserOffset != (startOffset + (int)textureDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)textureDataSection.Size);
                                    textureDataSection.IsDamaged = true;
                                }
                                parent.AddChild(textureDataSection);
                                continue;

                            case SectionType.RwAtomic:
                                var atomicDataSection = new AtomicDataSection()
                                {
                                    Type = sectionType,
                                    Size = sectionSize,
                                    Version = sectionVersion,
                                    //
                                    FrameNumber = NextUInt32(),
                                    GeometryNumber = NextUInt32(),
                                    Unknown1 = NextUInt32(),
                                    Unknown2 = NextUInt32()
                                };
                                if (parserOffset != (startOffset + (int)atomicDataSection.Size))
                                {
                                    parserOffset = (startOffset + (int)atomicDataSection.Size);
                                    atomicDataSection.IsDamaged = true;
                                }
                                parent.AddChild(atomicDataSection);
                                continue;

                            default:
                                // Unreadable: skip the number of bytes defined in the section header
                                NextString((int)sectionSize);
                                continue;
                        }

                    // 3.3 In case of special data formats, we parse their content
                    //     accordingly
                    case SectionType.RwFrame:
                        var frameSection = new FrameDataSection()
                        {
                            Type = sectionType,
                            Size = sectionSize,
                            Version = sectionVersion,
                        };
                        frameSection.FrameName = NextString((int)frameSection.Size);
                        if (parserOffset != (startOffset + (int)frameSection.Size))
                        {
                            parserOffset = (startOffset + (int)frameSection.Size);
                            frameSection.IsDamaged = true;
                        }
                        parent.AddChild(frameSection);
                        continue;

                    case SectionType.RwString:
                        var stringDataSection = new StringDataSection()
                        {
                            Type = sectionType,
                            Size = sectionSize,
                            Version = sectionVersion,
                        };
                        stringDataSection.String = NextString((int)stringDataSection.Size);
                        if (parserOffset != (startOffset + (int)stringDataSection.Size))
                        {
                            parserOffset = (startOffset + (int)stringDataSection.Size);
                            stringDataSection.IsDamaged = true;
                        }
                        parent.AddChild(stringDataSection);
                        continue;

                    case SectionType.RwMaterialSplit:
                        var materialSplitDataSection = new MaterialSplitDataSection()
                        {
                            Type = sectionType,
                            Size = sectionSize,
                            Version = sectionVersion,
                            //
                            FaceType = NextUInt32(),
                            SplitCount = NextUInt32(),
                            FaceCount = NextUInt32()
                        };
                        for (int i = 0; i < materialSplitDataSection.SplitCount; i++)
                        {
                            var split = new MaterialSplitDataSection.Split()
                            {
                                IndexCount = NextUInt32(),
                                Material = NextUInt32(),
                                Indices = new List<UInt32>()
                            };
                            materialSplitDataSection.Splits.Add(split);
                            for (int j = 0; j < split.IndexCount; j++)
                            {
                                split.Indices.Add(NextUInt32());
                            }
                        }
                        if (parserOffset != (startOffset + (int)materialSplitDataSection.Size))
                        {
                            parserOffset = (startOffset + (int)materialSplitDataSection.Size);
                            materialSplitDataSection.IsDamaged = true;
                        }
                        parent.AddChild(materialSplitDataSection);
                        continue;

                    case SectionType.RwAnimPlugin:
                        var animPluginDataSection = new AnimPluginDataSection()
                        {
                            Type = sectionType,
                            Size = sectionSize,
                            Version = sectionVersion,
                            //
                            Unknown1 = NextUInt32(),
                            BoneId = NextUInt32(),
                            BoneCount = NextUInt32(),
                            Unknown2 = NextUInt32(),
                            Unknown3 = NextUInt32()
                        };
                        for (int i = 0; i < animPluginDataSection.BoneCount; i++)
                        {
                            animPluginDataSection.Bones.Add(new AnimPluginDataSection.BoneInformation()
                            {
                                Id = NextUInt32(),
                                Index = NextUInt32(),
                                Type = NextUInt32()
                            });
                        }
                        if (parserOffset != (startOffset + (int)animPluginDataSection.Size))
                        {
                            parserOffset = (startOffset + (int)animPluginDataSection.Size);
                            animPluginDataSection.IsDamaged = true;
                        }
                        parent.AddChild(animPluginDataSection);
                        continue;

                    // In all other cases we simply skip the number of bytes
                    // declared in the section header, without creating a Section
                    // object, nor adding it to the tree
                    default:
                        NextString((int)sectionSize);
                        continue;
                }
            }
            return root;
        }
    }
}
