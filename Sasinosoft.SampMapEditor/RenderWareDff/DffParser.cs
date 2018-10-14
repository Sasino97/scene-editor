using Sasinosoft.SampMapEditor.IMG;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sasinosoft.SampMapEditor.RenderWareDff
{
    public static class DffParser
    {
        /// <summary>
        /// Parses a RenderWare DFF model from a stream given its start offset and its length.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="offset">The offset from which it must start reading the stream.</param>
        /// <param name="length">The length in bytes of the section of the stream to read.</param>
        /// <returns>A new DffModel object containing the DFF data read from the stream.</returns>
        public static DffModel Parse(Stream stream, int offset, int length)
        {
            byte[] data = new byte[length];
            stream.Read(data, offset, length);
            return Parse(data);
        }

        /// <summary>
        /// Parses a RenderWare DFF model from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>A new DffModel object containing the DFF data read from the stream.</returns>
        public static DffModel Parse(Stream stream)
        {
            int offset = 0;
            long size = stream.Length;
            return Parse(stream, offset, (int)size);
        }

        /// <summary>
        /// Parses a RenderWare DFF model from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <returns>A new DffModel object containing the DFF data read from the file stream.</returns>
        public static DffModel Parse(string fileName)
        {
            return Parse(new FileStream(fileName, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// Parses a RenderWare DFF model from an IMG archive, given the index of the file.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the DFF file.</param>
        /// <param name="index">The index of the file inside the archive.</param>
        /// <returns>A new DffModel object containing the DFF data read from the archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function.
        /// </remarks>
        /// <example>
        /// var fileIdx = 2415;
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     DffModel model = DffParser.Parse(imgArchive, fileIdx);
        /// }
        /// </example>
        public static DffModel Parse(IMGArchive imgArchive, int index)
        {
            IMGArchiveFile? archiveFile = imgArchive.GetFileById(index);
            if(archiveFile.HasValue)
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
        /// <returns>A new DffModel object containing the DFF data read from the archive.</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function.
        /// </remarks>
        /// <example>
        /// var fileName = "shamal.dff";
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     DffModel model = DffParser.Parse(imgArchive, fileName);
        /// }
        /// </example>
        public static DffModel Parse(IMGArchive imgArchive, string fileName)
        {
            IMGArchiveFile? archiveFile = imgArchive.GetFileByName(fileName);
            if (archiveFile.HasValue)
            {
                if(Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".dff")
                    return Parse(archiveFile.Value.FileByteBuffer);
            }
            return null;
        }

        /// <summary>
        /// Parses all RenderWare DFF models from an IMG archive.
        /// </summary>
        /// <param name="imgArchive">The object representing the IMG archive containing the DFF files.</param>
        /// <returns>A List of DffModels read from the IKG</returns>
        /// <remarks>
        /// The IMGArchive must not be already disposed when calling this function. Only the files having
        /// an extension of '.dff' will be considered DFF models, and thus parsed and added to the returned List.
        /// </remarks>
        /// <example>
        /// using (var imgArchive = new IMGArchive("gta3.img"))
        /// {
        ///     List&lt;DffModel&gt; models = DffParser.ParseAll(imgArchive);
        /// }
        /// </example>
        public static List<DffModel> ParseAll(IMGArchive imgArchive)
        {
            var list = new List<DffModel>();
            for (int i = 0; i < imgArchive.DirEntries.Count; i++)
            {
                IMGArchiveFile? archiveFile = imgArchive.GetFileById(i);
                if (archiveFile.HasValue)
                {
                    if (Path.GetExtension(archiveFile.Value.FileEntry.FileName) == ".dff")
                        list.Add(Parse(archiveFile.Value.FileByteBuffer));
                }
            }
            return list;
        }

        // the actual parsing procedural code:

        /// <summary>
        /// Parses a RenderWare DFF model from a byte array.
        /// </summary>
        /// <param name="data">The DFF data to read in binary form.</param>
        /// <returns>A new DffModel object containing the parsed DFF data.</returns>
        public static DffModel Parse(byte[] data)
        {
            // using this helper class greatly reduces the boilerplate 
            // code that would be needed otherwise
            Helper h = new Helper(data);

            // start of the parsing procedure
            var clump = new ComplexSection()
            {
                Type = h.NextSectionType(),
                Size = h.NextUInt32(),
                Unknown = h.NextUInt16(),
                Version = h.NextUInt16()
            };

            if (clump.Type != SectionType.RwClump)
                throw new DffParsingException("The first section must be a clump section.");

            var clumpData = new ClumpDataSection()
            {
                Type = h.NextSectionType(),
                Size = h.NextUInt32(),
                Unknown = h.NextUInt16(),
                Version = h.NextUInt16(),
                ObjectCount = h.NextUInt32(),
                Unknown1 = h.NextUInt32(),
                Unknown2 = h.NextUInt32()
            };
            clump.Children.Add(clumpData);

            if (clumpData.Type != SectionType.RwData)
                throw new DffParsingException("The clump section is missing its data.");

            ParseFrameLists(h, clump);
            ParseGeometryLists(h, clump);
            ParseAtomics(h, clump);
            ParseLights(h, clump);
            return null;
        }

        private static void ParseFrameLists(Helper h, ComplexSection clump)
        {
            while(h.NextSectionType() == SectionType.RwFrameList)
            {
                var frameList = new ComplexSection()
                {
                    Type = SectionType.RwFrameList,
                    Size = h.NextUInt32(),
                    Unknown = h.NextUInt16(),
                    Version = h.NextUInt16()
                };
                clump.Children.Add(frameList);

                var frameListData = new FrameListDataSection()
                {
                    Type = h.NextSectionType(),
                    Size = h.NextUInt32(),
                    Unknown = h.NextUInt16(),
                    Version = h.NextUInt16(),
                    FrameCount = h.NextUInt32()
                };

                for(int i = 0; i < frameListData.FrameCount; i ++)
                {
                    frameListData.FrameInformation.Add(new FrameListDataSection.FrameStruct()
                    {
                        RotationalMatrix = new float[,]
                        {
                            { h.NextFloat(), h.NextFloat(), h.NextFloat() },
                            { h.NextFloat(), h.NextFloat(), h.NextFloat() },
                            { h.NextFloat(), h.NextFloat(), h.NextFloat() }
                        },
                        CoordinatesOffset = new float[]
                        {
                            h.NextFloat(),
                            h.NextFloat(),
                            h.NextFloat()
                        },
                        Parent = h.NextUInt32(),
                        Unknown1 = h.NextUInt32()
                    });
                }
                frameList.Children.Add(frameListData);

                while (h.NextSectionType() == SectionType.RwExtension)
                {
                    var extension = new ComplexSection()
                    {
                        Type = SectionType.RwExtension,
                        Size = h.NextUInt32(),
                        Unknown = h.NextUInt16(),
                        Version = h.NextUInt16()
                    };
                    frameList.Children.Add(extension);

                    int o1 = h.Offset; 
                    if (h.NextSectionType() == SectionType.RwAnimPlugin)
                    {
                        var animData = new AnimPluginDataSection()
                        {
                            Type = SectionType.RwAnimPlugin,
                            Size = h.NextUInt32(),
                            Unknown = h.NextUInt16(),
                            Version = h.NextUInt16(),
                            Unknown1 = h.NextUInt32(),
                            BoneId = h.NextUInt32(),
                            BoneCount = h.NextUInt32(),
                            Unknown2 = h.NextUInt32(),
                            Unknown3 = h.NextUInt32()
                        };
                        for (int i = 0; i < animData.BoneCount; i++)
                        {
                            animData.Bones.Add(new AnimPluginDataSection.BoneInformation()
                            {
                                Id = h.NextUInt32(),
                                Index = h.NextUInt32(),
                                Type = h.NextUInt32()
                            });
                        }
                        extension.Children.Add(animData);
                    }
                    else
                        h.Offset -= sizeof(UInt32);

                    var frameData = new FrameDataSection()
                    {
                        Type = h.NextSectionType(),
                        Size = h.NextUInt32(),
                        Unknown = h.NextUInt16(),
                        Version = h.NextUInt16()
                    };
                    int o2 = h.Offset;
                    frameData.FrameName = h.NextString((int)extension.Size - (o2 - o1));
                    extension.Children.Add(frameData);
                }
                h.Offset -= sizeof(UInt32);
            }
            h.Offset -= sizeof(UInt32);
        }

        private static void ParseGeometryLists(Helper h, ComplexSection clump)
        {

        }

        private static void ParseAtomics(Helper h, ComplexSection clump)
        {

        }

        private static void ParseLights(Helper h, ComplexSection clump)
        {

        }

        private class Helper
        {
            public byte[] Data { get; set; }
            public int Offset { get; set; } 

            public Helper(byte[] data)
            {
                Data = data;
                Offset = 0;
            }

            public byte[] SubByteArray(int length)
            {
                var bytes = new byte[length];
                for (int i = Offset; i < (Offset + length); i++)
                {
                    bytes[i - Offset] = Data[i];
                }
                Offset += length;

                if (BitConverter.IsLittleEndian)
                    Array.Reverse(bytes);

                return bytes;
            }

            public UInt32 NextUInt32()
            {
                return BitConverter.ToUInt32(SubByteArray(sizeof(UInt32)), 0);
            }

            public UInt16 NextUInt16()
            {
                return BitConverter.ToUInt16(SubByteArray(sizeof(UInt16)), 0);
            }

            public float NextFloat()
            {
                return BitConverter.ToSingle(SubByteArray(sizeof(float)), 0);
            }

            public string NextString(int length)
            {
                byte[] chars = SubByteArray(length);
                string str = Encoding.ASCII.GetString(chars);
                return str.Remove(str.IndexOf('\0'));
            }
            
            public SectionType NextSectionType()
            {
                return (SectionType)NextUInt32();
            }
        }
    }
}
