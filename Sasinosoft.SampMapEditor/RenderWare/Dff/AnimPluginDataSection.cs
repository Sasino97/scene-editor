/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    public class AnimPluginDataSection : DataSection
    {
        public UInt32 Unknown1; // always 256
        public UInt32 BoneId; 
        public UInt32 BoneCount; 
        public UInt32 Unknown2; 
        public UInt32 Unknown3;

        public struct BoneInformation
        {
            public UInt32 Id;
            public UInt32 Index;
            public UInt32 Type;
        }
        public List<BoneInformation> Bones = new List<BoneInformation>();
    }
}
