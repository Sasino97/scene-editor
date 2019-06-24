/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.IO;
using System.Text;

using static Sasinosoft.SampMapEditor.Vehicles.VehicleUtils;
using static Sasinosoft.SampMapEditor.Pedestrians.PedestrianUtils;
using Sasinosoft.SampMapEditor.Utils;
using Sasinosoft.SampMapEditor.Data;
using System.Windows.Media.Media3D;

namespace Sasinosoft.SampMapEditor.IPL
{
    public class IPLParser
    {
        /// <summary>
        /// Creates a new instance of the IPL Parser.
        /// </summary>
        public IPLParser() { }

        /// <summary>
        /// Parses an IPL file from a stream given its start offset and its length.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="offset">The offset from which it must start reading the stream.</param>
        /// <param name="length">The length in bytes of the section of the stream to read.</param>
        /// <param name="errorCount">The number of errors.</param>
        public void Parse(Stream stream, int offset, int length, out int errorCount)
        {
            byte[] data = new byte[length];
            stream.Read(data, offset, length);
            Parse(data, out errorCount);
        }

        /// <summary>
        /// Parses an IPL file from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <param name="errorCount">The number of errors.</param>
        public void Parse(Stream stream, out int errorCount)
        {
            int offset = 0;
            long size = stream.Length;
            Parse(stream, offset, (int)size, out errorCount);
        }

        /// <summary>
        /// Parses an IPL file from a file given its name.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <param name="errorCount">The number of errors.</param>
        public void Parse(string fileName, out int errorCount)
        {
            Parse(new FileStream(fileName, FileMode.Open, FileAccess.Read), out errorCount);
        }

        /// <summary>
        /// Parses an IPL file from a byte array.
        /// </summary>
        /// <param name="data">The IPL data to read in an array of bytes.</param>
        /// <param name="errorCount">The number of errors.</param>
        public void Parse(byte[] data, out int errorCount)
        {
            // detect whether it's a text IPL or a binary IPL
            if (data[0] == 'b' && data[0] == 'n' && data[0] == 'r' && data[0] == 'y')
            {
                // parse bin IPL
                ParseBinaryIPL(data, out errorCount);
            }
            else
            {
                // parse text IPL
                ParseTextIPL(data, out errorCount);
            }
        }

        private void ParseBinaryIPL(byte[] data, out int errorCount)
        {
            // TODO
            errorCount = 0;
        }

        private void ParseTextIPL(byte[] data, out int errorCount)
        {
            string parserData = Encoding.UTF8.GetString(data);
            string[] lines = parserData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string mode = null;
            errorCount = 0;

            foreach (string line in lines)
            {
                if (line.StartsWith("#", StringComparison.InvariantCultureIgnoreCase) ||
                    line.Trim().Length == 0)
                    continue;

                if (mode == null)
                {
                    if (line.StartsWith("inst", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("cars", StringComparison.InvariantCultureIgnoreCase) ||
                        // unused
                        line.StartsWith("zone", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("cull", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("pick", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("occl", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("mult", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("grge", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("enex", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("jump", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("tcyc", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("auzo", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mode = line.Substring(0, 4).ToLowerInvariant();
                    }
                    continue;
                }
                else
                {
                    if (line.StartsWith("end", StringComparison.InvariantCultureIgnoreCase))
                    {
                        mode = null;
                        continue;
                    }

                    var parts = line.Split(new char[] { ',', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < parts.Length; i++)
                    {
                        parts[i] = parts[i].Trim(' ', '\t', '\r', ',');
                    }

                    try
                    {
                        if (mode == "objs")
                        {
                            var element = new InstanceIPLElement();
                            if (parts.Length >= 10)
                            {
                                element.Id = int.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.Interior = int.Parse(parts[2]);
                                element.X = float.Parse(parts[3]);
                                element.Y = float.Parse(parts[4]);
                                element.Z = float.Parse(parts[5]);
                                element.RX = float.Parse(parts[6]);
                                element.RY = float.Parse(parts[7]);
                                element.RZ = float.Parse(parts[8]);
                                element.RW = float.Parse(parts[9]);
                                element.LODIndex = int.Parse(parts[9]);
                            }

                            var obj = new ObjectPlacementDefinition()
                            {
                                Id = element.Id,
                                Position = new Point3D(element.X, element.Y, element.Z),
                                Rotation = new Quaternion(element.RX, element.RY, element.RZ, element.RW),
                                Interior = element.Interior,
                                LODIndex = element.LODIndex
                            };
                            MasterDictionary.ObjectPlacementDefinitions.Add(element.Id, obj);
                        }
                        else if (mode == "cars")
                        {
                            var element = new CarsIPLElement();
                            if (parts.Length >= 8)
                            {
                                element.X = float.Parse(parts[0]);
                                element.Y = float.Parse(parts[1]);
                                element.Z = float.Parse(parts[2]);
                                element.Angle = float.Parse(parts[3]);
                                element.Id = int.Parse(parts[4]);
                                element.Color1 = int.Parse(parts[5]);
                                element.Color2 = int.Parse(parts[6]);
                                element.ForceSpawn = int.Parse(parts[7]);
                            }
                            if (parts.Length >= 10)
                            {
                                element.Alarm = int.Parse(parts[8]);
                                element.DoorLock = int.Parse(parts[9]);
                            }
                            if (parts.Length >= 12)
                            {
                                element.Unknown1 = int.Parse(parts[10]);
                                element.Unknown2 = int.Parse(parts[11]);
                            }

                            var obj = new VehiclePlacementDefinition()
                            {
                                Id = element.Id,
                                Position = new Point3D(element.X, element.Y, element.Z),
                                ZRotation = element.Angle,
                                Color1 = element.Color1,
                                Color2 = element.Color2
                            };
                            MasterDictionary.VehiclePlacementDefinitions.Add(element.Id, obj);
                        }
                    }
                    catch
                    {
                        errorCount++;
                    }
                }
            }
        }
    }
}
