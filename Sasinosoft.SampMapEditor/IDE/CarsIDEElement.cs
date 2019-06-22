/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using Sasinosoft.SampMapEditor.Vehicles;

namespace Sasinosoft.SampMapEditor.IDE
{
    /// <summary>
    /// Represents a "cars" IDE element.
    /// </summary>
    /// <see cref="https://gtamods.com/wiki/CARS_(IDE_Section)"/>
    public class CarsIDEElement : IDEElement
    {
        public UInt32 Id;
        public string ModelName;
        public string TextureDictionaryName;
        public VehicleType Type;
        public string Handling;
        public string GXTKey;
        public string Anims;
        public VehicleClass Class;
        public string Frequency;
        public string Unknown;
        public string Comprules;
        public Int32 WheelId;
        public float WheelScaleFront;
        public float WheelScaleRear;
        public Int32 WheelUpgradeClass;
    }
}
