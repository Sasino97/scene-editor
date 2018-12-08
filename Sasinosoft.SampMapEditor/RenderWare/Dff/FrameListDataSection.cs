/* 
 * This Source Code Form is subject to the terms of the Mozilla Public
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/. 
 */
using System;
using System.Collections.Generic;

namespace Sasinosoft.SampMapEditor.RenderWare.Dff
{
    // variable length
    public class FrameListDataSection : DataSection
    {
        public class FrameStruct
        {
            public float[,] RotationalMatrix;
            public float[] CoordinatesOffset;
            public UInt32 Parent;
            public UInt32 Unknown1;
        }

        public UInt32 FrameCount;
        public List<FrameStruct> FrameInformation = new List<FrameStruct>();
    }
}
