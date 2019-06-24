/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

namespace Sasinosoft.SampMapEditor.IPL
{
    /// <summary>
    /// Represents a "cars" IPL element.
    /// </summary>
    /// <see cref="https://gtamods.com/wiki/CARS_(IPL_Section)"/>
    public class CarsIPLElement : IPLElement
    {
        public float X;
        public float Y;
        public float Z;
        public float Angle;
        public int Id;
        public int Color1;
        public int Color2;
        public int ForceSpawn;
        public int Alarm;
        public int DoorLock;
        public int Unknown1;
        public int Unknown2;
    }
}
