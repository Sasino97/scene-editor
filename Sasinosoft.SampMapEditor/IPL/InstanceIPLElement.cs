/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */

namespace Sasinosoft.SampMapEditor.IPL
{
    /// <summary>
    /// Represents an "inst" IPL element.
    /// </summary>
    /// <see cref="https://gtamods.com/wiki/INST"/>
    public class InstanceIPLElement : IPLElement
    {
        public int Id;
        public string ModelName; // irrelevant
        public int Interior;
        public float X;
        public float Y;
        public float Z;
        public float RX;
        public float RY;
        public float RZ;
        public float RW;
        public int LODIndex;
    }
}
