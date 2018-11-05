/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    // 28 bytes
    public class MaterialDataSection : DataSection
    {
        public UInt32 Unknown1;
        public struct MaterialColor
        {
            public byte R;
            public byte G;
            public byte B;
            public byte A;
        }
        public MaterialColor Color;
        public UInt32 Unknown2;
        public UInt32 TextureCount;
        public float[] Unknown3;
    }
}
