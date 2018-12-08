/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    public class MaterialSplitDataSection : DataSection
    {
        public UInt32 FaceType;
        public UInt32 SplitCount;
        public UInt32 FaceCount;

        public struct Split
        {
            public UInt32 IndexCount;
            public UInt32 Material;
            public List<UInt32> Indices;
        }
        public List<Split> Splits = new List<Split>();
    }
}
