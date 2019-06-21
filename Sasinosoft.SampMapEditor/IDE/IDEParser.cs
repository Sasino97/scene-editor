/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.IO;
using System.Text;
using Sasinosoft.SampMapEditor.Vehicles;

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
        /// <returns>A new collection of IDE objects containing the TXD data read from the stream.</returns>
        public ItemDefinitionDictionary Parse(Stream stream, int offset, int length)
        {
            byte[] data = new byte[length];
            stream.Read(data, offset, length);
            return Parse(data, out int _);
        }

        /// <summary>
        /// Parses a RenderWare TXD texture dictionary from a stream.
        /// </summary>
        /// <param name="stream">The stream to read.</param>
        /// <returns>A new RenderWareTextureDictionary object containing the TXD data read from the stream.</returns>
        public ItemDefinitionDictionary Parse(Stream stream)
        {
            int offset = 0;
            long size = stream.Length;
            return Parse(stream, offset, (int)size);
        }

        /// <summary>
        /// Parses a RenderWare TXD texture dictionary from a file.
        /// </summary>
        /// <param name="fileName">The name of the file to read.</param>
        /// <returns>A new RenderWareTextureDictionary object containing the TXD data read from the file stream.</returns>
        public ItemDefinitionDictionary Parse(string fileName)
        {
            return Parse(new FileStream(fileName, FileMode.Open, FileAccess.Read));
        }

        /// <summary>
        /// Parses a RenderWare TXD texture dictionary from a byte array.
        /// </summary>
        /// <param name="data">The TXD data to read in binary form.</param>
        /// <returns>A new RenderWareTextureDictionary object containing the parsed TXD data.</returns>
        public ItemDefinitionDictionary Parse(byte[] data, out int errorCount)
        {
            var dictionary = new ItemDefinitionDictionary();

            string parserData = Encoding.UTF8.GetString(data);
            string[] lines = parserData.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            string mode = null;
            errorCount = 0;

            //var memInfo = typeof(VehicleType).GetMember(FunkyAttributesEnum.NameWithoutSpaces1.ToString());
            //var mem = memInfo.FirstOrDefault(m => m.DeclaringType == type);
            //var attributes = mem.GetCustomAttributes(typeof(DescriptionAttribute), false);
            //var description = ((DescriptionAttribute)attributes[0]).Description;

            foreach (string line in lines)
            {
                if (line.StartsWith("#", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                if (mode == null)
                {
                    if (line.StartsWith("objs", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("tobj", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("anim", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("tanm", StringComparison.InvariantCultureIgnoreCase) || 
                        line.StartsWith("cars", StringComparison.InvariantCultureIgnoreCase) ||
                        line.StartsWith("weap", StringComparison.InvariantCultureIgnoreCase))
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

                    var parts = line.Split(',', ' ');
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
                        dictionary.Objects.Add((int)element.Id, obj);
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
                        dictionary.Objects.Add((int)element.Id, obj);
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
                        dictionary.Objects.Add((int)element.Id, obj);
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
                        dictionary.Objects.Add((int)element.Id, obj);
                    }
                    else if (mode == "cars")
                    {
                        var element = new CarsIDEElement();
                        if (parts.Length >= 15)
                        {
                            element.Id = UInt32.Parse(parts[0]);
                            element.ModelName = parts[1];
                            element.TextureDictionaryName = parts[2];
                            element.Type = (VehicleType)UInt32.Parse(parts[3]);
                            element.Handling = parts[4];
                            element.GXTKey = parts[5];
                            element.Anims = parts[6];
                            element.Class = (VehicleClass)UInt32.Parse(parts[7]);
                            element.Frequency = UInt32.Parse(parts[8]);
                            element.Unknown = UInt32.Parse(parts[9]);
                            element.Comprules = UInt32.Parse(parts[10]);
                            element.WheelId = UInt32.Parse(parts[11]);
                            element.WheelScaleFront = float.Parse(parts[12]);
                            element.WheelScaleRear = float.Parse(parts[13]);
                            element.WheelUpgradeClass = UInt32.Parse(parts[14]);
                        }

                        var veh = new VehicleDefinition()
                        {
                            ModelName = element.ModelName,
                            TextureDictionaryName = element.TextureDictionaryName,
                            Type = element.Type,
                            WheelScaleFront = element.WheelScaleFront,
                            WheelScaleRear = element.WheelScaleRear
                        };
                        dictionary.Vehicles.Add((int)element.Id, veh);
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
                        dictionary.Weapons.Add((int)element.Id, wep);
                    }
                }
            }
            return dictionary;
        }
    }
}
