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

namespace Sasinosoft.SampMapEditor.IDE
{
    public class IDEParser
    {
        /// <summary>
        /// Creates a new instance of the IDE Parser.
        /// </summary>
        public IDEParser() { }

        /// <summary>
        /// Parses an IDE file from a stream given its start offset and its length.
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
        /// Parses an IDE file from a stream.
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
        /// Parses an IDE file from a file given its name.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <param name="errorCount">The number of errors.</param>
        public void Parse(string fileName, out int errorCount)
        {
            Parse(new FileStream(fileName, FileMode.Open, FileAccess.Read), out errorCount);
        }

        /// <summary>
        /// Parses an IDE file from a byte array.
        /// </summary>
        /// <param name="data">The IDE data to read in an array of bytes.</param>
        /// <param name="errorCount">The number of errors.</param>
        public void Parse(byte[] data, out int errorCount)
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
                    if (line.StartsWith("objs", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("tobj", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("anim", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("tanm", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("cars", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("weap", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("peds", StringComparison.InvariantCultureIgnoreCase))
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

                    for (int i = 0; i < parts.Length; i ++)
                    {
                        parts[i] = parts[i].Trim(' ', '\t', '\r', ',');
                    }

                    try
                    {
                        if (mode == "objs")
                        {
                            var element = new ObjectsIDEElement();
                            if (parts.Length == 5)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[3]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[4]);
                            }
                            else if (parts.Length == 6)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[4]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[5]);
                            }
                            else if (parts.Length == 7)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[5]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[6]);
                            }
                            else if (parts.Length >= 8)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[6]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[7]);
                            }

                            var obj = new ObjectDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName
                            };
                            MasterDictionary.ObjectDefinitions.Add((int)element.Id, obj);
                        }
                        else if (mode == "tobj")
                        {
                            var element = new ObjectsIDEElement();
                            if (parts.Length == 7)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[3]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[4]);
                                element.TimeOn = UInt32.Parse(parts[5]);
                                element.TimeOff = UInt32.Parse(parts[6]);
                            }
                            else if (parts.Length == 8)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[3]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[5]);
                                element.TimeOn = UInt32.Parse(parts[6]);
                                element.TimeOff = UInt32.Parse(parts[7]);
                            }
                            else if (parts.Length == 9)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[3]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[6]);
                                element.TimeOn = UInt32.Parse(parts[7]);
                                element.TimeOff = UInt32.Parse(parts[8]);
                            }
                            else if (parts.Length >= 10)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.DrawDistance = float.Parse(parts[3]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[7]);
                                element.TimeOn = UInt32.Parse(parts[8]);
                                element.TimeOff = UInt32.Parse(parts[9]);
                            }

                            var obj = new ObjectDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName
                            };
                            MasterDictionary.ObjectDefinitions.Add((int)element.Id, obj);
                        }
                        else if (mode == "anim")
                        {
                            var element = new ObjectsIDEElement();
                            if (parts.Length >= 6)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.AnimationName = parts[3];
                                element.DrawDistance = float.Parse(parts[4]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[5]);
                            }

                            var obj = new ObjectDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName,
                                AnimationName = element.AnimationName
                            };
                            MasterDictionary.ObjectDefinitions.Add((int)element.Id, obj);
                        }
                        else if (mode == "tanm")
                        {
                            var element = new ObjectsIDEElement();
                            if (parts.Length >= 8)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.AnimationName = parts[3];
                                element.DrawDistance = float.Parse(parts[4]);
                                element.Flags = (IDEFlags)UInt32.Parse(parts[5]);
                                element.TimeOn = UInt32.Parse(parts[6]);
                                element.TimeOff = UInt32.Parse(parts[7]);
                            }

                            var obj = new ObjectDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName,
                                AnimationName = element.AnimationName
                            };
                            MasterDictionary.ObjectDefinitions.Add((int)element.Id, obj);
                        }
                        else if (mode == "cars")
                        {
                            var element = new CarsIDEElement();
                            if (parts.Length >= 11)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.Type = VehicleTypeFromString(parts[3]);
                                element.Handling = parts[4];
                                element.GXTKey = parts[5];
                                element.Anims = parts[6];
                                element.Class = VehicleClassFromString(parts[7]);
                                element.Frequency = parts[8];
                                element.Unknown = parts[9];
                                element.Comprules = parts[10];
                            }
                            if (parts.Length >= 15)
                            {
                                element.WheelId = Int32.Parse(parts[11]);
                                element.WheelScaleFront = float.Parse(parts[12]);
                                element.WheelScaleRear = float.Parse(parts[13]);
                                element.WheelUpgradeClass = Int32.Parse(parts[14]);
                            }

                            var veh = new VehicleDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName,
                                Type = element.Type,
                                WheelScaleFront = element.WheelScaleFront,
                                WheelScaleRear = element.WheelScaleRear
                            };
                            MasterDictionary.VehicleDefinitions.Add((int)element.Id, veh);
                        }
                        else if (mode == "weap")
                        {
                            var element = new WeaponsIDEElement();
                            if (parts.Length >= 6)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.AnimationName = parts[3];
                                element.MeshCount = UInt32.Parse(parts[4]);
                                element.DrawDistance = UInt32.Parse(parts[5]);
                            }

                            var wep = new WeaponDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName,
                                AnimationName = element.AnimationName
                            };
                            MasterDictionary.WeaponDefinitions.Add((int)element.Id, wep);
                        }
                        else if (mode == "peds")
                        {
                            var element = new PedsIDEElement();
                            if (parts.Length >= 14)
                            {
                                element.Id = UInt32.Parse(parts[0]);
                                element.ModelName = parts[1];
                                element.TextureDictionaryName = parts[2];
                                element.Type = PedestrianTypeFromString(parts[3]);
                                element.Behavior = parts[4];
                                element.AnimationGroup = parts[5];
                                element.CarsCanDrive = parts[6];
                                element.Flags = UInt32.Parse(parts[7]);
                                element.AnimationFile = parts[8];
                                element.Radio1 = UInt32.Parse(parts[9]);
                                element.Radio2 = UInt32.Parse(parts[10]);
                                element.VoiceArchive = parts[11];
                                element.Voice1 = parts[12];
                                element.Voice2 = parts[13];
                            }

                            var skin = new SkinDefinition()
                            {
                                ModelName = element.ModelName,
                                TextureDictionaryName = element.TextureDictionaryName,
                                AnimationGroupName = element.AnimationGroup,
                                Type = element.Type
                            };
                            MasterDictionary.SkinDefinitions.Add((int)element.Id, skin);
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
